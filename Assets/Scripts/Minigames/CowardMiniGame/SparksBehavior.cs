/*****************************************************************************
// File Name :         SparksBehavior.cs
// Author :            Mark Hanson
// Creation Date :     5/29/2024
//
// Brief Description : Spawns in sparks at a set adjustable amount.
*****************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using Random = UnityEngine.Random;

public class SparksBehavior : MonoBehaviour
{
    [Header("Sparks")]
    [SerializeField] private GameObject _sparks;
    [SerializeField] private float _spawnSpeed;
    [SerializeField] private FMODUnity.EventReference sparkEvent;
    [SerializeField] private FMODUnity.EventReference sparkStopEvent;
    private EventInstance _sparkInstance;
    private bool _isOn;
    private Vector3 _rangePos;
    private GameObject _oneFinder;
    
    void Awake()
    {
        _isOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        _oneFinder = GameObject.FindWithTag("Spark");
        if(_isOn && _oneFinder == null)
        {
            _rangePos = new Vector3(Random.Range(transform.position.x - 2.0f, transform.position.x + 2.0f), transform.position.y - 0.5f, transform.position.z - 0.5f);
            StartCoroutine(SpawnSlowly());
            _isOn = false;
        }
    }

    private void OnDisable()
    {
        if (_sparkInstance.isValid())
        {
            AudioManager.StopSound(_sparkInstance);
            AudioManager.PlaySound(sparkStopEvent, transform.position);
            _sparkInstance = default;
        }
    }

    IEnumerator SpawnSlowly()
    {
        yield return new WaitForSeconds(_spawnSpeed);
        _sparkInstance = AudioManager.PlaySound(sparkEvent, _rangePos);
        Instantiate(_sparks, _rangePos, Quaternion.identity);
        _isOn = true;
    }
}
