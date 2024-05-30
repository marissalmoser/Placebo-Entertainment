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
using TMPro;
using UnityEngine.UI;

public class WrenchBehavior : MonoBehaviour
{
    [Header("UI Stuff")]
    [SerializeField] private TextMeshPro _smackedText;

    [Header("Wrench overall functions")]
    [SerializeField] private GameObject _sparksMode;
    private PlayerController _pc;
    private int _sparkSmacked;

    [Header("Wrench within hand functions")]
    [SerializeField] private Animator _animate;
    [SerializeField] private GameObject _wrenchSpark;
    private bool _swing;
    [Header("Wrench outside hand functions")]
    [SerializeField] private GameObject _rightHand;
    private bool _withinProx;
    private bool _isEquipped;

    void Awake()
    {
        _rightHand = GameObject.FindWithTag("Righty");
        _sparksMode = GameObject.Find("SparksMode");
        GameObject _smackTextObject = GameObject.Find("Spark num");
        _smackedText = _smackTextObject.GetComponent<TextMeshPro>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject _pcObject = GameObject.FindWithTag("Player");
        _pc = _pcObject.GetComponent<PlayerController>();
        _isEquipped = false;
        _withinProx = false;
        _swing = false;
    }
    void FixedUpdate()
    {
        if (_pc.interact.IsPressed() && _isEquipped == false && _withinProx == true)
        {
            transform.parent = _rightHand.transform;
            _isEquipped = true;
        }
        if(_isEquipped == true)
        {
            transform.position = new Vector3(_rightHand.transform.position.x, _rightHand.transform.position.y, _rightHand.transform.position.z);
            transform.rotation = _rightHand.transform.rotation;
        }
        if(_pc.interact.IsPressed() && _isEquipped == true)
        {
            StartCoroutine(Swinging());
        }
        Debug.Log(_isEquipped);
    }
    // Update is called once per frame
    void Update()
    {
        _smackedText.text = _sparkSmacked.ToString();
        if(_swing == true && _isEquipped == true)
        {
            GetComponent<Collider>().enabled = true;
        }
        if(_swing == false && _isEquipped == true)
        {
            GetComponent<Collider>().enabled = false;
        }
        if(_sparkSmacked == 5)
        {
            _smackedText.color = Color.green;
            _sparksMode.SetActive(false);
            gameObject.SetActive(false);
        }
    }
    IEnumerator Swinging()
    {
        _animate.SetBool("isSwinging", true);
        _swing = true;
        yield return new WaitForSeconds(0.1f);
        _swing = false;
        _animate.SetBool("isSwinging", false);
    }
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Spark")
        {
            _sparkSmacked++;
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
