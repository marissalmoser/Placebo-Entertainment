using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private float _blinkingSpeed;
    [SerializeField] GameObject[] _lights;
    private bool _isFlashing;
    private int _currentLight = 0;

    private void OnEnable()
    {
        RipcordBehavior.OnRipcordScore += OnScore;
        RipcordBehavior.OnPlayerDetectionChange += SetIsFlashing;
    }

    private void OnDisable()
    {
        RipcordBehavior.OnRipcordScore -= OnScore;
        RipcordBehavior.OnPlayerDetectionChange -= SetIsFlashing;
    }

    public void StartBlinkingLightCoroutine()
    {
        StartCoroutine(BlinkingLights());
    }

    private IEnumerator BlinkingLights()
    {
        while (_currentLight <= _lights.Length - 1)
        {
            _lights[_currentLight].SetActive(true);
            yield return new WaitForSeconds(_blinkingSpeed);
            _lights[_currentLight].SetActive(false);
            yield return new WaitForSeconds(_blinkingSpeed);
        }
    }

    private void SetIsFlashing(bool val)
    {
        StopAllCoroutines();
        
            _lights[_currentLight].SetActive(val);
        
        if(!val)
        {
            StartCoroutine(BlinkingLights());
        }
    }

    private void OnScore()
    {
        _lights[_currentLight].SetActive(true);
        _currentLight++;
        StartCoroutine(BlinkingLights());
    }
}
