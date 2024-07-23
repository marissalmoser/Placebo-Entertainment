/******************************************************************
*    Author: Marissa MOser
*    Contributors: 
*    Date Created: 7/17/24
*    Description: This script will manage the different states of the generator minigame.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorMinigameManager : MonoBehaviour
{
    [SerializeField] private GameObject _ripcord;
    [SerializeField] private GameObject _gearBottom;
    [SerializeField] private GameObject _gears;

    private InventoryHolder _inventoryHolder;
    [SerializeField] private InventoryItemData _wrenchItemData;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        _inventoryHolder = player.GetComponent<InventoryHolder>();
    }

    /// <summary>
    /// Function called by event listener on generator minigame parent object. This
    /// function starts the generator/coward minigame from the beginning or spark
    /// stage based on if the player has the wrench in their inventory.
    /// </summary>
    public void StartMinigame()
    {
        //if player has wrench, start sparks and enable wrench object
        if(_inventoryHolder.InventorySystem.ContainsItem(_wrenchItemData, out _))
        {
            //TODO: set ripcord lights and gears to be visually completed
            _ripcord.GetComponent<RipcordBehavior>().StopRipcordSteam();
            _gears.SetActive(true);
            _gearBottom.GetComponent<GearCompletionCheck>().StartSparksSection();
            _gearBottom.GetComponent<GearCompletionCheck>().StartWithBypass();
        }

        //else enable ripcord interaction
        else
        {
            _ripcord.GetComponent<RipcordBehavior>().GameStart();
        }
    }
}
