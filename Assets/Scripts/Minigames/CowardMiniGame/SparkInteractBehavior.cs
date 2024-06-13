/*****************************************************************************
// File Name :         WrenchBehavior.cs
// Author :            Marissa Moser
// Contributors :      
// Creation Date :     6/5/2024
//
// Brief Description : This script manages the players interaction with the sparks
    during the coward minigame.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlaceboEntertainment.UI;

public class SparkInteractBehavior : MonoBehaviour, IInteractable
{
    /// <summary>
    /// When the player interacts with a spark, this function informs the wrench object
    /// of the smack and then destroys the spark.
    /// </summary>
    /// <param name="player"></param>
    public void Interact(GameObject player)
    {
        WrenchBehavior.SparkSmackedAction?.Invoke();
        Destroy(gameObject);
    }

    /// <summary>
    /// Displays UI prompt for sparks
    /// </summary>
    public void DisplayInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(true, "SMACK");
    }

    /// <summary>
    /// Hides UI prompt for sparks
    /// </summary>
    public void HideInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(false);
    }
}
