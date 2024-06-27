/******************************************************************
*    Author: Nick Grinstead
*    Contributors: Andrea Swihart-DeCoster
*    Date Created: 5/28/24
*    Description: NPC class containing logic for the Coward NPC.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowardNpc : BaseNpc
{
    [SerializeField] private InventoryItemData _targetLightBulbItem;
    [SerializeField] private float _secondsUntilExplosion;

    private bool _canTeleportToGenerator = false;
    private bool _hasLightbulb = false;
    private bool _wireGameCompleted = false;
    private bool _robotIsAlive = true;

    /// <summary>
    /// Called when the player enters the generator room
    /// </summary>
    public void GeneratorEventTriggered()
    {
        if (_currentState == NpcStates.MinigameReady)
        {
            Interact();
        }
    }

    /// <summary>
    /// Triggers Coward dialogue in response to the light bulb being collected
    /// </summary>
    /// <param name="item">The item that was collected</param>
    /// <param name="quantity">How many of that item was collected</param>
    public override void CollectedItem(InventoryItemData item, int quantity)
    {
        base.CollectedItem(item, quantity);

        if (item == _targetLightBulbItem)
        {
            LightbulbEventTriggered();
        }
    }

    /// <summary>
    /// Called when event for player picking up lightbulb is triggered
    /// </summary>
    public void LightbulbEventTriggered()
    {
        _hasLightbulb = true;
        Interact();

        _canTeleportToGenerator = true;
    }

    /// <summary>
    /// Called when event for player picking up lightbulb is triggered
    /// </summary>
    public void WireMinigameCompletedEvent()
    {
        _wireGameCompleted = true;
    }

    /// <summary>
    /// Called via dialogue to move into minigame ready state as well as from
    /// the minigame complete event to move into the postminigame state
    /// </summary>
    public override void CheckForStateChange()
    {
        // TODO : Add check if robot is alive (details in disc chat)
        if (CanBeginMinigame())
        {
            EnterMinigameReady();
        }
        else if (_currentState == NpcStates.PlayingMinigame)
        {
            EnterPostMinigame();
        }
    }

    private bool CanBeginMinigame()
    {
        return _currentState == NpcStates.DefaultIdle && _hasLightbulb && _wireGameCompleted && _robotIsAlive;
    }

    /// <summary>
    /// Disabling generator room listener
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();
    }

    /// <summary>
    /// Starts generator timer when entering idle state
    /// </summary>
    protected override void EnterIdle()
    {
        base.EnterIdle();

        StartCoroutine("GeneratorTimer");
    }

    /// <summary>
    /// Enables listener for when player enters generator room
    /// </summary>
    protected override void EnterMinigameReady()
    {
        base.EnterMinigameReady();
    }

    /// <summary>
    /// Starts Coward Interaction 3 when entering post minigame state
    /// </summary>
    protected override void EnterPostMinigame()
    {
        base.EnterPostMinigame();

        Interact();

        _playerInventorySystem.AddToInventory(_targetBypassItem, 1, out _);
    }

    /// <summary>
    /// Restarts the loop due to generator explosion when entering failure state
    /// </summary>
    protected override void EnterFailure()
    {
        base.EnterFailure();

        // TODO: trigger new loop here
    }

    /// <summary>
    /// When in idle state displays different responses based on if player has
    /// light bulb. For any other state, calls base version of function.
    /// </summary>
    protected override void GetPlayerResponses()
    {
        if (_currentState != NpcStates.DefaultIdle)
        {
            base.GetPlayerResponses();
        }
        else
        {
            if (_isInteracting)
            {
                DialogueNode currentNode = _stateDialogueTrees.GetStateData(_currentState)[_currentDialogueIndex];

                // Displays player dialogue options
                PlayerResponse option;
                _tabbedMenu.ClearDialogueOptions();

                if (_hasLightbulb)
                {
                    option = currentNode.PlayerResponses[0];
                    _tabbedMenu.DisplayDialogueOption(option.Answer, click: () => { Interact(0); });
                }
                else
                {
                    option = currentNode.PlayerResponses[1];
                    _tabbedMenu.DisplayDialogueOption(option.Answer, click: () => { Interact(1); });
                }
            }
        }
    }

    /// <summary>
    /// Checks for bypass item to see which path to take
    /// </summary>
    /// <param name="option">PlayerResponse being checked</param>
    /// <returns>Index of next dialogue node</returns>
    protected override int ChooseDialoguePath(PlayerResponse option)
    {
        // Checks for bypass
        if (_hasBypassItem && _currentState != NpcStates.PostMinigame)
        {
            _shouldEndDialogue = true;
            Invoke("EnterPostMinigame", 0.2f);
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
    /// Chooses between two responses depending on if the player has taken the 
    /// light bulb
    /// </summary>
    /// <param name="node">DialogueNode being examined</param>
    /// <returns>string dialogue response</returns>
    protected override string ChooseDialogueFromNode(DialogueNode node)
    {
        // Select different dialogue if player hasn't taken light bulb
        if (node.Dialogue.Length > 1 && _currentState == NpcStates.DefaultIdle &&
            !_hasLightbulb)
        {
            return node.Dialogue[1];
        }

        return node.Dialogue[0];
    }

    /// <summary>
    /// Waits for a time until the generator explodes before entering the failure state
    /// </summary>
    /// <returns>Waits for the time until explosion</returns>
    private IEnumerator GeneratorTimer()
    {
        yield return new WaitForSeconds(_secondsUntilExplosion);

        if (_currentState != NpcStates.PostMinigame)
        {
            EnterFailure();
        }
    }

    public void OnRobotFailState()
    {
        _robotIsAlive = false;
    }

    /// <summary>
    /// Teleports the NPC player to a destination
    /// </summary>
    /// <param name="destination"> teleport target destination </param>
    public void TeleportToLocation(GameObject destination)
    {
        if(_canTeleportToGenerator)
        {
            transform.position = destination.transform.position;
        }
    }
}
