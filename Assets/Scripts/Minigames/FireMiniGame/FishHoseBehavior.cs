/*****************************************************************************
// File Name :         FishHoseBehavior.cs
// Author :            Mark Hanson
// Contributors :      Marissa Moser, Nick Grinstead, Andrea Swihart-DeCoster
// Creation Date :     6/19/2024
//
// Brief Description : Any function to do with the fish will be found here. 
Fish water gauge, refill function, fish equip function, and UI for water gauge.
*****************************************************************************/

using System.Collections;
using UnityEngine;
using PlaceboEntertainment.UI;
using UnityEngine.InputSystem;
using FMOD;

public class FishHoseBehavior : MonoBehaviour, IInteractable
{
    [SerializeField] private NpcEvent _minigameEndEvent;
    [SerializeField] private GameObject _npcFish;

    [Header("Water Settings")]
    [SerializeField] private float _refillWaitTime;
    [SerializeField] private float _waterMaxAmount;

    [Header("UI")]
    [SerializeField] private string _interactPromptText = "FISH";

    [Header("VFX")]
    [SerializeField] private ParticleSystem _waterSpray;
    [SerializeField] private GameObject _fireAlarmLight;

    private GameObject _rightHand;
    private TabbedMenu _tabbedMenu;

    private float _currentWaterAmount;
    private bool _isEquipped;
    private int _numFires;
    private bool _isShooting;

    private void OnEnable()
    {
        PlayerController.Instance.Shoot.started += OnShoot;
        PlayerController.Instance.Shoot.canceled += OnRelease;
        FireBehavior.OnFireExtinguished += OnFireExtinguished;
    }

    private void OnDisable()
    {
        PlayerController.Instance.Shoot.started -= OnShoot;
        PlayerController.Instance.Shoot.canceled -= OnRelease;
        FireBehavior.OnFireExtinguished -= OnFireExtinguished;
    }

    private void Start()
    {
        _npcFish.SetActive(false);
        _isShooting = false;
        _currentWaterAmount = _waterMaxAmount;

        _rightHand = GameObject.FindWithTag("Righty");

        _tabbedMenu = TabbedMenu.Instance;

        _numFires = FindObjectsOfType<FireBehavior>().Length;
    }

    void FixedUpdate()
    {
        if (_isEquipped)
        {
            transform.position = new Vector3(_rightHand.transform.position.x, _rightHand.transform.position.y, _rightHand.transform.position.z);
            transform.rotation = _rightHand.transform.rotation;
        }
    }

    public void Interact(GameObject player)
    {
        if (!_isEquipped)
        {
            _isEquipped = true;
            _tabbedMenu.ToggleWaterMeter(true);

            transform.position = _rightHand.transform.position;
            transform.rotation = _rightHand.transform.rotation;
            transform.parent = _rightHand.transform;
        }
    }

    private void OnFireExtinguished()
    {
        _numFires--;

        if (_numFires <= 0)
        {
            _tabbedMenu.ToggleWaterMeter(false);

            //GAME ENDS HERE
            EndMinigame();
        }
    }

    /// <summary>
    /// Ends the minigame
    /// </summary>
    private void EndMinigame()
    {
        _minigameEndEvent.TriggerEvent(NpcEventTags.Fish);

        _npcFish.SetActive(true);
        _fireAlarmLight.SetActive(false);

        Destroy(gameObject);
    }

    /// <summary>
    /// When the player holds left click down.
    /// </summary>
    /// <param name="obj"></param>
    private void OnShoot(InputAction.CallbackContext obj)
    {
        if (_isEquipped && !_isShooting)
        {
            if (_currentWaterAmount <= 0)
            {
                return;
            }

            // Shoots water if player is able to when input is given
            if (_currentWaterAmount > 0f)
            {
                StartCoroutine(ShootWater());
            }
            else
            {
                _tabbedMenu.UpdateFishFaceState(0);
            }
        }
    }

    private IEnumerator ShootWater()
    {
        _waterSpray.Play();
        _isShooting = true;

        while (_currentWaterAmount > 0)
        {
            _tabbedMenu.UpdateFishFaceState(1);

            _currentWaterAmount -= 1f;
            _tabbedMenu.UpdateWaterFill(_currentWaterAmount);

            yield return new WaitForEndOfFrame();
        }
        StopWaterSpray();
    }

    private void OnRelease(InputAction.CallbackContext obj)
    {
        if (_isEquipped)
        {
            StopWaterSpray();
        }
    }

    private void StopWaterSpray()
    {
        _isShooting = false;

        _waterSpray.Stop();
        StopCoroutine(ShootWater());
        StartCoroutine(WaitForRefill());

        if (_currentWaterAmount <= 0)
        {
            _tabbedMenu.UpdateFishFaceState(2);
        }
    }

    /// <summary>
    /// Delay before water meter begins to refill.
    /// </summary>
    private IEnumerator WaitForRefill()
    {
        
        yield return new WaitForSeconds(_refillWaitTime);
        StartCoroutine(RefillWater());
    }

    /// <summary>
    /// Refills the water meter.
    /// </summary>
    private IEnumerator RefillWater()
    {
        _tabbedMenu.UpdateFishFaceState(0);

        while (_currentWaterAmount < _waterMaxAmount)
        {
            _currentWaterAmount += 0.5f;
            _tabbedMenu.UpdateWaterFill(_currentWaterAmount);

            yield return new WaitForEndOfFrame();
        }

        // Once water meter is full
        _currentWaterAmount = _waterMaxAmount;
    }

    /// <summary>
    /// Shows UI prompt for wrench
    /// </summary>
    public void DisplayInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(true, _interactPromptText);
    }

    /// <summary>
    /// Hides UI prompt for wrench
    /// </summary>
    public void HideInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(false);
    }
}
