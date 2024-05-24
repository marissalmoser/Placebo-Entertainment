using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlaceboEntertainment.UI;

public class DoorBehavior : MonoBehaviour
{
    private PlayerController _pc;
    private bool _interactable;
    private bool _isOpened = false;
    [SerializeField] private bool _isLocked;
    private Animator _anim;

    void Start()
    {
        //Call player controller
        GameObject _pcObject = GameObject.FindWithTag("Player");
        _pc = _pcObject.GetComponent<PlayerController>();
        //bool for when close to Game Object
        _interactable = false;

        _anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //When in zone and press E do thing
        if (_pc.interact.IsPressed() && _interactable && !_isOpened)
        {
            if(!_isLocked)
            {
                OpenDoor();
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        //your in press e zone
        if (col.gameObject.tag == "Player")
        {
            _interactable = true;

            //enable UI
            if(!_isOpened && !_isLocked)
            {
                TabbedMenu.Instance.ToggleInteractPrompt(true, "DOOR");
            }
        }
    }
    void OnTriggerExit(Collider col)
    {
        //your not in press e zone
        if (col.gameObject.tag == "Player")
        {
            _interactable = false;

            //disabe UI
            TabbedMenu.Instance.ToggleInteractPrompt(false);
        }
    }

    private void OpenDoor()
    {
        //print("open sesame");
        _isOpened = true;
        _anim.SetTrigger("_openDoor");
        //disabe UI
        TabbedMenu.Instance.ToggleInteractPrompt(false);
    }

    public void UnlockDoor()
    {
        _isLocked = false;
        if (!_isOpened && _interactable)
        {
            TabbedMenu.Instance.ToggleInteractPrompt(true, "DOOR");
        }

        //if unlocking door should open the door:
        //OpenDoor();
    }
}
