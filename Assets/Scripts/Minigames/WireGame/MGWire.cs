/*****************************************************************************
// File Name :         MGWire.cs
// Author :            Andrea Swihart-DeCoster
// Creation Date :     05/21/24
//
// Brief Description : Controls the wire logic
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class MGWire : MonoBehaviour
{
    public EWireNum WireNum;

    public enum EWireNum
    {
        ONE, TWO, THREE
    }

    private void OnInteract()
    {
        // TODO: When the player interacts with a wire, make the end
        // kinematic and have it follow the players direction
    }

    private void OnDrop()
    {
        // TODO: If the wire is in a slot, call PlaceWire()
        // Otherwise, make the wire non-kinematic
    }

    private void PlaceWire()
    {
        // TODO: What happens when the player puts the wire in a spot?
    }    
}