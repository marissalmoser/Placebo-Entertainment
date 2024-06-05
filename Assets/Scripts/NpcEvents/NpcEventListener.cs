/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: 5/16/24
*    Description: Add this script to any object that needs to listen
*       for certain events to occur. Will invoke methods on various
*       components when event is recieved.
*    Reference/Source: https://youtu.be/J01z1F-du-E?t=635 (10:32 - 12:45)
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NpcEventListener : MonoBehaviour
{
    /// <summary>
    /// This struct contains an event to listen for and a corresponding UnityEvent
    /// to invoke when that event is triggered
    /// </summary>
    [System.Serializable]
    private struct TargetEvent
    {
        [SerializeField] private NpcEvent _npcEvent;
        [SerializeField] private NpcEventTags _npcEventTag;
        [SerializeField] private UnityEvent _onEventTriggered;

        public NpcEvent NpcEvent { get => _npcEvent; }
        public NpcEventTags NpcEventTag { get => _npcEventTag; }

        public void InvokeUnityEvents()
        {
            _onEventTriggered.Invoke();
        }
    }

    [SerializeField] private TargetEvent[] _eventsToListenFor; 

    //[SerializeField] private NpcEvent _npcEvent;
    //[SerializeField] private string _targetEventTag;
    //[SerializeField] private UnityEvent _onEventTriggered;

    /// <summary>
    /// Called by NpcEvent to invoke a local UnityEvent if the incoming event tag
    /// matches to target.
    /// </summary>
    public void OnEventTriggered(NpcEventTags eventTag, NpcEvent triggeredEvent)
    {
        foreach (TargetEvent targetEvent in _eventsToListenFor)
        {
            if (targetEvent.NpcEventTag == eventTag && targetEvent.NpcEvent == triggeredEvent)
            {
                targetEvent.InvokeUnityEvents();
            }
        }
    }

    /// <summary>
    /// Subscribes to all events in list
    /// </summary>
    private void OnEnable()
    {
        foreach (TargetEvent targetEvent in _eventsToListenFor)
        {
            targetEvent.NpcEvent.AddListener(this);
        }
    }

    /// <summary>
    /// Unsubscribes from all events in list
    /// </summary>
    private void OnDisable()
    {
        foreach (TargetEvent targetEvent in _eventsToListenFor)
        {
            targetEvent.NpcEvent.RemoveListener(this);
        }
    }
}
