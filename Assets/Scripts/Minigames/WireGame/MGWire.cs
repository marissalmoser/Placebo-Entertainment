/*****************************************************************************
// File Name :         MGWire.cs
// Author :            Andrea Swihart-DeCoster
// Contributor :       Nick Grinstead
// Creation Date :     05/21/24
//
// Brief Description : Contains the logic and properties for the wire itself
                       and any relevant gameplay logic for how the wire works.
*****************************************************************************/

using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering;
using PlaceboEntertainment.UI;

public class MGWire : MonoBehaviour
{
    public EWireID WireID;
    [SerializeField] private Color _wireColor;
    [SerializeField] private Color _interactColor;

    [SerializeField] float _sphereScale;

    [SerializeField] private Transform _wireStartPosition;
    [SerializeField] private Transform _wireEndPosition;
    [SerializeField] private float _distanceFromPlayer;
    [SerializeField] private float _maxLength;

    private bool _canConnectToSlot = false;

    private MGWireSlot _currentSlot = null;
    private MGWireMovement _mgWireMovement;
    private bool _isCorrectlySlotted = false;

    private bool _isInteracting = false;
    private bool _canInteract = false;
    private Transform _cameraTrans;
    private TabbedMenu _tabbedMenu;
    private bool _minigameStarted = false;

    public enum EWireID
    {
        ONE, TWO, THREE
    }

    private void Start()
    {
        if (TryGetComponent<MGWireMovement>(out MGWireMovement wireMove))
        {
            _mgWireMovement = wireMove;
        }

        _tabbedMenu = TabbedMenu.Instance;
        _cameraTrans = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }

    /// <summary>
    /// Invoked by start minigame event triggered by Robot to enable wire interactions
    /// </summary>
    public void StartMinigame()
    {
        _minigameStarted = true;
        _canInteract = true;
        MGWireState.WireGameWon += EndMinigame;
    }

    /// <summary>
    /// Invoked by minigame won event to disable wire interactions
    /// </summary>
    private void EndMinigame()
    {
        _minigameStarted = false;
        MGWireState.WireGameWon -= EndMinigame;
    }

    /// <summary>
    /// Called toggle interacting with this wire
    /// </summary>
    /// <param name="player">Player interacting with the wire</param>
    public void Interact()
    {
        // Can't interact with wires unless the minigame has begun
        if (_minigameStarted && _canInteract)
        {
            if (!_isInteracting)
            {
                OnInteract();
            }
            else
            {
                OnDrop();
            }
        }
    }

    /// <summary>
    /// For interaction system to toggle interact prompt on
    /// </summary>
    public void DisplayInteractUI()
    {
        if (_tabbedMenu != null && _canInteract)
        {
            _tabbedMenu.ToggleInteractPrompt(true, "MOVE");
        }
    }

    /// <summary>
    /// For interaction system to toggle interact prompt off
    /// </summary>
    public void HideInteractUI()
    {
        if (_tabbedMenu != null)
        {
            _tabbedMenu.ToggleInteractPrompt(false);
        }
    }

    /// <summary>
    /// Make wire end move to a position in front of the camera if the player
    /// is interacting with this wire
    /// </summary>
    private void FixedUpdate()
    {
        if (_isInteracting)
        {
            Vector3 target = _cameraTrans.position + _cameraTrans.forward * _distanceFromPlayer;
            _wireEndPosition.position = target;

            if (Vector3.Distance(_wireEndPosition.position, _wireStartPosition.position) > _maxLength)
            {
                OnDrop();
            }
        }
    }

    /// <summary>
    /// Called when the player interacts with the wire. Changes the wire
    /// to be kinematic so the player has direct control over it.
    /// </summary>
    private void OnInteract()
    {
        _isInteracting = true;
        _mgWireMovement.ChangeEndKinematic(true);
    }

    /// <summary>
    /// When the player lets go of the wire, kinematic is turned on and
    /// attempts to place the wire in a slot
    /// </summary>
    private void OnDrop()
    {
        _isInteracting = false;
        _mgWireMovement.ChangeEndKinematic(true);

        PlaceWire();
    }

    /// <summary>
    /// Called when the trigger on the end of the wire is entered.
    /// Enables connection with the slot
    /// </summary>
    /// <param name="slot"></param>
    public void EndTriggerEnter(MGWireSlot slot)
    {
        _canConnectToSlot = true;
        _currentSlot = slot;

        // TODO: This is not a good spot to call PlaceWire(); Find a better
        // solution where the placewire function properly "slots" the wire
        OnDrop();
    }
    
    /// <summary>
    /// Called when the player moves the end of the wire outside of a slots 
    /// trigger
    /// </summary>
    public void EndTriggerExit()
    {
        _isCorrectlySlotted = false;
        _canConnectToSlot = false;
        _currentSlot = null;
    }

    /// <summary>
    /// Called after the player drops the wire. This attempts to connect it
    /// to a slot, otherwise kinematics are disabled and it responds to
    /// physics.
    /// </summary>
    private void PlaceWire()
    {
        if (_canConnectToSlot && _currentSlot && !_isCorrectlySlotted)
        {
            _isCorrectlySlotted = _currentSlot.CheckWire(this);

            // Prevents the moving of wires that are already in the right place
            if (_isCorrectlySlotted)
            {
                _canInteract = false;
            }
        }
        else if (!_canConnectToSlot)
        {
            _mgWireMovement.ChangeEndKinematic(false);
        }
    }

    /// <summary>
    /// Creates a sphere on a segment to visualize the wire. This is temporary
    /// until we have art assets
    /// </summary>
    /// <param name="parentObj">parent for the sphere</param>
    /// <param name="isEndSegment">defaulted to false, true if this is 
    /// the last sphere so it knows to set it to a unique color.</param>
    public void CreateSegmentSphere(Transform parentObj, bool isEndSegment = false)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        Destroy(sphere.GetComponent<Collider>());

        sphere.transform.parent = parentObj;
        sphere.transform.position = parentObj.position;
        sphere.transform.localScale = new Vector3(1f, 1f, 1f);

        sphere.transform.localScale *= _sphereScale;

        SetWireColor(sphere.GetComponent<MeshRenderer>().material, isEndSegment);
    }

    /// <summary>
    /// Sets the color of the sphere of the wire. Each wire has a different 
    /// color to indicate where it will be slotted [Will be a different
    /// indication method after first playable]
    /// </summary>
    /// <param name="mat">material for the color to be applied</param>
    /// <param name="isEndSegment">Unique color is set if it's the 
    /// last segment</param>
    private void SetWireColor(Material mat, bool isEndSegment)
    {
        if(!isEndSegment)
        {
            switch (WireID)
            {
                case EWireID.ONE:
                    mat.color = _wireColor;
                    break;
                case EWireID.TWO:
                    mat.color = _wireColor;
                    break;
                case EWireID.THREE:
                    mat.color = _wireColor;
                    break;
            }
        }
        else
        {
            mat.color = _interactColor;
        }
        
    }
}