    /*****************************************************************************
// File Name :         FireBehavior.cs
// Author :            Mark Hanson
// Contributors:       Andrea Swihart-DeCoster
// Creation Date :     6/19/2024
//
// Brief Description : Function for destroying fire when it reaches 0 by water touching it
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.ComponentModel;

public class FireBehavior : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _waterCooldown;
    [SerializeField] private float _timeBetweenScaleChanges;

    [Range(0f,50f)]
    [Tooltip("How much the fire changes size each time it grows or shrinks")]
    [SerializeField] private float _absoluteScaleChange;

    [Range(0f, 2f)]
    [Tooltip("Minimum size the fire can start at.")]
    [SerializeField] private float _minFireSize;
    [Range(0f, 2f)]
    [SerializeField] private float _maxFireSize;

    // _scaleChange starts at _absoluteScaleChange then changes to pos or neg situationally
    private float _scaleChange;

    private void Start()
    {
        float startingSize = Random.Range(_minFireSize, _maxFireSize);
        transform.localScale = new Vector3(startingSize, startingSize, startingSize);
        _scaleChange = _absoluteScaleChange;
        StartCoroutine(ChangeFireScale());
    }
    /* private void OnParticleCollision(GameObject other)
     {
         if (other.TryGetComponent<WaterBehavior>(out WaterBehavior _waterBehavior))
         {
             _scaleChange = -_absoluteScaleChange;
             StopCoroutine(WaterCooldown());
         }
     }*/

    private void OnParticleTrigger()
    {
        print("test");
        _scaleChange = -_absoluteScaleChange;
        StopCoroutine(WaterCooldown());
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<WaterBehavior>(out WaterBehavior _waterBehavior))
        {
            StartCoroutine(WaterCooldown());
        }
        
    }

    /// <summary>
    /// After the cooldown period, the fire should start growing
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaterCooldown()
    {
        _scaleChange = 0f;
        yield return new WaitForSeconds(_waterCooldown);
        _scaleChange = _absoluteScaleChange;
    }

    /// <summary>
    /// Changes the scale of the fire.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeFireScale()
    {
        // Stops once size is 0
        while (transform.localScale.x > 0)
        {
            // Shouldn't grow if bigger than maxFireSize
            if (CanFireGrow() || _scaleChange < 0)
            {
                // Changes fire scale
                float newScale = transform.localScale.x + _scaleChange;
                transform.localScale = new Vector3(newScale, newScale, newScale);

                if (transform.localScale.x <= 0)
                {
                    Destroy(gameObject);
                }
            }

            yield return new WaitForSeconds(_timeBetweenScaleChanges);
        }
    }

    private bool CanFireGrow()
    {
        return _scaleChange > 0 && transform.localScale.x <= _maxFireSize;
    }
}
