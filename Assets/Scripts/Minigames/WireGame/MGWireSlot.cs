/*****************************************************************************
// File Name :         MGWireSlot.cs
// Author :            Andrea Swihart-DeCoster
// Creation Date :     05/21/24
//
// Brief Description : Controls the logic for the wire attachment slot.
*****************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MGWireSlot : MonoBehaviour
{
    public static Action CorrectWire;

    [SerializeField] private MGWire.EWireNum _matchingWire;

    /// <summary>
    /// Checks to see if the wire was correct
    /// </summary>
    /// <param name="wire"></param>
    private void CheckWire(ref MGWire wire)
    {
        Assert.IsNotNull(wire, "Make sure the object passed in is a valid wire");

        if(wire.WireNum.Equals(_matchingWire))
        {
            CorrectWire?.Invoke();
        }

        // TODO: Wire was incorrect match

    }
}
