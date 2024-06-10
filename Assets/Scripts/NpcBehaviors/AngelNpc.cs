/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: 6/9/24
*    Description: NPC class containing logic for the Angel NPC.
*                 Currently set up for the first playable, will need to be updated
*                 for future milestones with more features.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelNpc : BaseNpc
{
    private bool _robotGameComplete = false;
    private bool _cowardGameComplete = false;

    /// <summary>
    /// Used in first playable for knowing if the robot game is complete
    /// done
    /// </summary>
    public void RobotMinigameCompleted()
    {
        _robotGameComplete = true;
    }

    /// <summary>
    /// Used in first playable for knowing if the coward game is complete
    /// done
    /// </summary>
    public void CowardMinigameCompleted()
    {
        _cowardGameComplete = true;
    }

    /// <summary>
    /// TODO: implement this method for the full version of the Angel
    /// </summary>
    public override void CheckForStateChange()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Decides if the player should win or if the loop resets when talking to
    /// the Angel. The dialogue path taken will trigger one of the two events
    /// </summary>
    /// <param name="option">PlayerResponse being examined</param>
    /// <returns>int index of the next dialogue node</returns>
    protected override int ChooseDialoguePath(PlayerResponse option)
    {
        if (_robotGameComplete && _cowardGameComplete)
        {
            return option.NextResponseIndex[1];
        }
        else
        {
            return option.NextResponseIndex[0];
        }
    }
}
