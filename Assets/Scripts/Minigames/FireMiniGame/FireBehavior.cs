/*****************************************************************************
// File Name :         FireBehavior.cs
// Author :            Mark Hanson
// Creation Date :     6/19/2024
//
// Brief Description : Function for destroying fire when it reaches 0 by water touching it
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FireBehavior : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _minFireEndure;
    [SerializeField] private int _maxFireEndure;
    [SerializeField] private int _curFireEndure;

    [Header("Connectors")]
    [SerializeField] private TextMeshPro _fireNum;

    void Awake()
    {
        _curFireEndure = Random.Range(_minFireEndure, _maxFireEndure);
    }
    // Update is called once per frame
    void Update()
    {
        _fireNum.text = _curFireEndure.ToString();
        if (_curFireEndure <= 0)
        {
            Destroy(gameObject);
        }
        if (_curFireEndure >= 15 && _curFireEndure <= 100)
        {
            transform.localScale = new Vector3(_curFireEndure / 30f, _curFireEndure / 30f, _curFireEndure / 30f);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Water")
        {
            _curFireEndure--;
        }
    }
}
