/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: 5/17/24
*    Description: Sample class showing how an NPC can be created from
*       BaseNPC. Can also be used for testing purposes.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleNpc : BaseNpc
{
    // Update method code for testing
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckForStateChange();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            EnterFailure();
        }
    }

    public override void CheckForStateChange()
    {
        // This method should contain various checks to determine the next state
        // to go to

        // Code for testing
        if (_currentState == NpcStates.DefaultIdle)
            EnterMinigameReady();
        else if (_currentState == NpcStates.MinigameReady)
            EnterPlayingMinigame();
        else if (_currentState == NpcStates.PlayingMinigame)
            EnterPostMinigame();
    }

    protected override void EnterIdle()
    {
        base.EnterIdle();

        Debug.Log("Idle State Reached");

        // Add any unique NPC behaviors here under the base call
    }

    protected override void EnterMinigameReady()
    {
        base.EnterMinigameReady();

        Debug.Log("Pre Minigame State Reached");
    }

    protected override void EnterPlayingMinigame()
    {
        base.EnterPlayingMinigame();

        Debug.Log("Minigame State Reached");

        if (_haveBypassItem)
        {
            // Skip minigame
        }
        else
        {
            // Start minigame
        }
    }

    protected override void EnterPostMinigame()
    {
        base.EnterPostMinigame();

        Debug.Log("Post Minigame State Reached");
    }

    protected override void EnterFailure()
    {
        base.EnterFailure();

        Debug.Log("Failure State Reached");
    }

}
