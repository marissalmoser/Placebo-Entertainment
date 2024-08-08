/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: June 20, 2024
*    Description: The script for arrow buttons used in Station 3 of the Angel 
*    minigame. Extends functionality from ButtonInteraction. 
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlaceboEntertainment.UI;

public class ArrowButton : ButtonInteraction
{
    [SerializeField] private Station3.Direction _arrowDirection;

   /// <summary>
   /// Invokes an action on station 3 when clicked
   /// </summary>
   /// <param name="player">Player interacting with button</param>
    public override void Interact(GameObject player)
    {
        AudioManager.PlaySound(interactEvent, _buttonPress.transform.position);
        _buttonPress.transform.position = _downPosition.transform.position;
        _canBePressed = false;
        StartCoroutine(ButtonCooldown());

        if (IsInteractable)
        {
            Station3.ButtonClicked?.Invoke(_arrowDirection);
        }
    }

    /// <summary>
    /// Shows UI prompt for directional arrow
    /// </summary>
    public override void DisplayInteractUI()
    {
        if (IsInteractable)
        {
            TabbedMenu.Instance.ToggleInteractPrompt(true, _arrowDirection.ToString());
        }
        else
        {
            TabbedMenu.Instance.ToggleInteractPrompt(true, "BUTTON");
        }
    }
}
