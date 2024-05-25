/*****************************************************************************
// File Name :         MGRobot.cs
// Author :            Andrea Swihart-DeCoster
// Creation Date :     05/24/24
//
// Brief Description : (Not implemented yet, TBD if it should be). This doc
                       would check conditions from the robot to make sure the
                       quest can start if the player has the right items and
                       trigger the event when the quest is over so the game
                       can progress.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Assertions;

public class MGRobot : MonoBehaviour
{
    private void BypassCheck()
    {
        // TODO: Q player inventory for item to bypass minigame
    }

    private void CheckBattery()
    {
        // TODO: Check if robot has battery
    }

    private void RoboSuccess()
    {
        // TODO: Send out success event
    }
}
