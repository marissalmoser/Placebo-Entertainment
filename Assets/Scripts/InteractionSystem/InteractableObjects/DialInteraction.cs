/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: June 24, 2024
*    Description: Contains the functionality for dials. Dials start in
*    the Up position and rotate 90 degrees clockwise when interacted with.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlaceboEntertainment.UI;

public class DialInteraction : MonoBehaviour, IInteractable
{
    public enum DialDirection
    {
        Up,
        Right,
        Down,
        Left
    }

    public DialDirection _direction { get; private set; }

    /// <summary>
    /// Sets dial direction
    /// </summary>
    private void Awake()
    {
        _direction = DialDirection.Up;
    }

    /// <summary>
    /// Rotates the dial when interacted with.
    /// </summary>
    /// <param name="player"></param>
    public virtual void Interact(GameObject player)
    {
        int newDirection = (int) _direction + 1;
        newDirection %= 4;
        _direction = (DialDirection) newDirection;

        transform.Rotate(new Vector3(0, 90, 0));
    }

    /// <summary>
    ///  Displays the specific UI prompt
    /// </summary>
    public void DisplayInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(true, "DIAL");
    }

    /// <summary>
    /// Hides the specific UI prompt
    /// </summary>
    public void HideInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(false);
    }
}
