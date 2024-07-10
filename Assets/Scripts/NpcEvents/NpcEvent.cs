/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: 5/16/24
*    Description: A template event script to create ScriptableObject events
*       from. Tracks listeners and updates them when the event triggers.
*    Reference/Source: https://youtu.be/J01z1F-du-E?t=635 (10:32 - 12:45)
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum for event tags used in differentiating events of the same type
/// </summary>
public enum NpcEventTags
{
    Angel,
    Coward,
    Fish,
    Game,
    Goop,
    Robot,
}

[CreateAssetMenu(menuName = "NPC Event")]
public class NpcEvent : ScriptableObject
{
    private List<NpcEventListener> _eventListeners = new List<NpcEventListener>();

    /// <summary>
    /// Called from another script to trigger this event. Event tags serve to 
    /// distinguish events related to different NPCs, quests, items, etc.
    /// </summary>
    public void TriggerEvent(NpcEventTags eventTag)
    {
        for (int i = 0; i < _eventListeners.Count; ++i)
        {
            _eventListeners[i].OnEventTriggered(eventTag, this);
        }
    }

    /// <summary>
    /// Allows NpcEventListeners to register themselves.
    /// </summary>
    public void AddListener(NpcEventListener newListener)
    {
        _eventListeners.Add(newListener);
    }

    /// <summary>
    /// Allows NpcEventListeners to unregister themselves
    /// </summary>
    public void RemoveListener(NpcEventListener listenerToRemove)
    {
        _eventListeners.Remove(listenerToRemove);
    }
}
