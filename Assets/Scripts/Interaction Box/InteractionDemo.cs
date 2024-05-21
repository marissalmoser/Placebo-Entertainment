/*****************************************************************************
// File Name :         InteractionDemo.cs
// Author :            Mark Hanson
// Creation Date :     5/16/2024
//
// Brief Description : Demostration script to show how to use interact for different future assets
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDemo : MonoBehaviour
{
    private PlayerController _pc;
    private bool _interactable;
    // Start is called before the first frame update
    void Start()
    {
       //Call player controller
        GameObject _pcObject = GameObject.FindWithTag("Player");
       _pc = _pcObject.GetComponent<PlayerController>();
        //bool for when close to Game Object
       _interactable = false;
    }

    void FixedUpdate()
    {
        //When in zone and press E say HIIIIII
        if(_pc.interact.IsPressed() && _interactable == true)
        {
            Debug.Log("HI");
        }
    }

    void OnTriggerEnter(Collider col)
    {
        //your in press e zone
        if(col.gameObject.tag == "Player")
        {
            _interactable = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        //your not in press e zone
        if(col.gameObject.tag == "Player")
        {
            _interactable = false;
        }
    }
}
