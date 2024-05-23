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
using UnityEngine.InputSystem;
using PlaceboEntertainment.UI;

public abstract class BaseNpc : MonoBehaviour
{
    /// <summary>
    /// Structure used to store a collection of data related to the different states
    /// Example: A StateDataGroup<Animation> could hold idle animations for each state
    /// </summary>
    /// <typeparam name="T">Type of data to be stored</typeparam>
    [System.Serializable]
    protected struct StateDataGroup<T>
    {
        [SerializeField] private T _idleState;
        [SerializeField] private T _minigameReadyState;
        [SerializeField] private T _playingMinigameState;
        [SerializeField] private T _postMinigameState;
        [SerializeField] private T _failureState;

        /// <summary>
        /// Returns data for a provided state
        /// </summary>
        /// <param name="currentState">The state this NPC is in</param>
        /// <returns>Some value of type T</returns>
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

    /// <summary>
    /// Structure to hold a collection of dialogue options and responses
    /// Note: This assumes NPC's won't have any alternative dialogue based on 
    /// various conditions. Subclasses will handle any alternative dialogue.
    /// </summary>
    [System.Serializable]
    protected struct DialogueGroup
    {
        [SerializeField] private bool _canTalk;
        [SerializeField] private string _initialNpcDialogue;
        [SerializeField] private string[] _playerResponses;
        [SerializeField] private string[] _npcResponses;

        public bool CanTalk { get => _canTalk; }
        public string InitialNpcDialogue { get => _initialNpcDialogue; }
        public string[] PlayerResponses { get => _playerResponses; }
        public string[] NpcResponses { get => _npcResponses; }
    }

    [SerializeField] protected NpcEvent _startMinigameEvent;
    [SerializeField] protected string _eventTag;

    [SerializeField] protected StateDataGroup<DialogueGroup> _stateDialogue;
    [SerializeField] protected StateDataGroup<Vector3> _navigationPositions;
    [SerializeField] protected StateDataGroup<Animation> _stateAnimations;

    protected PlayerControls _playerControls;
    protected NavMeshAgent _navAgent;
    protected Animator _animator;
    protected TabbedMenu _tabbedMenu;

    protected NpcStates _currentState = NpcStates.DefaultIdle;

    protected bool _canInteract = false;
    protected bool _isInteracting = false;
    protected bool _haveBypassItem = false;

    /// <summary>
    /// Invoking Initialize() on Awake to set up NPC
    /// </summary>
    protected void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// Handles basic NPC setup and derived classes can expand upon this to add
    /// unique functionality
    /// </summary>
    protected virtual void Initialize()
    {
        _playerControls = new PlayerControls();
        _playerControls.Enable();
        InputAction interact = _playerControls.FindAction("Interact");
        interact.performed += ctx => Interact();

        _tabbedMenu = TabbedMenu.Instance;
        _navAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        EnterIdle();
    }

    /// <summary>
    /// Called when player presses button to interact with NPC.
    /// </summary>
    public void Interact()
    {
        if (_canInteract)
        {
            DialogueGroup currentDialogue = _stateDialogue.GetStateData(_currentState);
            if (_tabbedMenu != null)
            {
                _tabbedMenu.ToggleInteractPrompt(false);
            }
            
            if (currentDialogue.CanTalk)
            {
                if (!_isInteracting)
                {
                    _isInteracting = true;

                    // TODO: replace the following block of code to make it
                    // display text in UI
                    Debug.Log(currentDialogue.InitialNpcDialogue);
                    string[] responses = currentDialogue.PlayerResponses;
                    string tempPrintString = "";
                    foreach (string response in responses)
                    {
                        tempPrintString += response;
                        tempPrintString += "  ";
                    }
                    Debug.Log(tempPrintString);
                }
                else
                {
                    _isInteracting = false;

                    // TODO: replace the following block of code to make it
                    // display text in UI
                    string[] responses = currentDialogue.NpcResponses;
                    string tempPrintString = "";
                    foreach (string response in responses)
                    {
                        tempPrintString += response;
                        tempPrintString += "  ";
                    }
                    Debug.Log(tempPrintString);
                }
                
            }
        }
    }

    /// <summary>
    /// When called, checks if the NPC should change to a new state
    /// </summary>
    public abstract void CheckForStateChange();

    /// <summary>
    /// To be called by event listener when bypass item is collected
    /// </summary>
    public void CollectedBypassItem()
    {
        _haveBypassItem = true;
    }


    #region StateFunctions
    /// <summary>
    /// The following five State functions are used to have the NPC enter a new state.
    /// Each handles any general setup for the given state.
    /// </summary>
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
        _startMinigameEvent.TriggerEvent(_eventTag);
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

    /// <summary>
    /// Handles functions common to all state changes such as updating animations
    /// or changing position
    /// </summary>
    private void StateUpdateHelper()
    {
        if (_navAgent != null && _navAgent.isOnNavMesh)
        {
            Vector3 newPosition = _navigationPositions.GetStateData(_currentState);
            _navAgent.SetDestination(newPosition);
        }

        if (_animator != null)
        {
            Animation newAnimation = _stateAnimations.GetStateData(_currentState);
            if (newAnimation != null)
            {
                _animator.Play(newAnimation.name);
            }
        }
    }
    #endregion

    #region CollisionChecks
    /// <summary>
    /// Enables ability to interact if the player is close to the NPC
    /// </summary>
    /// <param name="other">Other collider in the collision</param>
    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canInteract = true;
            if (_tabbedMenu != null)
            {
                _tabbedMenu.ToggleInteractPrompt(true);
            }
        }
    }

    /// <summary>
    /// Disables ability to interact if the player moves away from the NPC
    /// </summary>
    /// <param name="other">Other collider in the collision</param>
    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canInteract = false;
            if (_tabbedMenu != null)
            {
                _tabbedMenu.ToggleInteractPrompt(false);
            }
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