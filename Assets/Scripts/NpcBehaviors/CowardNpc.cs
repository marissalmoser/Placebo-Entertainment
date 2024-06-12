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

    private bool _canTriggerInteraction = false;
    private bool _canTeleportToGenerator = false;

    /// <summary>
    /// Called when the player enters the generator room
    /// </summary>
    public void GeneratorEventTriggered()
    {
        if (_currentState == NpcStates.MinigameReady)
        {
            _canTriggerInteraction = true;
            _canInteract = true;
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
        _canTriggerInteraction = true;
        _canInteract = true;
        Interact();

        _canTeleportToGenerator = true;
    }

    /// <summary>
    /// Called via dialogue to move into minigame ready state as well as from
    /// the minigame complete event to move into the postminigame state
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

    /// <summary>
    /// Overriding Interact to prevent player from triggering minigame ready
    /// dialogue without triggering generator explosion event
    /// </summary>
    /// <param name="responseIndex">Index in the dialogue tree, assumes 0 by default,
    /// shouldn't be negative</param>
    public override void Interact(int responseIndex = 0)
    {
        if (_canTriggerInteraction)
        {
            base.Interact(responseIndex);
        }
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

        _canTriggerInteraction = false;
    }

    /// <summary>
    /// Starts Coward Interaction 3 when entering post minigame state
    /// </summary>
    protected override void EnterPostMinigame()
    {
        base.EnterPostMinigame();

        _canTriggerInteraction = true;
        Interact();
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
            return option.NextResponseIndex[0];
        }
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
