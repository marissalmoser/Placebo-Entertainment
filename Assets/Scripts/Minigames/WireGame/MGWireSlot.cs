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

    [SerializeField] private MGWire.EWireID _matchingWire;


  /*private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(0.1f, 0.1f, 0.1f));
    }*/

    /// <summary>
    /// Checks to see if the wire plaaced in the slot was the corrent wire
    /// </summary>
    /// <param name="wire"></param>
    public void CheckWire(MGWire wire)
    {
        Assert.IsNotNull(wire, "Make sure the object passed in is a " +
            "valid wire");

        if(wire.WireID.Equals(_matchingWire))
        {
            CorrectWire?.Invoke();
        }

        // TODO: Add logic for if the wire was incorrect match
    }
}
