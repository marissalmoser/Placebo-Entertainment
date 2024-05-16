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
    [SerializeField] private NpcEvent npcEvent;
    [SerializeField] private UnityEvent onEventTriggered;

    /// <summary>
    /// Called by NpcEvent to invoke a local UnityEvent.
    /// </summary>
    public void OnEventTriggered()
    {
        onEventTriggered.Invoke();
    }

    private void OnEnable()
    {
        npcEvent.AddListener(this);
    }

    private void OnDisable()
    {
        npcEvent.RemoveListener(this);
    }
}
