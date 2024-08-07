/******************************************************************
*    Author: Nick Grinstead
*    Contributors: Andrea Swihart-DeCoster
*    Date Created: 5/28/24
*    Description: NPC class containing logic for the Coward NPC.
*******************************************************************/
using UnityEngine;

public class CowardNpc : BaseNpc
{
    [SerializeField] private NpcEvent _removeTimerEvent;

    private bool _canTeleportToGenerator = false;
    private bool _hasTeleported = false;
    private bool _hasLightbulb = false;
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
    /// Called when event for player picking up lightbulb is triggered
    /// </summary>
    public void LightbulbEventTriggered()
    {
        _hasLightbulb = true;
        _animator.SetTrigger("NotBlind");
        Interact();

        _tabbedMenu.ToggleInteractPrompt(false);
        _canTeleportToGenerator = true;
    }

    /// <summary>
    /// Called when event for player picking up lightbulb is triggered
    /// </summary>
    public void WireMinigameCompletedEvent()
    {
        CheckForStateChange();
    }

    /// <summary>
    /// Called via dialogue to move into minigame ready state as well as from
    /// the minigame complete event to move into the postminigame state
    /// </summary>
    public override void CheckForStateChange()
    {
        if ((_hasTeleported || !_robotIsAlive) && _currentState == NpcStates.DefaultIdle)
        {
            EnterMinigameReady();
        }
        else if (_currentState == NpcStates.PlayingMinigame)
        {
            EnterPostMinigame();
        }
    }

    /// <summary>
    /// Starts Coward Interaction 3 when entering post minigame state
    /// </summary>
    protected override void EnterPostMinigame()
    {
        base.EnterPostMinigame();

        Interact();

        _playerInventorySystem.AddToInventory(_targetBypassItem, 1, out _);

        _removeTimerEvent.TriggerEvent(NpcEventTags.Coward);
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
        //Checks for the wrench
        if (_playerInventorySystem.ContainsItem(_targetBypassItem, out _))
        {
            return option.NextResponseIndex[1];
        }
        else
        {
            return base.ChooseDialoguePath(option);
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
        if (node.Dialogue.Length > 1 && _currentState == NpcStates.DefaultIdle && !_hasLightbulb)
        {
            PlayRandomTalkingAnim();
            return node.Dialogue[1];
        }

        return base.ChooseDialogueFromNode(node);
    }

    /// <summary>
    /// Invoked via event by Robot when it dies
    /// </summary>
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
            _hasTeleported = true;
            CheckForStateChange();
        }
    }
}
