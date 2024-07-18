/*****************************************************************************
// File Name :         FishBehavior.cs
// Author :            Mark Hanson
// Contributors :      Marissa Moser
// Creation Date :     6/19/2024
//
// Brief Description : Any function to do with the fish will be found here. 
Fish water gauge, refill function, fish equip function, and UI for water gauge.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using PlaceboEntertainment.UI;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class FishBehavior : MonoBehaviour, IInteractable
{
    [SerializeField] private NpcEvent _minigameEndEvent;

    public PlayerControls _playerControls;
    public InputAction leftclick;

    [Header("Fish outside hand functions")]
    private bool _isEquipped;
    [SerializeField] private GameObject _rightHand;
    [SerializeField] private GameObject _fireSet;

    [Header("Fish within hand functions")]
    [SerializeField] private GameObject _water;
    private float _waterAmount;
    [SerializeField] private float _refillWaitTime;
    [SerializeField] private float _waterMaxAmount;

    [Header("Fish overall functions")]
    [SerializeField] private GameObject _npcFish;
    private bool _refillNow;
    private bool _refilled;
    [SerializeField] private GameObject _firePoint;
    private bool _doOnce;
    private bool _usingWater;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Collider coll;

    [Header("UI Stuff")]
    [SerializeField] private Slider _waterGaugeUI;
    [SerializeField] private GameObject _canvasHolderUI;

    [Header("VFX Stuff")]
    [SerializeField] private ParticleSystem _waterSpray;
    [SerializeField] private GameObject _fireAlarmLight;


    void Awake()
    {
        _refillNow = false;
        _rightHand = GameObject.FindWithTag("Righty");
        _waterAmount = _waterMaxAmount;
        _refilled = true;
        _doOnce = true;

        _playerControls = new PlayerControls();
        _playerControls.BasicControls.Enable();
        leftclick = _playerControls.FindAction("LeftClick");
        _npcFish.SetActive(false);
        _waterSpray.Stop();
    }

    // Start is called before the first frame update
    void Start()
    {
        _canvasHolderUI.SetActive(false);
    }

    void FixedUpdate()
    {
        if (_isEquipped)
        {
            transform.position = new Vector3(_rightHand.transform.position.x, _rightHand.transform.position.y, _rightHand.transform.position.z);
            transform.rotation = _rightHand.transform.rotation;
        }
        _waterGaugeUI.maxValue = _waterMaxAmount;
        _waterGaugeUI.value = _waterAmount;
    }

    // Update is called once per frame
    void Update()
    {
        //if(!_isEquipped)
        //{
        //    _fireSet.SetActive(false);
        //}

        if (leftclick.IsPressed() && _isEquipped && _waterAmount > 0f && _refilled == true)
        {
            _waterAmount -= 1f;
            _refillNow = false;
            Vector3 _firePosition = new Vector3(_firePoint.transform.position.x, _firePoint.transform.position.y, _firePoint.transform.position.z);
            Instantiate(_water, _firePosition, Quaternion.identity);
            _waterSpray.Play();
        }
        else
        {
            _waterSpray.Stop();
        }
        
        GameObject _waterFinder = GameObject.FindWithTag("Water");
        if (_waterFinder == null)
        {
            _waterAmount += 1.5f;
        }
        if (_refillNow)
        {
            _waterAmount += 0.5f;
        }
        if (_waterAmount <= 0f && _refillNow == false && _doOnce)
        {
            StartCoroutine(WaitForRefill());
            _doOnce = false;
        }
        if(_waterAmount >= _waterMaxAmount)
        {
            _refillNow = false;
        }
        if (_waterAmount > _waterMaxAmount)
        {
            _waterAmount = _waterMaxAmount;
        }
        if (_waterAmount == _waterMaxAmount)
        {
            _refilled = true;
            _doOnce = true;
        }
        GameObject _fireFinder = GameObject.FindWithTag("Fire");
        if (_fireFinder == null && _isEquipped)
        {
            _canvasHolderUI.SetActive(false);
            coll.isTrigger = false;
            _rb.useGravity = true;
            transform.parent = null;
            _isEquipped = false;
            Destroy(gameObject);

            //GAME ENDS HERE
            _npcFish.SetActive(true);
            _minigameEndEvent.TriggerEvent(NpcEventTags.Fish);
            _fireAlarmLight.SetActive(false);
        }
    }
    
    public void Interact(GameObject player)
    {
        if (!_isEquipped)
        {
            //_fireSet.SetActive(true);
            _isEquipped = true;
            transform.position = _rightHand.transform.position;
            transform.rotation = _rightHand.transform.rotation;
            transform.parent = _rightHand.transform;
            _canvasHolderUI.SetActive(true);
        }
    }

    IEnumerator WaitForRefill()
    {
        _refilled = false;
        _refillNow = false;
        _waterAmount = 0;
        yield return new WaitForSeconds(_refillWaitTime);
        _refillNow = true;
    }

    /// <summary>
    /// Shows UI prompt for wrench
    /// </summary>
    public void DisplayInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(true, "FISH");
    }

    /// <summary>
    /// Hides UI prompt for wrench
    /// </summary>
    public void HideInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(false);
    }


}
