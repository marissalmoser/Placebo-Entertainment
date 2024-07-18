/******************************************************************
*    Author: Marissa Moser
*    Contributors: 
*    Date Created: June 18, 2024
*    Description: Contains the base functionality for buttons. Each station that
*    requires a button can use a script that derives from this.

*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlaceboEntertainment.UI;

public class ButtonInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactPromptText = "BUTTON";

    [SerializeField] protected GameObject _buttonPress;
    [SerializeField] protected GameObject _upPosition;
    [SerializeField] protected GameObject _downPosition;
    protected bool _canBePressed;
    public bool IsInteractable;

    [SerializeField] private float _buttonCooldownTime;

    /// <summary>
    /// Sets state of the button
    /// </summary>
    private void Start()
    {
        _canBePressed = true;
    }

    /// <summary>
    /// presses the button when interacted with.
    /// </summary>
    /// <param name="player"></param>
    public virtual void Interact(GameObject player)
    {
        if (_canBePressed && IsInteractable)
        {
            _buttonPress.transform.position = _downPosition.transform.position;
            _canBePressed = false;
            StartCoroutine(ButtonCooldown());
        }
    }

    /// <summary>
    ///  Displays the specific UI prompt
    /// </summary>
    public virtual void DisplayInteractUI()
    {
        if (IsInteractable)
        {
            TabbedMenu.Instance.ToggleInteractPrompt(true, _interactPromptText);
        }
    }

    /// <summary>
    /// Hides the specific UI prompt
    /// </summary>
    public void HideInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(false);
    }

    /// <summary>
    /// Coroutine to return the button to the up position. The time that the button
    /// stays down can be specified in the inspector.
    /// </summary>
    /// <returns></returns>
    protected IEnumerator ButtonCooldown()
    {
        yield return new WaitForSeconds(_buttonCooldownTime);

        _buttonPress.transform.position = _upPosition.transform.position;
        _canBePressed = true;
    }
}
