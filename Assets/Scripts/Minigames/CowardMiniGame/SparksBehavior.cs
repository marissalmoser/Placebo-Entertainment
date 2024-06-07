/*****************************************************************************
// File Name :         SparksBehavior.cs
// Author :            Mark Hanson
// Creation Date :     5/29/2024
//
// Brief Description : Spawns in sparks at a set adjustable amount.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparksBehavior : MonoBehaviour
{
    [Header("Sparks")]
    [SerializeField] private GameObject _sparks;
    [SerializeField] private float _spawnSpeed;
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
            _rangePos = new Vector3(Random.Range(transform.position.x - 2.0f, transform.position.x + 2.0f), transform.position.y, transform.position.z);
            StartCoroutine(SpawnSlowly());
            _isOn = false;
        }
    }
    IEnumerator SpawnSlowly()
    {
        yield return new WaitForSeconds(_spawnSpeed);
        Instantiate(_sparks, _rangePos, Quaternion.identity);
        _isOn = true;
    }
}
