/******************************************************************
*    Author: Nick Grinstead
*    Contributors: Elijah Vroman
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
    [SerializeField] private InventoryItemData _targetPillsItem;
    private bool _hasPills;
    private bool _robotGameComplete = false;
    private bool _cowardGameComplete = false;

    private Animator _anim;

    /// <summary>
    /// Getting Animator on child
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();

        _anim = GetComponentInChildren<Animator>();
    }

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
        else if (_currentState == NpcStates.PlayingMinigame)
        {
            EnterPostMinigame();
        }
    }
    protected override void EnterPostMinigame()
    {
        base.EnterPostMinigame();
    }

    /// <summary>
    /// Temporary wingame as of 6/26
    /// </summary>
    public void WinGame()
    {
        _tabbedMenu.ToggleWin(true);
    }
    /// <summary>
    /// Temporary set-up for first playable that either continues the loop or 
    /// displays the winscreen
    /// Commented out by Elijah Vroman
    /// </summary>
    //protected override void EnterMinigameReady()
    //{
    //    base.EnterMinigameReady();

    //    if (_cowardGameComplete && _robotGameComplete)
    //    {
    //        _tabbedMenu.ToggleWin(true);
    //    }
    //    else
    //    {
    //        // Return to idle if player didn't win
    //        EnterIdle();
    //    }
    //}

    /// <summary>
    /// Chooses different dialogue based on if the player has done both minigames
    /// or not
    /// </summary>
    /// <param name="node">Dialogue node being examined</param>
    /// <returns>String dialogue to display</returns>
    protected override string ChooseDialogueFromNode(DialogueNode node)
    {
        PlayRandomTalkingAnim();
        if (_cowardGameComplete && _robotGameComplete)
        {
            return node.Dialogue[1];
        }
        else
        {
            return node.Dialogue[0];
        }
    }
    protected override int ChooseDialoguePath(PlayerResponse option)
    {
        if (option.NextResponseIndex.Length == 1)
        {
            _anim.SetTrigger("FinalChoice");
        }

        if (_hasPills)
        {
            _anim.SetTrigger("Healed");
            return option.NextResponseIndex[1];
        }
        else if(!_hasPills && option.NextResponseIndex.Length > 0)
        {
            return option.NextResponseIndex[0];
        }
        // Checks for bypass
        else if (_hasPills && _currentState != NpcStates.PostMinigame)
        {
            _shouldEndDialogue = true;
            Invoke(nameof(EnterPostMinigame), 0.2f);
            return 0;
        }
        // Don't have minigame bypass
        else
        {
            if (option.NextResponseIndex.Length > 0)
            {
                return option.NextResponseIndex[0];
            }
            else
            {
                return 0;
            }
        }
    }

    public override void CollectedItem(InventoryItemData item, int quantity)
    {
        base.CollectedItem(item, quantity);

        if (item == _targetPillsItem)
        {
            _hasPills = true;
            Debug.Log(_hasPills);
        }
    }
    private void PlayRandomTalkingAnim()
    {
        int rand = Random.Range(1, 4);
        switch (rand)
        {
            case 1:
                _anim.SetTrigger("Talking1");
                break;
            case 2:
                _anim.SetTrigger("Talking2");
                break;
            case 3:
                _anim.SetTrigger("Talking3");
                break;
        }
    }
}
