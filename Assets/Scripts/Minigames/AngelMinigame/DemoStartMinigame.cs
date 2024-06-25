/******************************************************************
*    Author: Marissa Moser
*    Contributors: 
*    Date Created: June 20, 2024
*    Description: This is demo script to contain the functionality to start the
*    angel minigame.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlaceboEntertainment.UI;

public class DemoStartMinigame : ButtonInteraction
{
    [SerializeField] private GameObject _manager;

    public override void Interact(GameObject player)
    {
        base.Interact(player);

        _manager.GetComponent<AngelMinigameManager>().StartMinigame();
        IsInteractable = false;
    }

    public override void DisplayInteractUI()
    {
        if (IsInteractable)
        {
            TabbedMenu.Instance.ToggleInteractPrompt(true, "START");
        }
    }
}
