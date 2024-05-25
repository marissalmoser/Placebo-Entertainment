/******************************************************************
*    Author: Marissa Moser
*    Contributors: 
*    Date Created: May 22, 2024
*    Description: Contains the functionality for the door when it is interacted with.

*******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlaceboEntertainment.UI;

public class DoorBehavior : Interactable
{
    private bool _isOpened = false;
    [SerializeField] private bool _isLocked;
    private Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public override void Interact(GameObject player)
    {
        if (!_isLocked && !_isOpened)
        {
            OpenDoor();
        }
    }
    public override void DisplayInteractUI()
    {
        //TabbedMenu.Instance.ToggleInteractPrompt(true, "DOOR");
    }


    private void OpenDoor()
    {
        _isOpened = true;
        _anim.SetTrigger("_openDoor");
        GetComponent<BoxCollider>().enabled = false;
        //disabe UI - should I just remove the interactable script from the door?
        //TabbedMenu.Instance.ToggleInteractPrompt(false);
    }

    public void UnlockDoor()
    {
        _isLocked = false;
        
        //if unlocking door should open the door:
        //OpenDoor();
    }
}
