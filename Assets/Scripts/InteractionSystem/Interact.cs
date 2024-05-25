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
using PlaceboEntertainment.UI;

public class Interact : MonoBehaviour
{
    private InputActionMap _map;
    private InputAction _interact;

    [SerializeField] private GameObject _targetGameObj;
    [SerializeField] private GameObject _interactable;
    public bool CanInteract { get; private set; }

    //raycast variables
    private RaycastHit _colliderHit;
    [SerializeField] private float _maxInteractDistance;
    [SerializeField] LayerMask _layerToIgnore;

    private void Awake()
    {
        _map = GetComponent<PlayerInput>().currentActionMap;
        _map.Enable();

        _interact = _map.FindAction("Interact");
        _interact.started += InteractPressed;

        CanInteract = true;
        StartCoroutine(DetectInteractable());
    }

    /// <summary>
    /// Raycasts in front of player to check if there is an item to be interacted
    /// with, if so , calls the interaction behavior on that object.
    /// </summary>
    private void InteractPressed(InputAction.CallbackContext ctx)
    {
        if(_interactable != null)
        {
            //Calls Interact() on the target game object
            _targetGameObj.GetComponent<Interactable>().Interact(gameObject);
        }
          
    }

    private IEnumerator DetectInteractable()
    {
        while(CanInteract)
        {
            //Casts Raycast in the center of the screen
            Ray r = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(r, out _colliderHit, _maxInteractDistance, ~_layerToIgnore))
            {
                _targetGameObj = _colliderHit.transform.gameObject;

                if (_targetGameObj.TryGetComponent(out Interactable interactable))
                {
                    _interactable = _targetGameObj;
                    _interactable.GetComponent<Interactable>().DisplayInteractUI();
                }
                else
                {
                    _interactable = null;
                    //TabbedMenu.Instance.ToggleInteractPrompt(false);
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
