/******************************************************************
*    Author: Marissa Moser
*    Contributors: 
*    Date Created: May 24, 2024
*    Description: 
*       Will derive from monobehaviour so its children classes can be placed on game objects
            Contains a function Interact(GameObject player) that will be overridden
            in its children classes to contain the functionality of the interaction
            (such as picking up an item, or triggering an NPC’s dialogue to be
            brought up, etc.) The player parameter is there so that each interactable
            object has a reference to the player built into the function, and we
            shouldn’t have to find the player.

*******************************************************************/

using System.Collections;
using System.Collections.Generic;
using PlaceboEntertainment.UI;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void Interact(GameObject player)
    {
        print("interact input detected");
    }

    public virtual void DisplayInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(true, "TEST");
    }
}
