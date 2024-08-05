/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Nick Grinstead
*    Date Created: 6/25/24
*    Description: NPC class containing logic for the Fish NPC.
*******************************************************************/
using PlaceboEntertainment.UI;
using System.Collections;
using UnityEngine;

public class FishNpc : BaseNpc
{
    [SerializeField] private Vector3 _postMinigameFishPos;
    [SerializeField] private NpcEvent _removeTimerEvent;

    private bool _enteredFireRoom = false;
    private bool _hasfish;
    private float _timeElapsed = 0f;
    private Animator _anim;

    [SerializeField] private float _fadeOutTime;

    protected override void Initialize()
    {
        base.Initialize();

        _anim = GetComponentInChildren<Animator>();
    }

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

        _removeTimerEvent.TriggerEvent(NpcEventTags.Fish);
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

        Debug.Log("Failed the fire/fish game");
    }

    /// <summary>
    /// 
    /// </summary>
    public void SteppedIn()
    {
        _hasfish = true;
    }

    protected override string ChooseDialogueFromNode(DialogueNode node)
    {
        PlayRandomTalkingAnim();
        return node.Dialogue[0];
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
