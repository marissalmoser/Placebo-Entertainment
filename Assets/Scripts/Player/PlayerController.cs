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

    //[SerializeField] float cutCameraDamping;
    //float defaultDamping;
    //[SerializeField] float cutCameraFOV;
   // float defaultFOV;

    public PlayerControls PlayerControls { get; private set; }
    public InputAction move, interact, reset, quit;

    Rigidbody rb;
    CinemachineVirtualCamera mainCamera;
    CinemachineTransposer transposer;
    //[SerializeField] GameObject laser;

    PlayerInteractSystem _InteractionCheck;
    private bool _doOnce;

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
        //defaultDamping = transposer.m_XDamping;
        //defaultFOV = mainCamera.m_Lens.FieldOfView;
        //laser = mainCamera.transform.GetChild(0).gameObject;

        PlayerControls = new PlayerControls();
        PlayerControls.BasicControls.Enable();

        _InteractionCheck = new PlayerInteractSystem("Default None");
        _doOnce = true;


        move = PlayerControls.FindAction("Move");
        interact = PlayerControls.FindAction("Interact");
        reset = PlayerControls.FindAction("Reset");
        quit = PlayerControls.FindAction("Quit");

        move.performed += ctx => moveDirection = move.ReadValue<Vector2>();
        move.performed += ctx => isMoving = true;
        move.canceled += ctx => moveDirection = move.ReadValue<Vector2>();
        move.canceled += ctx => isMoving = false;
       // move.performed += ctx => rb.velocity = 
        //new Vector3(move.ReadValue<Vector2>().x * moveSpeed, rb.velocity.y, move.ReadValue<Vector2>().y * moveSpeed);
       // move.canceled += ctx => rb.velocity = new Vector3(0, rb.velocity.y, 0);

        //slash.performed += ctx => laser.SetActive(true);
        //slash.performed += ctx => transposer.m_XDamping = transposer.m_YDamping = transposer.m_ZDamping = cutCameraDamping;
        //slash.performed += ctx => cameraLens.FieldOfView = cutCameraFOV;
        //slash.performed += ctx => StartZoom(true);

       // slash.canceled += ctx => laser.SetActive(false);
        //slash.canceled += ctx => transposer.m_XDamping = transposer.m_YDamping = transposer.m_ZDamping = defaultDamping;
        //slash.canceled += ctx => cameraLens.FieldOfView = defaultFOV;
       // slash.canceled += ctx => StartZoom(false);
    }
    /// <summary>
    /// Initiates a camera zoom when inputs are recieved
    /// </summary>
    /// <param name="zoomingIn">Determines which coroutine to start</param>
    //private void StartZoom(bool zoomingIn)
    //{
    //    StopAllCoroutines();

    //    if (zoomingIn)
    //        StartCoroutine(ZoomIn());
    //    else
    //        StartCoroutine(ZoomOut());
    //}

    /// <summary>
    /// Zooms the camera from defaultFOV to cutCameraFOV
    /// </summary>
    //IEnumerator ZoomIn()
    //{
    //    float interpolationVal = 0;

    //    while (true)
    //    {
    //        yield return new WaitForSeconds(0.01f);

    //        interpolationVal += 0.05f;

    //        mainCamera.m_Lens.FieldOfView = Mathf.Lerp(defaultFOV, cutCameraFOV, interpolationVal);

    //        if (interpolationVal >= 1f && mainCamera.m_Lens.FieldOfView >= defaultFOV)
    //            break;
    //    }

    //    mainCamera.m_Lens.FieldOfView = defaultFOV;
    //}

    /// <summary>
    /// Zooms the camera from cutCameraFOV back to defaultFOV
    /// </summary>
    //IEnumerator ZoomOut()
    //{
    //    float interpolationVal = 0;

    //    while (true)
    //    {
    //        yield return new WaitForSeconds(0.01f);

    //        interpolationVal += 0.05f;

    //        mainCamera.m_Lens.FieldOfView = Mathf.Lerp(cutCameraFOV, defaultFOV, interpolationVal);

    //        if (interpolationVal >= 1f && mainCamera.m_Lens.FieldOfView <= cutCameraFOV)
    //            break;
    //    }

    //    mainCamera.m_Lens.FieldOfView = cutCameraFOV;
    //}

    void FixedUpdate()
    {
        // Player Movement
        if (isMoving == true && _isKinemat == true)
        {
            velocity = transform.right * moveDirection.x + transform.forward * moveDirection.y;
            velocity = velocity.normalized;
            velocity *= moveSpeed;
             Vector3 _velocity = new Vector3(velocity.x, 0, velocity.z);
            rb.MovePosition(transform.position + _velocity * Time.deltaTime);
            rb.AddForce(velocity * moveSpeed);
        }
        if(_isKinemat == true)
        {
            rb.isKinematic = true;
        }
        if(_isKinemat == false)
        {
            rb.isKinematic = false;
        }
        if(isMoving == true && _isKinemat == false)
        {
            velocity = transform.right * moveDirection.x + transform.forward * moveDirection.y;
            velocity = velocity.normalized;
            velocity *= moveSpeed;
            Vector3 _velocity = new Vector3(velocity.x, 0, velocity.z);
            rb.MovePosition(transform.position + _velocity * Time.deltaTime);
            rb.AddForce(velocity);
        }

//float speed = rb.velocity.magnitude;
//if (speed > 5)
//{
//    float brakeSpeed = speed - 5;
//    Vector3 brakingVelocity = rb.velocity.normalized * brakeSpeed;
//    rb.AddForce(-brakingVelocity);
//}

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
        rb.rotation = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0);
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
