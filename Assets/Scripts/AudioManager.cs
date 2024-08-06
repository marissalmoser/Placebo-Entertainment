using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private static Dictionary<EventReference, List<EventInstance>> eventInstancesDict = new();
    private static Dictionary<EventInstance, GameObject> eventInstanceWorldDict = new();
    private static Dictionary<EventInstance, Dictionary<string, FMOD.Studio.PARAMETER_ID>> paramRefDict = new();

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        var bus = FMODUnity.RuntimeManager.GetBus("bus:/");
        if (bus.isValid())
        {
            bus.stopAllEvents(STOP_MODE.IMMEDIATE);
        }
    }

    public static void PlaySound(EventReference eventReference, Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(eventReference, position);
    }

    public static void PlaySound(EventReference eventReference, GameObject gameObject)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(eventReference, gameObject);
    }

    public static EventInstance PlaySound(FMODUnity.EventReference eventReference, Vector3 origin, Rigidbody rb,
        params ParamRef[] parameters)
    {
        if (eventReference.IsNull)
        {
            return default;
        }

        EventInstance instance = default;
        if (eventInstancesDict.ContainsKey(eventReference))
        {
            bool foundNew = false;
            foreach (var eventInstance in eventInstancesDict[eventReference])
            {
                eventInstance.getPlaybackState(out var state);
                if (state is not (PLAYBACK_STATE.STOPPED or PLAYBACK_STATE.STOPPING)) continue;
                foundNew = true;
                instance = eventInstance;
                Debug.Log("found a new audio source to use!");
                break;
            }

            if (!foundNew)
            {
                instance = FMODUnity.RuntimeManager.CreateInstance(eventReference);
                eventInstancesDict[eventReference].Add(instance);
            }
        }
        else
        {
            instance = FMODUnity.RuntimeManager.CreateInstance(eventReference);
            eventInstancesDict.Add(eventReference, new List<EventInstance>());
            eventInstancesDict[eventReference].Add(instance);
        }

        GameObject attachObject;
        if (eventInstanceWorldDict.TryGetValue(instance, out var value))
        {
            attachObject = value;
        }
        else
        {
            attachObject = new GameObject(eventReference.Path);
            eventInstanceWorldDict.Add(instance, attachObject);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, attachObject.transform, rb ? rb : null);
        }

        attachObject.transform.parent = rb ? rb.transform : null;
        attachObject.transform.position = origin;



        if (parameters != null)
        {
            if (!paramRefDict.TryGetValue(instance, out var paramDict))
            {
                instance.getDescription(out var eventDescription);
                paramRefDict.Add(instance, new Dictionary<string, PARAMETER_ID>());
                foreach (var paramRef in parameters)
                {
                    eventDescription.getParameterDescriptionByName(paramRef.Name, out var paramDescription);
                    var handle = paramDescription.id;
                    paramRefDict[instance].Add(paramRef.Name, handle);
                    instance.setParameterByID(handle, paramRef.Value);
                }
            }
            else
            {
                foreach (var paramRef in parameters)
                {
                    var handle = paramDict[paramRef.Name];
                    instance.setParameterByID(handle, paramRef.Value);
                }
            }
        }

        instance.start();
        return instance;
    }
    
     public static EventInstance PlaySound(FMODUnity.EventReference eventReference, Vector3 origin,
        params ParamRef[] parameters)
    {
        if (eventReference.IsNull)
        {
            return default;
        }

        EventInstance instance = default;
        if (eventInstancesDict.ContainsKey(eventReference))
        {
            bool foundNew = false;
            foreach (var eventInstance in eventInstancesDict[eventReference])
            {
                eventInstance.getPlaybackState(out var state);
                if (state is not (PLAYBACK_STATE.STOPPED or PLAYBACK_STATE.STOPPING)) continue;
                foundNew = true;
                instance = eventInstance;
                Debug.Log("found a new audio source to use!");
                break;
            }

            if (!foundNew)
            {
                instance = FMODUnity.RuntimeManager.CreateInstance(eventReference);
                eventInstancesDict[eventReference].Add(instance);
            }
        }
        else
        {
            instance = FMODUnity.RuntimeManager.CreateInstance(eventReference);
            eventInstancesDict.Add(eventReference, new List<EventInstance>());
            eventInstancesDict[eventReference].Add(instance);
        }

        GameObject attachObject;
        if (eventInstanceWorldDict.TryGetValue(instance, out var value))
        {
            attachObject = value;
        }
        else
        {
            attachObject = new GameObject(eventReference.Path);
            eventInstanceWorldDict.Add(instance, attachObject);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, null);
        }
        
        attachObject.transform.position = origin;

        if (parameters != null)
        {
            if (!paramRefDict.TryGetValue(instance, out var paramDict))
            {
                instance.getDescription(out var eventDescription);
                paramRefDict.Add(instance, new Dictionary<string, PARAMETER_ID>());
                foreach (var paramRef in parameters)
                {
                    eventDescription.getParameterDescriptionByName(paramRef.Name, out var paramDescription);
                    var handle = paramDescription.id;
                    paramRefDict[instance].Add(paramRef.Name, handle);
                    instance.setParameterByID(handle, paramRef.Value);
                }
            }
            else
            {
                foreach (var paramRef in parameters)
                {
                    var handle = paramDict[paramRef.Name];
                    instance.setParameterByID(handle, paramRef.Value);
                }
            }
        }

        instance.start();
        return instance;
    }

    public static void ModifyPlayingSound(EventInstance instance, params ParamRef[] parameters)
    {
        //TODO null checks
        foreach (var paramRef in parameters)
        {
            var handle = paramRefDict[instance][paramRef.Name];
            instance.setParameterByID(handle, paramRef.Value);
        }
    }

    public static void StopSound(EventInstance instance, bool fadeOut = false)
    {
        instance.stop(fadeOut ? STOP_MODE.ALLOWFADEOUT : STOP_MODE.IMMEDIATE);
    }

}