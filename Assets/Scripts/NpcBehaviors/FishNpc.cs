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

public class FishNpc : BaseNpc
{
    [SerializeField] private Vector3 _postMinigameFishPos;

    private bool _enteredFireRoom = false;
    private bool _hasfish;
    [SerializeField] private int secondsUntilFailFireGame;
    private float _timeElapsed = 0f;

    [SerializeField] private float _fadeOutTime;
    //[SerializeField] private GameObject _fadeOutObject;
    //[SerializeField] private Image _fadeOutImage;
    [SerializeField] private UIDocument _fadeOutDoc;
    private VisualElement _fadeOutElement;

    private const string FadeOutElementName = "FadeOutBackground";
    private const string FadeOutClassName = "fadeOut";
    private const string FadeInClassName = "fadeIn";

    protected override void Initialize()
    {
        base.Initialize();

        _fadeOutElement = _fadeOutDoc.rootVisualElement.Q(FadeOutElementName);
        _fadeOutElement.style.transitionProperty = new List<StylePropertyName> { "opacity" };
        _fadeOutElement.style.transitionDuration = new List<TimeValue> { new TimeValue(_fadeOutTime, TimeUnit.Second) };
        _fadeOutElement.style.transitionTimingFunction = new List<EasingFunction> { EasingMode.Linear };
    }

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

    protected override void EnterPostMinigame()
    {
        base.EnterPostMinigame();

        StartCoroutine(FadeToBlack());
        _playerController.LockCharacter(true);
    }

    private IEnumerator FadeToBlack()
    {
        // Fade out
        _fadeOutDoc.rootVisualElement.style.display = DisplayStyle.Flex;
        _fadeOutElement.style.opacity = 1;

        yield return new WaitForSeconds(_fadeOutTime);

        // Fade in
        transform.localPosition = _postMinigameFishPos;
        _fadeOutElement.style.opacity = 0;

        yield return new WaitForSeconds(_fadeOutTime);

        _fadeOutDoc.rootVisualElement.style.display = DisplayStyle.None;

        Interact();
    }

    protected override void EnterFailure()
    {
        base.EnterFailure();

        ResetLoop();
    }

    public void SteppedIn()
    {
        _hasfish = true;
    }

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

    protected override void EnterIdle()
    {
        base.EnterIdle();
        //TimerManager.Instance.CreateTimer("FireRoomMiniGameTimer", secondsUntilFailFireGame);
        StartCoroutine(FireTimer());
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
    private IEnumerator FireTimer()
    {
        while (_timeElapsed < secondsUntilFailFireGame)
        {
            yield return new WaitForSeconds(1f);

            _timeElapsed += 1f;
        }
        //Not sure how the fish confirms the minigame got completed in dialogue
        //if (!_hasRepairedRobot)
        //{
        //    EnterFailure();
        //}
    }
}
