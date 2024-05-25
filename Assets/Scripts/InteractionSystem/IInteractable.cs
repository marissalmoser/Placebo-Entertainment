/******************************************************************
*    Author: Marissa Moser
*    Contributors: 
*    Date Created: May 25, 2024
*    Description: Interface that all interactable objects will derive from. 
*       Includes a function to Interact, and to turn on and off the UI prompt.
*       The player parameter is included on Interact() so that each interactable
        object has a reference to the player built into the function, and we
        shouldn’t have to find the player.
*******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    /// <summary>
    /// Function called from player's Interact script. Will contain that object's
    /// functionality.
    /// </summary>
    /// <param name="player"></param>
    void Interact(GameObject player);

    void DisplayInteractUI();

    void HideInteractUI();
}
