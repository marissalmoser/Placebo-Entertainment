using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlaceboEntertainment.UI;

public class ArrowButton : ButtonInteraction
{
    [SerializeField] private Station3.Direction _arrowDirection;

   
    public override void Interact(GameObject player)
    {
        _buttonPress.transform.position = _downPosition.transform.position;
        _canBePressed = false;
        StartCoroutine(ButtonCooldown());

        if (IsInteractable)
        {
            Station3.ButtonClicked?.Invoke(_arrowDirection);
        }
    }

    
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
