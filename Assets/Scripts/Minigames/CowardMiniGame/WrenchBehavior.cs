/*****************************************************************************
// File Name :         WrenchBehavior.cs
// Author :            Mark Hanson
// Contributors :      Marissa Moser
// Creation Date :     5/27/2024
//
// Brief Description : Any function to do with the wrench will be found here. Wrench swinging, spark interaction, and completion of this segment of the minigame.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlaceboEntertainment.UI;
using System;

public class WrenchBehavior : MonoBehaviour, IInteractable
{
    [SerializeField] private NpcEvent _minigameEndEvent;

    [Header("UI Stuff")]
    [SerializeField] private TextMeshPro _smackedText;

    [Header("Wrench overall functions")]
    [SerializeField] private GameObject _sparksMode;
    //private PlayerController _pc;
    private int _sparkSmacked;
    [SerializeField] private int _maxSpark;

    [Header("Wrench within hand functions")]
    [SerializeField] private Animator _animate;
    [SerializeField] private GameObject _wrenchSpark;
    //private bool _swing;
    [Header("Wrench outside hand functions")]
    [SerializeField] private GameObject _rightHand;
    private bool _withinProx;
    private bool _isEquipped;

    public static Action SparkSmackedAction;

    void Awake()
    {
        _rightHand = GameObject.FindWithTag("Righty");
        _sparksMode = GameObject.Find("SparksMode");
        GameObject _smackTextObject = GameObject.Find("Spark num");
        _smackedText = _smackTextObject.GetComponent<TextMeshPro>();
        GameObject _pc = GameObject.FindWithTag("RightArm");
        _animate = _pc.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //GameObject _pcObject = GameObject.FindWithTag("Player");
        //_pc = _pcObject.GetComponent<PlayerController>();
        _isEquipped = false;
        //_withinProx = false;
        //_swing = false;
        SparkSmackedAction += SparkSmacked;
    }
    void FixedUpdate()
    {
        if(_isEquipped == true)
        {
            transform.position = new Vector3(_rightHand.transform.position.x, _rightHand.transform.position.y, _rightHand.transform.position.z);
            transform.rotation = _rightHand.transform.rotation;
        }
    }

    /// <summary>
    /// A coroutine to manage the swinging animation.
    /// </summary>
    /// <returns></returns>
    IEnumerator Swinging()
    {
        //print("swing");
        GetComponent<Collider>().enabled = true;
        _animate.SetBool("_isSwinging", true);
        //_swing = true;
        yield return new WaitForSeconds(1f);
        //_swing = false;
        GetComponent<Collider>().enabled = false;
        _animate.SetBool("_isSwinging", false);
    }
    IEnumerator SystematicShutDown()
    {
        yield return new WaitForSeconds(1.1f);
        gameObject.SetActive(false);
    }
    /// <summary>
    /// This function is invoked in SparkInteractBehavior whenever a spark is interacted
    /// with. It keeps track of the number of sparks that have been smacked and ends the game.
    /// </summary>
    private void SparkSmacked()
    {
        if (_isEquipped == true)
        {
            _sparkSmacked++;
            _smackedText.text = _sparkSmacked.ToString();
            StartCoroutine(Swinging());
            if (_sparkSmacked >= _maxSpark)
            {
                _smackedText.color = Color.green;
                _sparksMode.SetActive(false);
                StartCoroutine(SystematicShutDown());

                //game ends here?
                _minigameEndEvent.TriggerEvent(NpcEventTags.Coward);
                print("game end");
            }
        }
    }
    /// <summary>
    /// This function is called when the player interacts with the wrench.
    /// </summary>
    /// <param name="player"></param>
    public void Interact(GameObject player)
    {
        if (_isEquipped == false)
        {
            //_animate.SetTrigger("pickedUp");
            _isEquipped = true;
            GetComponent<Collider>().enabled = false;
            transform.position = _rightHand.transform.position;
            transform.rotation = _rightHand.transform.rotation;
            transform.parent = _rightHand.transform;
        }
    }

    /// <summary>
    /// Shows UI prompt for wrench
    /// </summary>
    public void DisplayInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(true, "WRENCH");
    }

    /// <summary>
    /// Hides UI prompt for wrench
    /// </summary>
    public void HideInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(false);
    }

    private void OnDisable()
    {
        SparkSmackedAction -= SparkSmacked;
    }
}
