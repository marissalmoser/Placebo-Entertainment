/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: 5/17/24
*    Description: Sample class showing how an NPC can be created from
*       BaseNPC.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleNpc : BaseNpc
{
    private void Awake()
    {
        Initialize();
    }

    protected override void Initialize()
    {
        base.Initialize();

        // Other setup unique to this NPC goes here
    }

    public override void CheckForStateChange()
    {
        // This method should contain various checks to determine the next state
        // to go to
    }

    protected override void EnterIdle()
    {
        base.EnterIdle();

        // Add any unique NPC behaviors here under the base call
    }

    protected override void EnterMinigameReady()
    {
        //base.EnterMinigameReady();

        EnterPlayingMinigame(); // If a NPC doesn't use a certain state, it can be skipped
    }

    protected override void EnterPlayingMinigame()
    {
        base.EnterPlayingMinigame();

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
    }

    protected override void EnterFailure()
    {
        base.EnterFailure();
    }

}
