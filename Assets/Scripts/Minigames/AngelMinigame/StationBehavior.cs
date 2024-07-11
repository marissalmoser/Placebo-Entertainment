/******************************************************************
*    Author: Marissa Moser
*    Contributors: 
*    Date Created: June 19, 2024
*    Description: This is the base class for each station in the angel minigame. It
*    contains similar functions that each station will need such as checking the states,
*    determining what type of random int list is needed (range and size), as well as 
*    the functionality to reset the station's state and make the station confirmable.
*    When not confirmable the station will still be interactable with all the buttons
*    and levers and such, but the confirm button will not do anything but move.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _confirmButton;
    [SerializeField] private GameObject _spotlight;

    /// <summary>
    /// The confirm button calls this functin from an action in the minigame manager.
    /// Override this in each station's script with an evaluation of the state of the
    /// interactables. Return if the positions are correct or not.
    /// </summary>
    public virtual bool CheckStates()
    {
        return false;
    }

    /// <summary>
    /// Returns a list of random ints. Range and length should be specified in station
    /// specific implementations.
    /// </summary>
    /// <returns></returns>
    public virtual List<int> SetRandomToMatch()
    {
        return new List<int>();
    }

    /// <summary>
    /// Returns all interactables to their default state.
    /// </summary>
    public virtual void RestartStationState()
    {
    }

    /// <summary>
    /// Used by Stations 3 and 4 for their clear input levers.
    /// </summary>
    public virtual void InvokeClearEvent()
    { }

    /// <summary>
    /// This function makes a station's confirm button Interactable so that station
    /// can be completed.
    /// </summary>
    public void MakeStationConfirmable()
    {
        _confirmButton.GetComponent<ButtonInteraction>().IsInteractable = true;
    }

    /// <summary>
    /// This makes a stations's confirm button Uninteractable so it cannot be 
    /// interacted with.
    /// </summary>
    public void MakeStationUnconfirmable()
    {
        _confirmButton.GetComponent<ButtonInteraction>().IsInteractable = false;
    }

    /// <summary>
    /// This function allows for the minigame manager to enable and disable the spotlight.
    /// </summary>
    /// <param name="light"></param>
    public void SetSpotlight(bool light)
    {
        if(light)
        {
            _spotlight.SetActive(true);
        }
        else
        {
            _spotlight.SetActive(false);
        }
    }
}
