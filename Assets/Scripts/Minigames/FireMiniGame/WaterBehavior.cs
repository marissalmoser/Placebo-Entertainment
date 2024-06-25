/*****************************************************************************
// File Name :         WaterBehavior.cs
// Author :            Mark Hanson
// Creation Date :     6/19/2024
//
// Brief Description : Function for destroying water when its in contact with specific things.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehavior : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private GameObject _cam;

    void Awake()
    {
        _cam = GameObject.FindWithTag("MainCamera");
        _rb.AddForce(_cam.transform.forward * 12f, ForceMode.Impulse);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name != "FishHose" && col.gameObject.tag != "Water")
        {
            Destroy(gameObject);
        }
    }

}
