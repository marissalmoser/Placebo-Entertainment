/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: 5/22/24
*    Description: NPC class containing logic for the Robot NPC.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotNpc : BaseNpc
{
    [SerializeField] private NpcEvent unlockDoorsEvent;

    [SerializeField] private float _secondsUntilDeath;
    private float _timeElapsed = 0f;

    private bool _hasLightbulb = false;

    /// <summary>
    /// Invoked by event upon collecting lightbulb item
    /// </summary>
    public void CollectLightbulb()
    {
        _hasLightbulb = true;
    }

    /// <summary>
    /// Called when engaging in dialogue during idle state and when recieving
    /// an event upon minigame completion
    /// </summary>
    public override void CheckForStateChange()
    {
        if (_hasLightbulb && _currentState == NpcStates.DefaultIdle)
        {
            StopAllCoroutines();

            EnterMinigameReady();
        }

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

        StartCoroutine("DeathTimer");
    }

    /// <summary>
    /// Moves straight into pre-minigame dialogue with bypass check
    /// </summary>
    protected override void EnterMinigameReady()
    {
        base.EnterMinigameReady();

        Interact();
        // TODO: add bypass check with minigame trigger?
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

        if (_currentState == NpcStates.DefaultIdle)
        {
            EnterFailure();
        }
    }
}
