/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: 5/28/24
*    Description: Triggers an event when the player walks into the 
*    generator room. Used for Coward NPC.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorRoomCheck : MonoBehaviour
{
    [SerializeField] private NpcEvent _generatorRoomEvent;
    [SerializeField] private NpcEvent.NpcEventTags _eventTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _generatorRoomEvent.TriggerEvent(_eventTag);
        }
    }
}
