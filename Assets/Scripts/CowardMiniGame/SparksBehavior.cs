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
    

    void Awake()
    {
        _isOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isOn)
        {
            _rangePos = new Vector3(Random.Range(2f, 6f), 1.5f, 4f);
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
