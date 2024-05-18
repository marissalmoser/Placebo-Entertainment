using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class MGWire : MonoBehaviour
{
    [SerializeField] private WireNum _wireNum;

    private enum WireNum
    {
        ONE, TWO, THREE
    }

    private void OnInteract()
    {
        // TODO: What happens when player clicks?
    }

    private void OnDrop()
    {
        // TODO: What happens when the player drops the wire?
    }

    private void PlaceWire()
    {
        // TODO: What happens when the player puts the wire in a spot?
    }    

    private void CheckConnection()
    {
        // TODO: Checks if the wire is in the correct spot
    }
}