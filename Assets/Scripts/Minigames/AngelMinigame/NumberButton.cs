/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: June 24, 2024
*    Description: The script for number buttons used in Station 4 of the Angel 
*    minigame. Extends functionality from ButtonInteraction. 
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlaceboEntertainment.UI;

public class NumberButton : ButtonInteraction
{
    [SerializeField] private int _buttonNumber;

    /// <summary>
    /// Invokes an action on station 4 when clicked
    /// </summary>
    /// <param name="player">Player interacting with button</param>
    public override void Interact(GameObject player)
    {
        _buttonPress.transform.position = _downPosition.transform.position;
        _canBePressed = false;
        StartCoroutine(ButtonCooldown());

        if (IsInteractable)
        {
            Station4.NumberButtonClicked?.Invoke(_buttonNumber);
        }
    }

    /// <summary>
    /// Shows UI prompt for number button
    /// </summary>
    public override void DisplayInteractUI()
    {
        if (IsInteractable)
        {
            TabbedMenu.Instance.ToggleInteractPrompt(true, _buttonNumber.ToString());
        }
        else
        {
            TabbedMenu.Instance.ToggleInteractPrompt(true, "BUTTON");
        }
    }
}
