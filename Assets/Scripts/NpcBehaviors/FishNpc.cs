/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Nick Grinstead
*    Date Created: 6/25/24
*    Description: NPC class containing logic for the Fish NPC.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using PlaceboEntertainment.UI;

public class FishNpc : BaseNpc
{
    [SerializeField] private Vector3 _postMinigameFishPos;

    private bool _enteredFireRoom = false;
    private bool _hasfish;
    [SerializeField] private int secondsUntilFailFireGame;
    private float _timeElapsed = 0f;

    [SerializeField] private float _fadeOutTime;

    /// <summary>
    /// Called to update the NPCs state
    /// </summary>
    public override void CheckForStateChange()
    {
        if(_currentState == NpcStates.DefaultIdle && _enteredFireRoom)
        {
            EnterMinigameReady();
        }
        else if (_currentState == NpcStates.PlayingMinigame)
        {
            EnterPostMinigame();
        }
    }

    /// <summary>
    /// Begins fade out and fish teleport
    /// </summary>
    protected override void EnterPostMinigame()
    {
        base.EnterPostMinigame();

        TabbedMenu.Instance.StartFadeOut(_fadeOutTime);
        _playerController.LockCharacter(true);
        StartCoroutine(MoveFishDuringFadeOut());
    }

    /// <summary>
    /// Moves fish to fire room at the middle of the fade out
    /// </summary>
    /// <returns>Waits for half of _fadeOutTime twice</returns>
    private IEnumerator MoveFishDuringFadeOut()
    {
        yield return new WaitForSeconds(_fadeOutTime / 2);

        transform.localPosition = _postMinigameFishPos;

        yield return new WaitForSeconds(_fadeOutTime / 2);

        Interact();
        _tabbedMenu.ToggleInteractPrompt(false);
    }

    /// <summary>
    /// Resets loop on failure
    /// </summary>
    protected override void EnterFailure()
    {
        base.EnterFailure();

        ResetLoop();
    }

    /// <summary>
    /// 
    /// </summary>
    public void SteppedIn()
    {
        _hasfish = true;
    }

    /// <summary>
    /// Selects a dialogue path from a player response based on various conditions
    /// </summary>
    /// <param name="option">Player response to examine</param>
    /// <returns>Returns next dialogue response index</returns>
    protected override int ChooseDialoguePath(PlayerResponse option)
    {
        if(_hasfish)
        {
            return option.NextResponseIndex[1];
        }
        else if (!_hasfish && option.NextResponseIndex.Length > 0)
        {
            return option.NextResponseIndex[0];
        }
        else if (_hasfish && _currentState != NpcStates.PostMinigame)
        {
            _shouldEndDialogue = true;
            Invoke(nameof(EnterPostMinigame), 0.2f);
            return 0;
        }
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
    /// Will start the fire death timer
    /// </summary>
    protected override void EnterIdle()
    {
        base.EnterIdle();
        // TODO: uncomment this once timer updates are done
        //TimerManager.Instance.CreateTimer("FireRoomMiniGameTimer", secondsUntilFailFireGame);
    }

    /// <summary>
    /// Lifted this from LoopController
    /// </summary>
    public void ResetLoop()
    {
        SaveLoadManager.Instance.SaveGameToSaveFile();
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
        SaveLoadManager.Instance.LoadGameFromSaveFile();
    }
}
