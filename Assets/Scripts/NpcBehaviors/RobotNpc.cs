/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: 5/22/24
*    Description: NPC class containing logic for the Robot NPC.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RobotNpc : BaseNpc
{
    [SerializeField] private InventoryItemData _targetLightBulbItem;
    [SerializeField] private float _secondsUntilDeath;
    private float _timeElapsed = 0f;

    private bool _hasLightbulb = false;
    private bool _hasRepairedRobot = false;
    private bool _isFirstInteraction = true;

    private Animator _anim;

    [SerializeField] private GameObject _lightbulbMesh;

    /// <summary>
    /// Subscribing to wire game won event on initialization
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();

        _anim = GetComponentInChildren<Animator>();

        MGWireState.WireGameWon += CheckForStateChange;
    }

    /// <summary>
    /// Unsubscribing from events on disable
    /// </summary>
    protected override void OnDisable()
    {
        base.OnDisable();

        MGWireState.WireGameWon -= CheckForStateChange;
    }

    /// <summary>
    /// Checks if collected item is either the light bulb or the calibration tool
    /// </summary>
    /// <param name="item">The item that was collected</param>
    /// <param name="quantity">How many of that item was collected</param>
    public override void CollectedItem(InventoryItemData item, int quantity)
    {
        base.CollectedItem(item, quantity);

        if (item == _targetLightBulbItem)
        {
            Debug.Log("Lightbulb collected");
            _hasLightbulb = true;
        }
    }

    /// <summary>
    /// Invoked by event upon collecting lightbulb item
    /// </summary>
    public void CollectLightbulb()
    {
        _hasLightbulb = true;
    }

    /// <summary>
    /// Called  when recieving an event upon minigame completion to change states
    /// </summary>
    public override void CheckForStateChange()
    {
        if (_currentState == NpcStates.PlayingMinigame)
        {
            EnterPostMinigame();
        }
    }

    /// <summary>
    /// Starts death timer
    /// </summary>
    protected override void EnterIdle()
    {
        base.EnterIdle();

        StartCoroutine(DeathTimer());
    }

    /// <summary>
    /// Moves straight into post-minigame dialogue
    /// </summary>
    protected override void EnterPostMinigame()
    {
        base.EnterPostMinigame();

        Interact();
    }

    /// <summary>
    /// Determines whether to use first or second interaction dialogue for 
    /// Robot Interaction 1
    /// </summary>
    /// <param name="node">DialogueNode being checked</param>
    /// <returns>string dialogue to display</returns>
    protected override string ChooseDialogueFromNode(DialogueNode node)
    {
        PlayRandomTalkingAnim();
        if (node.Dialogue.Length == 1 || _isFirstInteraction)
        {
            _isFirstInteraction = false;
            string temp = node.Dialogue[0];
            if (temp.Contains("(time left)"))
            {
                int timeRemaining = (int)(_secondsUntilDeath - _timeElapsed);
                temp = temp.Replace("(time left)", timeRemaining.ToString() + " seconds");
            }

            return temp;
        }

        return node.Dialogue[1];
    }

    /// <summary>
    /// Checks for if player has lightbulb or bypass item to determine which
    /// dialogue paths to take
    /// </summary>
    /// <param name="option">PlayerResponse being checked</param>
    /// <returns>Index of next dialogue node</returns>
    protected override int ChooseDialoguePath(PlayerResponse option)
    {
        // Trying to repair robot and you have lightbulb
        if (!_hasRepairedRobot && _hasLightbulb)
        {
            _hasRepairedRobot = true;
            _anim.SetTrigger("Lightbulb");
            _lightbulbMesh.SetActive(true);
            return option.NextResponseIndex[0];
        }
        // Trying to repair robot without the lightbulb
        else if (!_hasRepairedRobot && !_hasLightbulb && option.NextResponseIndex.Length > 0)
        {
            return option.NextResponseIndex[1];
        }
        // Bypass for minigame
        else if (_hasRepairedRobot && _hasBypassItem && _currentState != NpcStates.PostMinigame)
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

    /// <summary>
    /// Plays random talking animation when selecting a dialogue choice
    /// </summary>
    private void PlayRandomTalkingAnim()
    {
        int rand = Random.Range(1, 3);
        switch(rand)
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

    /// <summary>
    /// Runs a timer that when complete will set the Robot to its failure state
    /// </summary>
    /// <returns>Waits one second</returns>
    private IEnumerator DeathTimer()
    {
        while (_timeElapsed < _secondsUntilDeath)
        {
            yield return new WaitForSeconds(1f);

            _timeElapsed += 1f;
        }

        if (!_hasRepairedRobot)
        {
            EnterFailure();
            _anim.SetBool("Dead", true);
        }
    }
}
