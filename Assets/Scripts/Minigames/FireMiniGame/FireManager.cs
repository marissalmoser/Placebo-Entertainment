/*****************************************************************************
// File Name :         FireManager.cs
// Author :            Andrea Swihart-DeCoster
// Creation Date :     08/03/2024
//
// Brief Description : Destroys fire objects
*****************************************************************************/

using PlaceboEntertainment.UI;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    [SerializeField] private NpcEvent _minigameEndEvent;

    [SerializeField] private GameObject _fireAlarmLight;

    [SerializeField] private FMODUnity.EventReference sirenEvent;
    
    private List<FireBehavior> _fires;

    private TabbedMenu _tabbedMenu;
    private GameObject _fishNPC;
    private EventInstance _sirenInstance;
    private void Start()
    {
        _tabbedMenu = TabbedMenu.Instance;
        _sirenInstance = AudioManager.PlaySound(sirenEvent, _fireAlarmLight.transform.position);
    }

    public void StartMinigame()
    {
        _fires = FindObjectsOfType<FireBehavior>().ToList();
        InitializeFireSizes();

        _fishNPC = FindObjectOfType<FishNpc>().gameObject;
        _fishNPC.SetActive(false);
    }
    
    /// <summary>
    /// Sets the initial fire sizes when the minigame starts
    /// </summary>
    private void InitializeFireSizes()
    {
        foreach(FireBehavior fire in _fires)
        {
            fire.SetStartFireSize();
        }
    }

    private void OnEnable()
    {
        FireBehavior.OnFireExtinguished += OnFireDestroyed;
    }

    private void OnDisable()
    {
        FireBehavior.OnFireExtinguished -= OnFireDestroyed;
    }

    private void OnFireDestroyed(FireBehavior fire)
    {
        _fires.Remove(fire);

        Destroy(fire.gameObject);

        // MINIGAME ENDS HERE
        if (_fires.Count <= 0)
        {
            _tabbedMenu.ToggleWaterMeter(false);

            EndMinigame();

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Ends the minigame
    /// </summary>
    private void EndMinigame()
    {
        _fishNPC.SetActive(true);
        _fireAlarmLight.SetActive(false);
        AudioManager.StopSound(_sirenInstance);

        Destroy(FishHoseBehavior.Instance.gameObject);

        _minigameEndEvent.TriggerEvent(NpcEventTags.Fish);
        Destroy(gameObject);
    }
}
