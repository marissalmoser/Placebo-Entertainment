/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: 5/17/24
*    Description: Base NPC class for specific NPCs to derive from. 
*       Has basic functionality for switching states and interacting
*       that child scripts will expand upon.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseNpc : MonoBehaviour
{
    //[SerializeField] protected StateDialogue<DialogueClassHere> _stateDialogue;
    [SerializeField] protected StateDataGroup<Vector3> _navigationPositions;
    [SerializeField] protected StateDataGroup<Animation> _stateAnimations;

    protected NavMeshAgent navAgent;
    protected Animator animator;

    protected NpcStates _currentState = NpcStates.DefaultIdle;
    protected bool _canChangeStates = false;
    protected int _prerequisitesChecked = 0;

    protected bool _canInteract = false;
    protected bool _haveBypassItem = false;

    // Derived classes should call this
    protected virtual void Initialize()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

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
        StateUpdateHelper();
    }

    protected virtual void EnterMinigameReady()
    {
        _currentState = NpcStates.MinigameReady;
        StateUpdateHelper();
    }

    protected virtual void EnterPlayingMinigame()
    {
        _currentState = NpcStates.PlayingMinigame;
        StateUpdateHelper();
    }

    protected virtual void EnterPostMinigame()
    {
        _currentState = NpcStates.PostMinigame;
        StateUpdateHelper();
    }
    
    protected virtual void EnterFailure()
    {
        _currentState = NpcStates.Failure;
        StateUpdateHelper();
    }

    private void StateUpdateHelper()
    {
        _prerequisitesChecked = (int)_currentState;

        if (navAgent != null)
        {
            Vector3 newPosition = _navigationPositions.GetStateData(_currentState);
            navAgent.SetDestination(newPosition);
        }

        if (animator != null)
        {
            Animation newAnimation = _stateAnimations.GetStateData(_currentState);
            if (newAnimation != null)
            {
                animator.Play(newAnimation.name);
            }
        }
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

[System.Serializable]
public class StateDataGroup<T>
{
    [SerializeField] T _idleState;
    [SerializeField] T _minigameReadyState;
    [SerializeField] T _playingMinigameState;
    [SerializeField] T _postMinigameState;
    [SerializeField] T _failureState;

    public T GetStateData(NpcStates currentState)
    {
        switch (currentState)
        {
            case NpcStates.DefaultIdle:
                return _idleState;

            case NpcStates.MinigameReady:
                return _minigameReadyState;

            case NpcStates.PlayingMinigame:
                return _playingMinigameState;

            case NpcStates.PostMinigame:
                return _postMinigameState;

            case NpcStates.Failure:
                return _failureState;
        }

        return default(T);
    }
}
