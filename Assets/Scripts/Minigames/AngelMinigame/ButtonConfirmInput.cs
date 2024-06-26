/******************************************************************
*    Author: Marissa Moser
*    Contributors: 
*    Date Created: June 19, 2024
*    Description: This button class and prefab will be used on every station to 
*    confirm the interactable's input. It derives from the base button class. It can 
*    be interacted with when IsInteractable is false, but will have no functionality
*    other than moving the button when pressed until it is made interactable for the
*    minigame.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlaceboEntertainment.UI;

public class ButtonConfirmInput : ButtonInteraction
{
    /// <summary>
    /// presses the button when interacted with and invokes the action to check the
    /// state of the game.
    /// </summary>
    /// <param name="player"></param>
    public override void Interact(GameObject player)
    {
        _buttonPress.transform.position = _downPosition.transform.position;
        _canBePressed = false;
        StartCoroutine(ButtonCooldown());

        //invoke action to check states if this station is active
        if (IsInteractable)
        {
            AngelMinigameManager.CheckState?.Invoke();
        }
    }

    /// <summary>
    /// Shows the UI prompt to confirm or button based on if the minigame is active.
    /// </summary>
    public override void DisplayInteractUI()
    {
        if (IsInteractable)
        {
            TabbedMenu.Instance.ToggleInteractPrompt(true, "CONFIRM");
        }
        else
        {
            TabbedMenu.Instance.ToggleInteractPrompt(true, "BUTTON");
        }
    }
}
