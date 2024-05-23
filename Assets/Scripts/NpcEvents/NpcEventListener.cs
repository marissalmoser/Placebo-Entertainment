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
    [SerializeField] private NpcEvent _npcEvent;
    [SerializeField] private string _targetEventTag;
    [SerializeField] private UnityEvent _onEventTriggered;

    /// <summary>
    /// Called by NpcEvent to invoke a local UnityEvent if the incoming event tag
    /// matches to target.
    /// </summary>
    public void OnEventTriggered(string eventTag)
    {
        if (eventTag.Equals(_targetEventTag))
        {
            _onEventTriggered.Invoke();
        }
    }

    private void OnEnable()
    {
        _npcEvent.AddListener(this);
    }

    private void OnDisable()
    {
        _npcEvent.RemoveListener(this);
    }
}
