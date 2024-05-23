using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
    }
    void OnTriggerExit(Collider col)
    {
        //your not in press e zone
        if (col.gameObject.tag == "Player")
        {
            _interactable = false;
        }
    }

    private void OpenDoor()
    {
        print("open sesame");
        _isOpened = true;
        _anim.SetTrigger("_openDoor");
    }

    public void UnlockDoor()
    {
        _isLocked = false;
        OpenDoor();
    }
}
