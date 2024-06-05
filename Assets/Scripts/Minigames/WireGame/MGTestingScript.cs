/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: 5/29/24
*    Description: Class used for testing wires minigame without interaction
*    system.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGTestingScript : MonoBehaviour
{
    [SerializeField] MGWire[] _wiresArray;
    [SerializeField] GameObject _player;
    [SerializeField] NpcEvent _minigameStartEvent;

    // Keys 1-3 call Interact() on a chosen wire
    // M triggers the start minigame event (normally triggered by Robot)
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _wiresArray[0].Interact();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _wiresArray[1].Interact();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _wiresArray[2].Interact();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            _minigameStartEvent.TriggerEvent(NpcEvent.NpcEventTags.Robot);
            Debug.Log("Start minigame event triggered");
        }
    }
}
