/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Elijah Vroman
*    Date Created: 5/30/24 (Yes, very late...) 
*    Description: Help ive spent far too long making a flexible timer
*    manager ahhhhhhhhhh
*    Anyways, this things main idea it to have a list of Timer classes
*    that you can affect if you have a bunch of timers running around. 
*    If programmers want to make a timer, instead of making their own 
*    they can just use the methods from this script 
*    The timer class has an "update" method but thats just what this calls every
*    update. All the bs in update here is because the way the stacks
*    were running cause of the event it would try to delete a timer 
*    while the collection was still being iterated upon. Because we do need
*    an event specific to the timer, i had to look for solutions.
*******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct TimerStruct
{
    public string timerName;
    public Timer timer;

    public TimerStruct(string name, float maxTime, NpcEvent eventTimerCalls, NpcEventTags npcToAlert)
    {
        timerName = name;
        timer = new Timer(maxTime, eventTimerCalls, npcToAlert);
    }
}

public class TimerManager : MonoBehaviour
{
    public List<TimerStruct> _timers = new List<TimerStruct>();
    #region Instance
    //regions are cool, i guess. Just hiding boring stuff
    public static TimerManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion
    public void Start()
    {
        foreach (TimerStruct timer in _timers)
        {
            timer.timer.IncreaseTime(0, timer.timer._maxTime);
        }
    }
    public void Update()
    {
        if (_timers.Count > 0)
        {
            //So i found a page on stack that said make it .ToList bc then the
            //compiler makes a copy of the list and iterates over that instead
            //Apparently effeciency of this "isn't great" (surprise suprise)
            //so im going to look into solutions, but as it is ive spent wayyy
            //too long on timers so im just going to leave the script as is
            foreach (TimerStruct timerStruct in _timers.ToList())
            {
                if (timerStruct.timer.IsRunning())
                {
                    timerStruct.timer.UpdateTimer(Time.deltaTime);
                }
                if (timerStruct.timer.GetCurrentTimeInSeconds() <= 0)
                {
                    _timers.Remove(timerStruct);
                }
            }
        }
    }

    /// <summary>
    /// This is used in the inspector to allow Nick G's NPC event system to 
    /// alert the timer list that a timer with this name should start counting.
    /// Specifically, this is used by the EventListener script.
    /// </summary>
    /// <param name="timerName"></param>
    public void StartTimerWithName(string timerName)
    {
        //By adding ? to the struct i can make it nullable, which makes for an 
        //easy check to use in conjunction with searching the list. Ive seen 
        //videos use this and figured id try it out. - Eli
        TimerStruct? timerStruct = _timers.Find(thatTimer => thatTimer.timerName == timerName);
        if (timerStruct.HasValue)
        {
            timerStruct.Value.timer.StartTimer();
        }
    }
    public Timer CreateTimer(string timerName, float maxTime, NpcEvent eventTimerCalls, NpcEventTags npcToAlert)
    {
        if (_timers.Exists(t => t.timerName == timerName))
        {
            print("Timer " + timerName + " already exists.");
            return null;
        }

        TimerStruct newTimerStruct = new TimerStruct(timerName, maxTime, eventTimerCalls, npcToAlert);
        newTimerStruct.timer.StartTimer();
        _timers.Add(newTimerStruct);
        return newTimerStruct.timer;
    }
    public Timer GetTimer(string timerName)
    {
        TimerStruct timerStruct = _timers.Find(thatTimer => thatTimer.timerName == timerName);
        if (timerStruct.timer != null)
        {
            return timerStruct.timer;
        }
        print("Timer " + timerName + " does not exist.");
        return null;
    }

    public Timer RemoveTimer(string timerName)
    {
        TimerStruct timerStruct = _timers.Find(t => t.timerName == timerName);
        if (timerStruct.timer != null)
        {
            _timers.Remove(timerStruct);
            return timerStruct.timer;
        }
        print("Timer " + timerName + " does not exist.");
        return null;
    }
}
