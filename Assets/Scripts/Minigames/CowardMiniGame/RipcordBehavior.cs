/*****************************************************************************
// File Name :         RipcordBehavior.cs
// Author :            Mark Hanson
// Contributors :      Marissa Moser
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

public class RipcordBehavior : MonoBehaviour
{
    [SerializeField] private float _returnSpeed;
    [SerializeField] private GameObject _gears;

    [Header("Scoring Information")]
    [SerializeField] private GameObject _goalObject;
    [SerializeField] private float _distanceFromGoalToScore;

    [Header("UI")]
    [SerializeField] private TextMeshPro _successfulPulls;

    [Header("VFX")]
    [SerializeField] private ParticleSystem _steam;

    [Header("Lights")]
    [SerializeField] private float _lightBlinkSpeed;
    [SerializeField] private GameObject[] _lights;

    private PlayerController _playerController;
    private GameObject _player;
    private bool _isFollowingPlayer;
    private bool _lightIsBlinking;
    private bool _canScore;
    private bool _isMinigameActive;

    private Vector3 _startPosition;

    private bool _isAtStartPosition;
    private int _score;
    private GameObject _goal;

    // Start is called before the first frame update
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

    public void ActivateMinigame()
    {
        _isMinigameActive = true;
        StartCoroutine(BlinkingLight());
    }

    private void DeactivateMinigame()
    {
        _isMinigameActive = false;
    }

    private void OnInteract()
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

    private IEnumerator FollowPlayer()
    {
        _isFollowingPlayer = true;
        _isAtStartPosition = false;

        while (_isFollowingPlayer)
        {
            transform.position =
                    new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(transform.position.z, _player.transform.position.z + 1f, Time.fixedDeltaTime * 1.25f));

            yield return new WaitForFixedUpdate();
        }
    }

    private void SpawnTargetObject()
    {
        Vector3 _goalPosition = new Vector3(_startPosition.x, _startPosition.y, Random.Range(_distanceFromGoalToScore, _startPosition.z));
        _goal = Instantiate(_goalObject, _goalPosition, Quaternion.identity);
        _goal.GetComponent<Collider>().enabled = false;
    }

    IEnumerator BlinkingLight()
    {
        while (_isMinigameActive)
        {
           /* if (_goal != null && transform.position.z > _goal.transform.position.z + _distanceFromGoalToScore || transform.position.z < _goal.transform.position.z - _distanceFromGoalToScore)
            {
                _canScore = true;
                _lights[_score].SetActive(true);
            }*/

            // If the ripcord is within the scoring distance threshhold, disable the blinking lights
            if (transform.position.z < _goal.transform.position.z + _distanceFromGoalToScore || transform.position.z > _goal.transform.position.z - _distanceFromGoalToScore)
            {
                _canScore = false;
                _lights[_score].SetActive(true);
                yield return new WaitForSeconds(_lightBlinkSpeed);
                _lights[_score].SetActive(false);
                yield return new WaitForSeconds(_lightBlinkSpeed);
            }
        }
    }

    private IEnumerator ReturnToStartPosition()
    {
        _isFollowingPlayer = false;
        _goal.GetComponent<Collider>().enabled = true;
        _canScore = true;

        while (!_isAtStartPosition)
        {
            transform.position =
                         new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(transform.position.z, _startPosition.z, Time.fixedDeltaTime * _returnSpeed));

            if (transform.position.z >= (_startPosition.z - 0.1f))
            {
                _isAtStartPosition = true;
            }

            yield return new WaitForFixedUpdate();
        }

        if(_goal != null)
        {
            Destroy(_goal);
        }
    }

    private void OnScore()
    {
        _lights[_score].SetActive(true);
        _score++;
        _successfulPulls.text = (_score).ToString();

        // Winning score
        if (_score >= 3)
        {
            _gears.SetActive(true);
            _successfulPulls.color = Color.green;
            StopRipcordSteam();

            DeactivateMinigame();
        }
    }

    /// <summary>
    /// Stops the steam coming from the ripcord.
    /// </summary>
    public void StopRipcordSteam()
    {
        _steam.Stop();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (_isMinigameActive)
        {
            if (collider.gameObject.tag == "Release" && _canScore)
            {
                OnScore();
                Destroy(collider.gameObject);
            }

            if (collider.gameObject.tag == "Player")
            {
                _playerController.Interact.started += ctx => OnInteract();
                TabbedMenu.Instance.ToggleInteractPrompt(true, "GRAB");
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (_isMinigameActive)
        {
            if (collider.gameObject.tag == "Player")
            {
                _playerController.Interact.started -= ctx => OnInteract();
                TabbedMenu.Instance.ToggleInteractPrompt(false);
            }
        }
    }
}
