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

public class FishHoseBehavior : MonoBehaviour, IInteractable
{
    public static FishHoseBehavior Instance;

    [SerializeField] private Collider _waterCollisionCollider;
    [SerializeField] private GameObject _positionInHand;

    [Header("Water Settings")]
    [SerializeField] private float _refillWaitTime;

    [Range(0f, 100f)]   // water meter UI will break if exceeds 100
    [SerializeField] private int _maxWaterAmount;

    [Tooltip("Smaller values will lower the rate the water meter decreases")]
    [SerializeField] private float _meterSpendRate;

    [Tooltip("Smaller values will lower the rate the water meter increase")]
    [SerializeField] private float _meterRefillRate;

    [Header("UI")]
    [SerializeField] private string _interactPromptText = "FISH";

    [Header("VFX")]
    [SerializeField] private ParticleSystem _waterSpray;

    private TabbedMenu _tabbedMenu;
    private Animator _anim;

    public static GameObject FishModel { get; private set; }

    private float _currentWaterAmount;
    private bool _isEquipped;
    private bool _isShooting;

    private void OnEnable()
    {
        PlayerController.Instance.Shoot.started += OnShoot;
        PlayerController.Instance.Shoot.canceled += OnRelease;
    }

    private void OnDisable()
    {
        PlayerController.Instance.Shoot.started -= OnShoot;
        PlayerController.Instance.Shoot.canceled -= OnRelease;
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        _isShooting = false;
        _currentWaterAmount = _maxWaterAmount;

        _waterCollisionCollider.gameObject.SetActive(false);

        _tabbedMenu = TabbedMenu.Instance;

        _anim = GetComponentInChildren<Animator>();
    }

    public void Interact(GameObject player)
    {
        if (!_isEquipped)
        {
            _isEquipped = true;
            _tabbedMenu.ToggleWaterMeter(true);
            _anim.SetTrigger("Picked");

            transform.position = _positionInHand.transform.position;
            transform.rotation = _positionInHand.transform.rotation;
            transform.parent = _positionInHand.transform;
            transform.localScale = new Vector3(.7f, .7f, .7f); // This resizes the fish,
                                                               // but the water spray is still at the old mouth
        }
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
                StopAllCoroutines();
                StartCoroutine(ShootWater());
                _anim.SetBool("Firing", true);
            }
        }
    }

    /// <summary>
    /// Water shooting logic
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootWater()
    {
        _waterCollisionCollider.gameObject.SetActive(true);
        _waterSpray.Play();
        _isShooting = true;
        _tabbedMenu.UpdateFishFaceState(1);

        while (_currentWaterAmount > 0)
        {
            _currentWaterAmount -= _meterSpendRate;
            _tabbedMenu.UpdateWaterFill(_currentWaterAmount);
            yield return new WaitForEndOfFrame();
        }
        _anim.SetBool("Firing", false);
        StopWaterSpray();
    }

    /// <summary>
    /// When player lets go of shoot
    /// </summary>
    /// <param name="obj"></param>
    private void OnRelease(InputAction.CallbackContext obj)
    {
        if (_isEquipped && _isShooting)
        {
            _anim.SetBool("Firing", false);
            StopWaterSpray();
        }
    }

    /// <summary>
    /// Controls the logic when the water spray should stop
    /// </summary>
    private void StopWaterSpray()
    {
        _waterCollisionCollider.gameObject.SetActive(false);
        StopAllCoroutines();    // I wasn't able to explicitly stop the ShootWater coroutine
        _waterSpray.Stop();
        _isShooting = false;

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

        while (_currentWaterAmount < _maxWaterAmount)
        {
            _currentWaterAmount += _meterRefillRate;
            _tabbedMenu.UpdateWaterFill(_currentWaterAmount);

            yield return new WaitForEndOfFrame();
        }

        // Once water meter is full
        _currentWaterAmount = _maxWaterAmount;
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
