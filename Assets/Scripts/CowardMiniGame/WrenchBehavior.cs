/*****************************************************************************
// File Name :         WrenchBehavior.cs
// Author :            Mark Hanson
// Creation Date :     5/27/2024
//
// Brief Description : Any function to do with the wrench will be found here. Wrench swinging, spark interaction, and completion of this segment of the minigame.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchBehavior : MonoBehaviour
{
    [Header("Wrench overall functions")]
    private PlayerController _pc;

    [Header("Wrench within hand functions")]
    [SerializeField] private Animator _animate;
    [SerializeField] private GameObject _wrenchSpark;
    [Header("Wrench outside hand functions")]
    [SerializeField] private GameObject _rightHand;
    private bool _withinProx;
    private bool _isEquipped;

    // Start is called before the first frame update
    void Start()
    {
        GameObject _pcObject = GameObject.FindWithTag("Player");
        _pc = _pcObject.GetComponent<PlayerController>();
        _isEquipped = false;
        _withinProx = false;
    }
    void FixedUpdate()
    {
        if (_pc.interact.IsPressed() && _isEquipped == false && _withinProx == true)
        {

            transform.position = _rightHand.transform.position;
            transform.parent = _rightHand.transform;
            _isEquipped=true;
        }

        if(_pc.interact.IsPressed() && _isEquipped == true)
        {

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Spark")
        {
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "Player")
        {
            _withinProx = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            _withinProx = false;
        }
    }
}
