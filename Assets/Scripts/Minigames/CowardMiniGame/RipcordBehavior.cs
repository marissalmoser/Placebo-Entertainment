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

    [Header("UI Stuff")]
    [SerializeField] private TextMeshPro _successfulPulls;

    [Header("VFX Stuff")]
    [SerializeField] private ParticleSystem _steam;

    [Header("Points of Movement")]
    [SerializeField] private float _maxReach;
    [SerializeField] private GameObject _targetFollow;
    [SerializeField] private float _speed;
    [SerializeField] bool PressedE;
    private bool _inSwitchPos;
    private bool _switchDoOnce;
    private int _switchMove;
    private Vector3 _relocatePoint;

    [Header("Release Windowing")]
    [SerializeField] private GameObject _ReleasePreviewer;
    private bool _hasReleased;
    private bool _doOnce2;
    private bool _doOnce;
    private int _numReleased;
    private GameObject _preview;



    private bool _gameStarted;

    [Header("Light")]
    private int _currentLight;
    [SerializeField] private GameObject[] _listLights;
    private float _lightSpeed;
    private bool _lightDoOnce;

    [SerializeField] private GameObject _gears;

    private PlayerController _pc;

    // Start is called before the first frame update
    void Start()
    {
        _switchMove = -1;
        PressedE = false;
        _doOnce = true;
        _doOnce2 = true;
        _hasReleased = false;
        _numReleased = 0;
        _relocatePoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        _gameStarted = false;
        GameObject _pcObject = GameObject.FindWithTag("Player");
        _pc = _pcObject.GetComponent<PlayerController>();
        _currentLight = 0;
        _lightDoOnce = true;
        _switchDoOnce = true;
    }
    // Update is called once per frame
    void Update()
    {
        _successfulPulls.text = _numReleased.ToString();
        if(_pc.Interact.IsPressed() && _switchDoOnce == true && _inSwitchPos == true)
        {
            _switchMove *= -1;
            _switchDoOnce = false;
            StartCoroutine(PauseEPress());
        }
        if(_switchMove == -1)
        {
            PressedE = false;
        }
        if (_switchMove == 1)
        {
            PressedE = true;
        }
        if (PressedE == true && transform.position.z > _targetFollow.transform.position.z + 1.7f && _numReleased != 3 && _gameStarted == true)
        {
            transform.position -= new Vector3(0, 0, _speed * Time.deltaTime);
            StartCoroutine(FindPreview());
            if (_preview != null)
            {
                _preview.GetComponent<Collider>().enabled = false;
            }
        }
        if (PressedE == true && _doOnce2 == true && _numReleased != 3 && _gameStarted == true)
        {
            Vector3 _previewerPoint = new Vector3(_relocatePoint.x, _relocatePoint.y, Random.Range(_maxReach, _relocatePoint.z));
            Instantiate(_ReleasePreviewer, _previewerPoint, Quaternion.identity);
            _doOnce2 = false;
        }
        if (PressedE == false && transform.position.z < _relocatePoint.z && transform.position.z != _relocatePoint.z ||PressedE == true && transform.position.z < _maxReach && transform.position.z != _relocatePoint.z)
        {
            transform.position += new Vector3(0, 0, _speed * Time.deltaTime);
            StartCoroutine(FindPreview());
            if (_preview != null)
            {
                _preview.GetComponent<Collider>().enabled = true;
            }
        }
        if (PressedE == false && transform.position.z == _relocatePoint.z || PressedE == false && transform.position.z > _relocatePoint.z)
        {
            _doOnce = true;
            _doOnce2 = true;
            StartCoroutine(FindPreview());
            if (_preview != null)
            {
                GameObject.Destroy(_preview);
            }
        }
        if (PressedE == false && _doOnce == true)
        {
            StartCoroutine(FindPreview());
            if (_preview != null && _preview.transform.position.z > transform.position.z)
            {
                StartCoroutine(ReleaseWindow());
                _doOnce = false;
            }
        }
        if (_numReleased == 0 && _gameStarted == true && _lightDoOnce == true)
        {
            _currentLight = 0;
            StartCoroutine(blinkingLight());
            _lightDoOnce = false;
        }
        if (_numReleased == 1 && _gameStarted == true && _lightDoOnce == true)
        {
            _listLights[0].SetActive(true);
            _currentLight = 1;
            StartCoroutine(blinkingLight());
            _lightDoOnce = false;
        }
        if(_numReleased == 2 && _gameStarted == true && _lightDoOnce == true)
        {
            _listLights[1].SetActive(true);
            _currentLight = 2;
            StartCoroutine(blinkingLight());
            _lightDoOnce = false;
        }
        if (_numReleased == 3)
        {
            _listLights[2].SetActive(true);
            StopCoroutine(blinkingLight());
            _gears.SetActive(true);
            _successfulPulls.color = Color.green;
            _steam.Stop();
            //Destroy(this);
        }
        //Debug.Log(_numReleased);

    }
    IEnumerator PauseEPress()
    {
        yield return new WaitForSeconds(0.4f);
        _switchDoOnce = true;
    }
    IEnumerator blinkingLight()
    {
        _listLights[_currentLight].SetActive(true);
        yield return new WaitForSeconds(_lightSpeed);
        _listLights[_currentLight].SetActive(false);
        _lightDoOnce = true;
    }
    IEnumerator ReleaseWindow()
    {
        _hasReleased = true;
        yield return new WaitForSeconds(0.2f);
        _hasReleased = false;
    }
    IEnumerator FindPreview()
    {
        _preview = GameObject.FindWithTag("Release");
        yield return _preview;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Release" && _hasReleased == true)
        {
            _numReleased++;
            GameObject.Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "Player")
        {
            _inSwitchPos = true;
            TabbedMenu.Instance.ToggleInteractPrompt(true, "GRAB");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            _inSwitchPos = false;
            _switchMove = -1;
            TabbedMenu.Instance.ToggleInteractPrompt(false);
        }
    }
    public void GameStart()
    {
        _gameStarted = true;
    }
}
