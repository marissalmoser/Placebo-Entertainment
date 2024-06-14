/*****************************************************************************
// File Name :         PlayerController.cs
// Author :            Nick Grinsteasd & Mark Hanson
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
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;


    public PlayerControls PlayerControls { get; private set; }
    public InputAction move, interact, reset, quit;

    Rigidbody rb;
    CinemachineVirtualCamera mainCamera;
    CinemachineTransposer transposer;

    PlayerInteractSystem _InteractionCheck;
    private bool _doOnce;
    private bool _isInDialogue = false;

    [SerializeField] bool _isKinemat;

    bool isMoving = false;
    Vector2 moveDirection;
    Vector3 velocity;

    bool isGrounded = true;
    float groundDistance = 0.3f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundChecker;

    void Awake()
    {
        Instance = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = gameObject.GetComponent<Rigidbody>();
        mainCamera = FindObjectOfType<CinemachineVirtualCamera>();
        transposer = mainCamera.GetCinemachineComponent<CinemachineTransposer>();

        PlayerControls = new PlayerControls();
        PlayerControls.BasicControls.Enable();

        _InteractionCheck = new PlayerInteractSystem("Default None");
        _doOnce = true;

        mainCamera.transform.rotation = transform.rotation;

        move = PlayerControls.FindAction("Move");
        interact = PlayerControls.FindAction("Interact");
        reset = PlayerControls.FindAction("Reset");
        quit = PlayerControls.FindAction("Quit");

        move.performed += ctx => moveDirection = move.ReadValue<Vector2>();
        move.performed += ctx => isMoving = true;
        move.canceled += ctx => moveDirection = move.ReadValue<Vector2>();
        move.canceled += ctx => isMoving = false;
    }
    void FixedUpdate()
    {
        // Player Movement
        if (!_isInDialogue && isMoving && _isKinemat)
        {
            velocity = transform.right * moveDirection.x + transform.forward * moveDirection.y;
            velocity = velocity.normalized;
            velocity *= moveSpeed;
             Vector3 _velocity = new Vector3(velocity.x, 0, velocity.z);
            rb.MovePosition(transform.position + _velocity * Time.deltaTime);
            rb.AddForce(velocity * moveSpeed);
        }
        if(_isKinemat)
        {
            rb.isKinematic = true;
        }
        if(_isKinemat == false)
        {
            rb.isKinematic = false;
        }
        if(!_isInDialogue && isMoving && _isKinemat == false)
        {
            velocity = transform.right * moveDirection.x + transform.forward * moveDirection.y;
            velocity = velocity.normalized;
            velocity *= moveSpeed;
            Vector3 _velocity = new Vector3(velocity.x, 0, velocity.z);
            rb.MovePosition(transform.position + _velocity * Time.deltaTime);
            rb.AddForce(velocity);
        }

        // Ground Check
        if (!isGrounded)
            isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);

        if (interact.IsPressed() && _doOnce == true)
        {
            _InteractionCheck.CallInteract();
        }
        if (reset.IsPressed())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (quit.IsPressed())
        {
            Application.Quit();
        }

        // Player Rotation
        if (!_isInDialogue)
            rb.rotation = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0);
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
            mainCamera.transform.rotation = transform.rotation;
        }

        mainCamera.gameObject.SetActive(!isLocked);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Interactable")
        {
            _InteractionCheck = new PlayerInteractSystem(col.name);
        }
    }
    void OnTriggerExit(Collider col)
    {
        _InteractionCheck = new PlayerInteractSystem("Default None");
    }
}
