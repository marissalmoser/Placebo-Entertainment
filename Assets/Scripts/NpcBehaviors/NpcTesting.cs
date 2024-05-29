/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: 5/28/24
*    Description: Class for testing NPCs. Allows for calling fucntions
*    on an assigned BaseNpc class using keyboard inputs.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcTesting : MonoBehaviour
{
    [SerializeField] private BaseNpc _npcToTest;
    [SerializeField] private NpcEvent _eventToTrigger;
    [SerializeField] private string _eventTag;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            _npcToTest.CheckForStateChange();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Acquired bypass item");
            _npcToTest.CollectedBypassItem();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            _npcToTest.Interact(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            _npcToTest.Interact(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            _npcToTest.Interact(2);

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Event triggered");
            _eventToTrigger.TriggerEvent(_eventTag);
        }
    }
}