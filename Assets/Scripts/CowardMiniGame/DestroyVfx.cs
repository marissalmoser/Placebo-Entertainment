/*****************************************************************************
// File Name :         DestroyVfx.cs
// Author :            Mark Hanson
// Creation Date :     5/29/2024
//
// Brief Description : Its sole purpose is to destory the tied gameObject at a set time.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyVfx : MonoBehaviour
{
    [SerializeField] private float _secondUntilDead;
    void Awake()
    {
        StartCoroutine(DestroyNow());
    }
    IEnumerator DestroyNow()
    {
        yield return new WaitForSeconds(_secondUntilDead);
        Destroy(gameObject);
    }
}
