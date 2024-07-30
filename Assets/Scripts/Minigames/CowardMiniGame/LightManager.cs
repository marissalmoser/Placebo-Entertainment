/*****************************************************************************
// File Name :         LightManager.cs
// Author :            Andrea Swihart-DeCoster
// Creation Date :     5/23/2024
//
// Brief Description : Controls the lights attached the generator.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private float _timeBetweenBlinks;
    [SerializeField] GameObject[] _lights;
    [SerializeField] private Color _idleColor;
    [SerializeField] private Color _correctColor;
    private bool _isFlashing;
    private int _currentLight = 0;

    private void OnEnable()
    {
        RipcordBehavior.OnRipcordScore += OnScore;
        RipcordBehavior.OnRipcordReleaseDetection += DisableFlashingForCurrentLight;
        RipcordBehavior.OnRipcordReleaseDetection += SetLightColor;
    }

    private void OnDisable()
    {
        RipcordBehavior.OnRipcordScore -= OnScore;
        RipcordBehavior.OnRipcordReleaseDetection -= DisableFlashingForCurrentLight;
        RipcordBehavior.OnRipcordReleaseDetection -= SetLightColor;
    }

    /// <summary>
    /// Starts the blinking lights coroutine so the lights blink
    /// </summary>
    public void StartBlinkingLightCoroutine()
    {
        StartCoroutine(BlinkingLights());
    }

    /// <summary>
    /// Makes the lights blink
    /// </summary>
    /// <returns></returns>
    private IEnumerator BlinkingLights()
    {
        while (_currentLight <= _lights.Length - 1)
        {
            _lights[_currentLight].SetActive(true);
            yield return new WaitForSeconds(_timeBetweenBlinks);
            _lights[_currentLight].SetActive(false);
            yield return new WaitForSeconds(_timeBetweenBlinks);
        }
    }

    /// <summary>
    /// Sets a light to permanently on OR off and resumes the blinking state
    /// </summary>
    /// <param name="val"> T if the light is on (no longer flashing) </param>
    private void DisableFlashingForCurrentLight(bool val)
    {
        StopAllCoroutines();
        
        _lights[_currentLight].SetActive(val);
        
        if(!val)
        {
            StartCoroutine(BlinkingLights());
        }
    }

    /// <summary>
    /// Called when the player scores with the ripcord - sets the current light 
    /// to indefinitely on and resumes blinking for the next bulb
    /// </summary>
    private void OnScore()
    {
        DisableFlashingForCurrentLight(true);
        SetLightColor(true);
        _currentLight++;
        //MAKE THIS START WHEN PLAYER GRABS RIPCORD
        //StartCoroutine(BlinkingLights());
    }

    /// <summary>
    /// Sets the current ripcord light to the color based on input parameter. True
    /// for correct and false for idle.
    /// </summary>
    private void SetLightColor(bool input)
    {
        if (input)
        {
            _lights[_currentLight].GetComponent<Light>().color = _correctColor;
        }
        else
        {
            _lights[_currentLight].GetComponent<Light>().color = _idleColor;
        }
    }
}
