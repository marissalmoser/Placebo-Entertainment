/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Elijah Vroman
*    Date Created: 5/29/24
*    Description: Just a timer class with some helper methods. The time
*    manager script does the creation, running, and deletion of these
*******************************************************************/
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Timer
{
    [SerializeField] private float _maxTime;
    [SerializeField] private bool _isRunning;
    public float _timeRemaining { get; private set; }

    //[Header("Events")]
    //public UnityEvent EventToStart;
    //public UnityEvent OnTimesUp;

    public Timer(float maxTime)
    {
        _maxTime = maxTime; 
        _maxTime = _timeRemaining;
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
                //OnTimesUp?.Invoke();
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
