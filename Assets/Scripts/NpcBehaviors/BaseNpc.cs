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
    #region Structs
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
    /// Acts as a node in the dialogue tree, contains NPC's dialogue and potential
    /// player responses
    /// </summary>
    [System.Serializable]
    protected struct DialogueNode
    {
        [SerializeField] private NpcEvent _eventToTrigger;
        [SerializeField] private string _eventTag;
        [SerializeField] private string[] _dialogue;
        [SerializeField] private bool _endsDialogue;
        [SerializeField] private PlayerResponse[] _playerResponses;

        public NpcEvent EventToTrigger { get => _eventToTrigger; }
        public string EventTag { get => _eventTag; }
        public string[] Dialogue { get => _dialogue; }
        public bool EndsDialogue { get => _endsDialogue; }
        public PlayerResponse[] PlayerResponses { get => _playerResponses; }
    }

    /// <summary>
    /// Structure for a player response, containing a dialogue string, the index
    /// of the next node, and other any prerequisite checks
    /// </summary>
    [System.Serializable]
    protected struct PlayerResponse
    {
        [SerializeField] private bool _hasPrerequisiteCheck;
        [SerializeField] private string _answer;
        [SerializeField] private int _nextResponseIndex;
        [SerializeField] private bool _endsDialogue;

        public bool HasPrerequisiteCheck { get => _hasPrerequisiteCheck; }
        public string Answer { get => _answer; }
        public int NextResponseIndex { get => _nextResponseIndex; }
        public bool EndsDialogue { get => _endsDialogue; }
    }
    #endregion

    [SerializeField] protected NpcEvent _startMinigameEvent;
    [SerializeField] protected string _eventTag;

    [SerializeField] protected StateDataGroup<DialogueNode[]> _stateDialogueTrees;
    [SerializeField] protected StateDataGroup<Vector3> _navigationPositions;
    [SerializeField] protected StateDataGroup<Animation> _stateAnimations;

    protected PlayerControls _playerControls;
    protected NavMeshAgent _navAgent;
    protected Animator _animator;
    protected TabbedMenu _tabbedMenu;

    protected NpcStates _currentState = NpcStates.DefaultIdle;

    protected int _currentDialogueIndex = 0;
    protected bool _canInteract = false;
    protected bool _shouldEndDialogue = false;
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

    #region DialogueFunctions
    /// <summary>
    /// Called when player presses button to interact to initiate an interaction with
    /// a NPC or when giving a dialogue response.
    /// </summary>
    /// <param name="responseIndex">Index in the dialogue tree, assumes 0 by default</param>
    public void Interact(int responseIndex = 0)
    {
        if (_canInteract)
        {
            int newNodeIndex = 0;

            if (_isInteracting == false)
            {
                if (_tabbedMenu != null)
                {
                    _tabbedMenu.ToggleInteractPrompt(true);
                }
                _isInteracting = true;
                _currentDialogueIndex = 0;
                // TODO: turn on dialogue UI here
            }
            else
            {   
                // If you're already talking, determines which node to go to based on player response
                DialogueNode currentNode = _stateDialogueTrees.GetStateData(_currentState)[_currentDialogueIndex];
                if (responseIndex < currentNode.PlayerResponses.Length)
                {
                    newNodeIndex = currentNode.PlayerResponses[responseIndex].NextResponseIndex;
                }
            }

            if (_shouldEndDialogue)
            {
                _isInteracting = false;
                _shouldEndDialogue = false;
                // TODO: turn off dialogue UI here
                return;
            }
            
            GetNpcResponse(newNodeIndex);
        }
    }

    /// <summary>
    /// Called to display an NPC's dialogue for a given index of the dialogue tree
    /// </summary>
    /// <param name="nextNodeIndex">Index in the dialogue tree</param>
    protected void GetNpcResponse(int nextNodeIndex)
    {
        if (_isInteracting && nextNodeIndex < _stateDialogueTrees.GetStateData(_currentState).Length)
        {
            // TODO: hide player response buttons here

            _currentDialogueIndex = nextNodeIndex;
            DialogueNode currentNode = _stateDialogueTrees.GetStateData(_currentState)[_currentDialogueIndex];
            Debug.Log(currentNode.Dialogue[0]); // TODO: display dialogue here

            // Triggers a dialogue event if there is one
            if (currentNode.EventToTrigger != null)
            {
                currentNode.EventToTrigger.TriggerEvent(currentNode.EventTag);
            }

            // Checks if this node is a leaf
            if (currentNode.EndsDialogue)
            {
                _shouldEndDialogue = true;
                return;
            }

            GetPlayerResponses();
        }
    }

    /// <summary>
    /// Called by GetNpcResponse to display corresponding player responses
    /// </summary>
    protected void GetPlayerResponses()
    {
        if (_isInteracting)
        {
            DialogueNode currentNode = _stateDialogueTrees.GetStateData(_currentState)[_currentDialogueIndex];

            // Displays player dialogue options
            PlayerResponse option;
            for (int i = 0; i < currentNode.PlayerResponses.Length; ++i)
            {
                option = currentNode.PlayerResponses[i];

                // Checking if option should display
                if (!option.HasPrerequisiteCheck || CheckDialoguePrerequisite(option))
                {
                    Debug.Log(option.Answer); // TODO: display player option in UI
                }
            }
        }
    }

    /// <summary>
    /// This method should be overriden by subclasses and filled with NPC specific
    /// logic. That overriden function should not call this base function.
    /// </summary>
    /// <returns>Whether prerequisite is met or not</returns>
    protected virtual bool CheckDialoguePrerequisite(PlayerResponse option)
    {
        return true;
    }
    #endregion

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