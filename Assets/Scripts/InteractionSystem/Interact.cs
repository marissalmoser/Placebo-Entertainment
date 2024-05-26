/******************************************************************
*    Author: Marissa Moser
*    Contributors: 
*    Date Created: May 24, 2024
*    Description: 
*       Contains a function called when the player presses E. This function will:
            Use a raycast to check if there is something in from of them
            to interact with and if it is interactable. If so, call the
            Interact(player) function on that game object

*******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    private InputActionMap _map;
    private InputAction _interact;

    [SerializeField] private GameObject _targetGameObj;
    [SerializeField] private GameObject _interactable;
    private bool _canInteract;

    //raycast variables
    private RaycastHit _colliderHit;
    [SerializeField] private float _maxInteractDistance;
    [SerializeField] LayerMask _layerToIgnore;

    private void Awake()
    {
        //is there a better way to do this?
        _map = GetComponent<PlayerInput>().currentActionMap;
        _map.Enable();
        _interact = _map.FindAction("Interact");
        _interact.started += InteractPressed;

        StartDetectingInteractions();
    }

    /// <summary>
    /// Called when Interact input is started. Calls Interact() on the detected
    /// interactable game object
    /// </summary>
    private void InteractPressed(InputAction.CallbackContext ctx)
    {
        if(_interactable != null)
        {
            _interactable.GetComponent<IInteractable>().Interact(gameObject);
        }
          
    }

    /// <summary>
    /// Starts the Detect Interactable coroutine
    /// </summary>
    public void StartDetectingInteractions()
    {
        _canInteract = true;
        StartCoroutine(DetectInteractable());
    }

    /// <summary>
    /// Ends the Detect Interactable coroutine
    /// </summary>
    public void StopDetectingInteractions()
    {
        _canInteract = false;
    }

    /// <summary>
    /// A coroutine that detects if there is an interactable object in front of
    /// the player using a raycast. This coroutine can be stopped with the public 
    /// Start/StopDetectingInteraction function
    /// </summary>
    /// <returns></returns>
    private IEnumerator DetectInteractable()
    {
        while(_canInteract)
        {
            //Casts Raycast in the center of the screen
            Ray r = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(r, out _colliderHit, _maxInteractDistance, ~_layerToIgnore))
            {
                _targetGameObj = _colliderHit.transform.gameObject;

                //sets the _interactable variable for the InteractPressed function
                if (_targetGameObj.TryGetComponent(out IInteractable interactable))
                {
                    _interactable = _targetGameObj;
                    _interactable.GetComponent<IInteractable>().DisplayInteractUI();
                }
                else if (_interactable != null)
                {
                    _interactable.GetComponent<IInteractable>().HideInteractUI();
                    _interactable = null;
                }
            }
            yield return null;
        }
    }


    private void OnDisable()
    {
        _interact.started -= InteractPressed;
    }
}
