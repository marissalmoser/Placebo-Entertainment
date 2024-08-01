/*****************************************************************************
// File Name :         RipcordBehavior.cs
// Author :            Mark Hanson
// Contributors :      Marissa Moser, Andrea Swihart-DeCoster
// Creation Date :     5/23/2024
//
// Brief Description : Any function to do for the ripcord mini game can be found hear from pulling it back, pulling towards certain ranges, and counting successful pulls.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlaceboEntertainment.UI;
using UnityEngine.InputSystem;
using System;

public class RipcordBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _gears;

    [Header("Ripcord Stats")]
    [SerializeField] private float _followPlayerSpeed;
    [SerializeField] private float _returnSpeed;
    [SerializeField] private Transform _maxPlayerDetectionPosition;


    [Header("Goal/Scoring Information")]
    [SerializeField] private GameObject _scoringRangeObject;
    [SerializeField] private Transform _maxGoalSpawnPosition;
    [SerializeField] private Transform _minGoalSpawnPosition;

    [Header("UI")]
    [SerializeField] private TextMeshPro _successfulPulls;
    [SerializeField] private String _interactPrompt;

    [Header("VFX")]
    [SerializeField] private ParticleSystem _steam;

    private PlayerController _playerController;
    private GameObject _player;

    private Vector3 _startPosition;
    private int _score;
    private GameObject _scoreDetectionRange;

    //bitmask to hit later 8 (player)
    int layerMask = 1 << 8;

    private bool _isFollowingPlayer;
    private bool _lightIsBlinking;
    private bool _canScore;
    private bool _isMinigameActive;
    private bool _isAtStartPosition;

    public static Action OnRipcordScore;
    public static Action<bool> OnRipcordReleaseDetection;

    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _player = _playerController.gameObject;
        _isFollowingPlayer = false;
        _canScore = false;
        _isAtStartPosition = true;
        _score = 0;
        _startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        // Cancels the ripcord following if the player is out of the detection range
        if (_isFollowingPlayer && Physics.OverlapCapsule(transform.position, _maxPlayerDetectionPosition.position, 1f, layerMask).Length <= 0)
        {
            StartCoroutine(ReturnToStartPosition());
        }
    }

    /// <summary>
    /// Activates the minigame
    /// </summary>
    public void ActivateMinigame()
    {
        _isMinigameActive = true;
    }

    /// <summary>
    /// Deactivates the minigame
    /// </summary>
    private void DeactivateMinigame()
    {
        _isMinigameActive = false;
        TabbedMenu.Instance.ToggleInteractPrompt(false);
    }

    /// <summary>
    /// When the player presses W within range of the ripcord
    /// </summary>
    /// <param name="obj"></param>
    private void OnInteract(InputAction.CallbackContext obj)
    {
        if (_isAtStartPosition && _score < 3)
        {
            StartCoroutine(FollowPlayer());
            SpawnTargetObject();
        }
        else if (!_isAtStartPosition)
        {
            StartCoroutine(ReturnToStartPosition());
        }
    }

    /// <summary>
    /// Ripcord follows the players z position backwards
    /// </summary>
    /// <returns></returns>
    private IEnumerator FollowPlayer()
    {
        // Disable blinking when it starts following the player so it can reset properly
        OnRipcordReleaseDetection?.Invoke(false);

        _isFollowingPlayer = true;
        _isAtStartPosition = false;
       
        while (_isFollowingPlayer)
        {
            if (_player.transform.position.z + 1 < transform.position.z)
            {
                // Chose to lerp the z value to prevent visual tearing.
                transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(transform.position.z, _player.transform.position.z + 1f, Time.fixedDeltaTime * _followPlayerSpeed));
            }

            // Waits for fixed update to align with the players movement update.
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Spawns the goal object that represents the range where the player can letgo to score
    /// </summary>
    private void SpawnTargetObject()
    {
        Vector3 _goalPosition = new Vector3(_startPosition.x, _startPosition.y, UnityEngine.Random.Range(_minGoalSpawnPosition.position.z, _maxGoalSpawnPosition.position.z));
        _scoreDetectionRange = Instantiate(_scoringRangeObject, _goalPosition, Quaternion.identity);
    }

    /// <summary>
    /// Ripcord returns to the wall
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReturnToStartPosition()
    {
        // Disable blinking when it returns to start so it can reset properly
        OnRipcordReleaseDetection?.Invoke(false);

        _isFollowingPlayer = false;
        _scoreDetectionRange.GetComponent<Collider>().enabled = true;

        while (!_isAtStartPosition)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(transform.position.z, _startPosition.z, Time.fixedDeltaTime * _returnSpeed));

            // Small margin of error to determine if the ripcord is at start
            if (transform.position.z >= (_startPosition.z - 0.1f))
            {
                _isAtStartPosition = true;
            }

            yield return new WaitForFixedUpdate();
        }

        // Make sure the score detection range is destroyed once the ripcord returns to the start position
        if(_scoreDetectionRange != null)
        {
            Destroy(_scoreDetectionRange);
        }
    }

    /// <summary>
    /// When the player successfully scores
    /// </summary>
    private void OnScore()
    {
        if (_canScore)
        {
            _score++;

            OnRipcordScore?.Invoke();
            _successfulPulls.text = (_score).ToString();
        }

        // Winning score
        if (_score >= 3)
        {
            _gears.SetActive(true);
            _successfulPulls.color = Color.green;
            StopGeneratorSteam();

            DeactivateMinigame();
        }
    }

    /// <summary>
    /// Stops the steam coming from the ripcord.
    /// </summary>
    public void StopGeneratorSteam()
    {
        _steam.Stop();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (_isMinigameActive)
        {
            // Enable player interaction
            if (collider.gameObject.tag == "Player")
            {
                _playerController.Interact.started += OnInteract;
                TabbedMenu.Instance.ToggleInteractPrompt(true, _interactPrompt);
            }

            // Enable scoring when in score-range of the goal when following the player
            if (collider.gameObject.tag != "Release")
            {
                return;
            }

            if (_isFollowingPlayer)
            {
                OnRipcordReleaseDetection?.Invoke(true);
                _canScore = true;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (_isMinigameActive)
        {
            // Disables player interaction & should stop following player if they leave
            if (collider.gameObject.tag == "Player")
            {
                _playerController.Interact.started -= OnInteract;
                TabbedMenu.Instance.ToggleInteractPrompt(false);
            }

            // Disable scoring if scoring range is exited when following player, otherwise, score points.
            if (collider.gameObject.tag != "Release")
            {
                return;
            }
            if (_isFollowingPlayer)
            {
                _canScore = false;
                OnRipcordReleaseDetection?.Invoke(false);
            }
            else
            {
                OnScore();
            }
        }
    }
}
