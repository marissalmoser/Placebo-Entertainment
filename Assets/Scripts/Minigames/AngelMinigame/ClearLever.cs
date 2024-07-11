/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: June 24, 2024
*    Description: Used by the arrows and numbers stations to clear player
*    inputs. Invokes an action on the connected station when pulled.

*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlaceboEntertainment.UI;

public class ClearLever : LeverInteraction
{
    [SerializeField] private StationBehavior _station;

    /// <summary>
    /// Defaults lever to up.
    /// </summary>
    private void Awake()
    {
        SetLever(true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    public override void SetLever(bool input)
    {
        base.SetLever(input);

        if (!input)
        {
            _station.InvokeClearEvent();
            Invoke(nameof(SetLeverDelayHelper), 0.3f);
        }
    }

    /// <summary>
    /// Invoked to reset lever on a delay.
    /// </summary>
    private void SetLeverDelayHelper()
    {
        SetLever(true);
    }

    /// <summary>
    /// Changes the lever interaction prompt to say clear instead of lever.
    /// </summary>
    public override void DisplayInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(true, "CLEAR");
    }
}
