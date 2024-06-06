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

    [Header("Points of Movement")]
    [SerializeField] private float _maxReach;
    [SerializeField]private GameObject _targetFollow;
    [SerializeField] private float _speed;
    public bool PressedE;
    private Vector3 _relocatePoint;

    [Header("Release Windowing")]
    [SerializeField] private GameObject _ReleasePreviewer;
    private bool _hasReleased;
    private bool _doOnce2;
    private bool _doOnce;
    private int _numReleased;

    [SerializeField] private GameObject _gears;

    private PlayerController _pc;

    // Start is called before the first frame update
    void Start()
    {
        PressedE = false;
        _doOnce = true;
        _doOnce2 = true;
        _hasReleased = false;
        _numReleased = 0;
        _relocatePoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        GameObject _pcObject = GameObject.FindWithTag("Player");
        _pc = _pcObject.GetComponent<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {
        _successfulPulls.text = _numReleased.ToString();
        if (_pc.interact.IsPressed() && PressedE == true && transform.position.z > _targetFollow.transform.position.z && _numReleased != 3)
        {
            transform.position -= new Vector3(0, 0, _speed * Time.deltaTime);
            GameObject _preview = GameObject.FindWithTag("Release");
            if (_preview != null)
            {
                _preview.GetComponent<Collider>().enabled = false;
            }
        }
        if(_pc.interact.IsPressed() && PressedE == true && _doOnce2 == true && _numReleased != 3)
        {
            Vector3 _previewerPoint = new Vector3(_relocatePoint.x, _relocatePoint.y, Random.Range(_maxReach,_relocatePoint.z));
            Instantiate(_ReleasePreviewer, _previewerPoint, Quaternion.identity);
            _doOnce2 = false;
        }
        if (PressedE == false && transform.position.z < _relocatePoint.z && transform.position.z != _relocatePoint.z || _pc.interact.IsPressed() && PressedE == true && transform.position.z < _maxReach && transform.position.z != _relocatePoint.z)
        {
            transform.position += new Vector3(0, 0, _speed * Time.deltaTime);
            GameObject _preview = GameObject.FindWithTag("Release");
            if (_preview != null)
            {
                _preview.GetComponent<Collider>().enabled = true;
            }
        }
        if (PressedE == false && transform.position.z == _relocatePoint.z || PressedE == false && transform.position.z > _relocatePoint.z)
        {
            _doOnce = true;
            _doOnce2 = true;
            GameObject _preexistingPV = GameObject.FindWithTag("Release");
            if (_preexistingPV != null)
            {
                GameObject.Destroy(_preexistingPV);
            }
        }
        if(PressedE == false && _doOnce == true)
        {
            GameObject _preexistingPV = GameObject.FindWithTag("Release");
            if (_preexistingPV != null && _preexistingPV.transform.position.z > transform.position.z)
            {
                StartCoroutine(ReleaseWindow());
                _doOnce = false;
            }
        }
        if(_numReleased == 3)
        {
            _gears.SetActive(true);
            _successfulPulls.color = Color.green;
            //Destroy(this);
        }
        //Debug.Log(_numReleased);
    }
    IEnumerator ReleaseWindow()
    {
        _hasReleased = true;
        yield return new WaitForSeconds(0.2f);
        _hasReleased = false;
    }
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Release" && _hasReleased == true)
        {
            _numReleased++;
            GameObject.Destroy(col.gameObject);
        }

        if(col.gameObject.tag == "Player")
        {
            PressedE = true;
            TabbedMenu.Instance.ToggleInteractPrompt(true, "RIPCORD");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            PressedE = false;
            TabbedMenu.Instance.ToggleInteractPrompt(false);
        }
    }
}
