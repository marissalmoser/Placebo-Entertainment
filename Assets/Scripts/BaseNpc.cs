using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNpc : MonoBehaviour
{ 
    //[SerializeField] protected Dialogue[] dialogueList; 

    protected NpcStates _currentState = NpcStates.DefaultIdle;
    protected bool _canChangeStates = false;
    protected int _prerequisitesChecked = 0;

    protected bool _canInteract = false;
    protected bool _haveBypassItem = false;

    // Derived classes should call this
    protected virtual void Initialize()
    {
        EnterIdle();
    }

    public void Interact()
    {
        if (_canInteract)
        {
            // TODO: display dialogue based on current state
        }
    }

    public abstract void CheckPrerequisite();

    public void CollectedBypassItem()
    {
        _haveBypassItem = true;
    }

    #region StateFunctions
    protected virtual void EnterIdle()
    {
        _currentState = NpcStates.DefaultIdle;
        _prerequisitesChecked = (int) _currentState;
    }

    protected virtual void EnterMinigameReady()
    {
        _currentState = NpcStates.MinigameReady;
        _prerequisitesChecked = (int)_currentState;
    }

    protected virtual void EnterPlayingMinigame()
    {
        _currentState = NpcStates.PlayingMinigame;
        _prerequisitesChecked = (int)_currentState;

        if (_haveBypassItem)
        {
            // TODO: Skip minigame
        }
        else
        {
            // TODO: Start minigame
        }
    }

    protected virtual void EnterPostMinigame()
    {
        _currentState = NpcStates.PostMinigame;
        _prerequisitesChecked = (int)_currentState;
    }
    
    protected virtual void EnterFailure()
    {
        _currentState = NpcStates.Failure;
        _prerequisitesChecked = (int)_currentState;
    }
    #endregion

    #region CollisionChecks
    // Enables ability to interact if the player is close to the NPC
    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canInteract = true;
        }
    }

    // Disables ability to interact if the player moves away from the NPC
    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canInteract = false;
        }
    }
    #endregion
}

public enum NpcStates
{ 
    // Starting state of the NPC
    DefaultIdle,
    // Player can begin minigame
    MinigameReady,
    // Checks for bypass item before starting minigame
    PlayingMinigame,
    // State of the NPC after their quest/minigame is over
    PostMinigame,
    // State of an NPC once their quest is failed
    Failure
}

