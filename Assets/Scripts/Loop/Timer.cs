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
    private float maxTime;
    private float timeRemaining;
    private bool isRunning;
    public event Action TimesUp;

    public Timer(int durationInSeconds)
    {
        maxTime = timeRemaining = durationInSeconds;
        this.isRunning = false;
    }
    public void UpdateTimer(float deltaTime)
    {
        if (isRunning)
        {
            timeRemaining -= deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isRunning = false;
                TimesUp?.Invoke();
            }
        }
    }
    public void StartTimer()
    {
        isRunning = true;
    }
    public void StopTimer()
    {
        isRunning = false;
    }
    public float GetCurrentTimeInSeconds()
    {
        return timeRemaining;
    }
    public bool IsRunning()
    {
        return isRunning;
    }
    public void ResetTimer()
    {
        timeRemaining = maxTime;
    }
    public void IncreaseTime(int minutes, int seconds)
    {
        timeRemaining = Mathf.Clamp(timeRemaining + (minutes * 60) + seconds, 0, maxTime);
    }
    public void ReduceTime(int minutes, int seconds)
    {
        timeRemaining = Mathf.Clamp(timeRemaining - (minutes * 60) + seconds, 0, maxTime);
    }
}
