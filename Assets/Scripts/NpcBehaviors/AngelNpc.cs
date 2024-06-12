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
        if (_currentState == NpcStates.DefaultIdle)
        {
            EnterMinigameReady();
        }
    }

    /// <summary>
    /// Temporary set-up for first playable that either resets the loop or 
    /// displays the winscreen
    /// </summary>
    protected override void EnterMinigameReady()
    {
        base.EnterMinigameReady();

        if (_cowardGameComplete && _robotGameComplete)
        {
            // TODO: display winscreen
        }
        else
        {
            // TODO: reset loop
        }
    }

    /// <summary>
    /// Chooses different dialogue based on if the player has done both minigames
    /// or not
    /// </summary>
    /// <param name="node">Dialogue node being examined</param>
    /// <returns>String dialogue to display</returns>
    protected override string ChooseDialogueFromNode(DialogueNode node)
    {
        if (_cowardGameComplete && _robotGameComplete)
        {
            return node.Dialogue[1];
        }
        else
        {
            return node.Dialogue[0];
        }
    }
}
