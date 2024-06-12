/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Elijah Vroman
*    Date Created: 5/29/24
*    Description: Just a timer class with some helper methods. The time
*    manager script does the creation, running, and deletion of these
*******************************************************************/
using System;
using UnityEngine;

public class Timer
{
    private float _maxTime;
    private float _timeRemaining;
    private bool _isRunning;
    public event Action TimesUp;

    public Timer(int durationInSeconds)
    {
        _maxTime = _timeRemaining = durationInSeconds;
        _isRunning = false;
    }
    public void UpdateTimer(float deltaTime)
    {
        if (_isRunning)
        {
            _timeRemaining -= deltaTime;
            if (_timeRemaining <= 0)
            {
                _timeRemaining = 0;
                _isRunning = false;
                TimesUp?.Invoke();
            }
        }
    }
    public void StartTimer()
    {
        _isRunning = true;
    }
    public void StopTimer()
    {
        _isRunning = false;
    }
    public float GetCurrentTimeInSeconds()
    {
        return _timeRemaining;
    }
    public bool IsRunning()
    {
        return _isRunning;
    }
    public void ResetTimer()
    {
        _timeRemaining = _maxTime;
    }
    public void IncreaseTime(int minutes, int seconds)
    {
        _timeRemaining = Mathf.Clamp(_timeRemaining + (minutes * 60) + seconds, 0, _maxTime);
    }
    public void ReduceTime(int minutes, int seconds)
    {
        _timeRemaining = Mathf.Clamp(_timeRemaining - (minutes * 60) + seconds, 0, _maxTime);
    }
}
