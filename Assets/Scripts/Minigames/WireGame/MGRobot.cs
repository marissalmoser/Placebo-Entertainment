using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Assertions;

public class MGRobot : MonoBehaviour
{
    private int _correctWires = 0;
    private int _maxWires = 1;

    private void BypassCheck()
    {
        // TODO: Q player inventory for item to bypass minigame
    }

    private void CheckBattery()
    {
        // TODO: Check if robot has battery
    }

    private void OnCorrectWire()
    {
        _correctWires++;
        
        Debug.LogAssertion(_maxWires == _correctWires);
    }

    private void RoboSuccess()
    {
        // TODO: Send out success event
    }
}
