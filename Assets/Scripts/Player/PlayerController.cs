/*****************************************************************************
// File Name :         PlayerController.cs
// Author :            Nick Grinsteasd & Mark Hanson, Andrea Swihart-DeCoster
// Creation Date :     5/16/2024
//
// Brief Description : All player actions which includes movement, looking around, and interacting with the world
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _jumpForce;

    public PlayerControls PlayerControls { get; private set; }
    public InputAction Move, Interact, Reset, Quit;

    Rigidbody _rb;
    CinemachineVirtualCamera _mainCamera;
    CinemachineTransposer _transposer;

    PlayerInteractSystem InteractionCheck;
    private bool _doOnce;
    private bool _isInDialogue = false;

    [SerializeField] bool _isKinemat;

    private bool _isMoving = false;
    private Vector2 _moveDirection;
    private Vector3 _velocity;

    private bool _isGrounded = true;
    private float _groundedDistance = 0.3f;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] Transform _groundChecker;

    private void OnEnable()
    {
        Move.performed += ctx => _moveDirection = Move.ReadValue<Vector2>();
        Move.performed += ctx => _isMoving = true;
        Move.canceled += ctx => _moveDirection = Move.ReadValue<Vector2>();
        Move.canceled += ctx => _isMoving = false;
        Move.canceled += ctx => _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
    }

    void Awake()
    {
        Instance = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _rb = GetComponent<Rigidbody>();
        _mainCamera = FindObjectOfType<CinemachineVirtualCamera>();
        _transposer = _mainCamera.GetCinemachineComponent<CinemachineTransposer>();

        PlayerControls = new PlayerControls();
        PlayerControls.BasicControls.Enable();

        InteractionCheck = new PlayerInteractSystem("Default None");
        _doOnce = true;

        _mainCamera.transform.rotation = transform.rotation;

        Move = PlayerControls.FindAction("Move");
        Interact = PlayerControls.FindAction("Interact");
        Reset = PlayerControls.FindAction("Reset");
        Quit = PlayerControls.FindAction("Quit");
    }
    void FixedUpdate()
    {
        // Player Movement
        if (!_isInDialogue && _isMoving)
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
            _velocity = transform.right * _moveDirection.x + transform.forward * _moveDirection.y;
            _velocity = _velocity.normalized * _moveSpeed;
            _rb.AddForce(_velocity, ForceMode.VelocityChange);
        }
        if(_isKinemat)
        {
            _rb.isKinematic = true;
        }
        if(_isKinemat == false)
        {
            _rb.isKinematic = false;
        }

        // Ground Check
        if (!_isGrounded)
            _isGrounded = Physics.CheckSphere(_groundChecker.position, _groundedDistance, _groundMask);

        if (Interact.IsPressed() && _doOnce == true)
        {
            InteractionCheck.CallInteract();
        }
        if (Reset.IsPressed())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Quit.IsPressed())
        {
            Application.Quit();
        }

        // Player Rotation
        if (!_isInDialogue)
            _rb.rotation = Quaternion.Euler(0, _mainCamera.transform.eulerAngles.y, 0);
    }

    /// <summary>
    /// Called to lock or unlock player and camera movement during dialogue
    /// </summary>
    /// <param name="isLocked">Whether the player should move or not</param>
    public void LockCharacter(bool isLocked)
    {
        _isInDialogue = isLocked;

        // Preventing camera from snapping after dialogue
        if (!isLocked)
        {
            _mainCamera.transform.rotation = transform.rotation;
        }

        _mainCamera.gameObject.SetActive(!isLocked);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Interactable")
        {
            InteractionCheck = new PlayerInteractSystem(col.name);
        }
    }
    void OnTriggerExit(Collider col)
    {
        InteractionCheck = new PlayerInteractSystem("Default None");
    }

    private void OnDisable()
    {
        Move.performed -= ctx => _moveDirection = Move.ReadValue<Vector2>();
        Move.performed -= ctx => _isMoving = true;
        Move.canceled -= ctx => _moveDirection = Move.ReadValue<Vector2>();
        Move.canceled -= ctx => _isMoving = false;
        Move.canceled -= ctx => _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
    }
}
