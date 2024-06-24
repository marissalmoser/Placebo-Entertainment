/******************************************************************
*    Author: Marissa Moser
*    Contributors: 
*    Date Created: June 19, 2024
*    Description: This script is the manager for the Angel Minigame. It keeps track
*    of starting and ending the game, the state of each station, switching between
*    stations, as well as counting the rounds and stations completed.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.VisualScripting;

[System.Serializable]
public struct Station
{
    public GameObject StationScreen;
    public ScreenBehavior ScreenBehavior;
    public GameObject StationConsole;
    public StationBehavior StationBehavior;
}

public class AngelMinigameManager : MonoBehaviour
{
    [SerializeField] private List<Station> _stations;
    [SerializeField] int _countDownTime;
    private int _currentTime;

    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private NpcEvent _endMinigameEvent;
    [SerializeField] private NpcEventTags _endMinigameEventTag;

    //this will keep track of which station is active. -1 is pre-minigame, then
    //stations 1-4 are 0-3 (matched list of structs in inspector
    private int _stationCount;

    public static Action AngelStationComplete, CheckState, TriggerFail, TriggerStart;
    private int _round;

    /// <summary>
    /// Assigns values for play
    /// </summary>
    private void Start()
    {
        AngelStationComplete += SwitchStation;
        CheckState += CheckStates;
        TriggerFail += StartStation;

        //find the scripts on all the screens to ref from the struct
        for (int i = 0; i < _stations.Count; i++)
        {
            var structCopy = _stations[i];
            structCopy.ScreenBehavior = _stations[i].StationScreen.GetComponent<ScreenBehavior>();
            structCopy.StationBehavior = _stations[i].StationConsole.GetComponent<StationBehavior>();
            _stations[i] = structCopy;
        }
    }

    /// <summary>
    /// This function is called from the confirm button to check the state of the
    /// interactables at each station as the game progresses. It keeps track of which 
    /// round the player is on as well as which station.
    /// </summary>
    private void CheckStates()
    {
        bool state = _stations[_stationCount].StationBehavior.CheckStates();

        if (state)
        {
            _round++;
            //checks how many rounds are left
            if(_round >= 3)
            {
                //checks how many stations are left. This is based on how many stations
                //are in the array, so if some ar ecut this will still work.
                if(_stationCount < _stations.Count - 1)
                {
                    _round = 0;
                    SwitchStation();
                }
                else
                {
                    StopMinigame();
                }
            }
            else
            {
                StartStation();
                print("correct");
            }
        }
        else
        {
            StartStation();
            print("wrong");
        }
    }

    /// <summary>
    /// This function will contain the functionality to switch stations once one is
    /// complete. It then starts the next station.
    /// </summary>
    private void SwitchStation()
    {
        //print("switch to next station");
        if (_stationCount < _stations.Count && _stationCount >= 0)
        {
            _stations[_stationCount].StationScreen.SetActive(false);
        }
        _stationCount++;

        StopAllCoroutines();
        _timerText.text = "0:00";
        if (_stationCount < _stations.Count)
        {
            _stations[_stationCount].StationScreen.SetActive(true);
        }

        _stations[_stationCount].StationBehavior.MakeStationConfirmable();
        StartStation();
    }

    /// <summary>
    /// This function will set the station and screen with the correct information
    /// based on the station count.
    /// </summary>
    private void StartStation()
    {
        //print("start station");

        //set random to match list of ints on station and screen 
        List<int> target = _stations[_stationCount].StationBehavior.SetRandomToMatch();
        _stations[_stationCount].ScreenBehavior.SetRandomOrderList(target);
        RestartTimer();
    }

    /// <summary>
    /// Called to start the timer
    /// </summary>
    private void RestartTimer()
    {
        StopAllCoroutines();
        _currentTime = _countDownTime;
        StartCoroutine(nameof(Timer));
    }

    /// <summary>
    /// Counts down from _countDownTime to 0, updating the timer screen text
    /// as it goes.
    /// </summary>
    /// <returns>Waits 1 second</returns>
    private IEnumerator Timer()
    {
        while(_currentTime >= 0)
        {
            yield return new WaitForSeconds(1f);

            int minutes = _currentTime / 60;
            int seconds = _currentTime % 60;
            if (seconds >= 10)
            {
                _timerText.text = minutes + ":" + seconds;
            }
            else
            {
                _timerText.text = minutes + ":0" + seconds;
            }

            _currentTime -= 1;
        }

        TriggerFail?.Invoke();
    }

    /// <summary>
    /// Function called to start the minigame. Triggered by NpcEventListener
    /// looking for OnMinigameStart with tag Angel.
    /// </summary>
    public void StartMinigame()
    {
        _stationCount = -1;
        SwitchStation();
        TriggerStart?.Invoke();
    }

    /// <summary>
    /// Function called when minigame is complete.
    /// </summary>
    private void StopMinigame()
    {
        StopAllCoroutines();
        _timerText.text = "0:00";
        _endMinigameEvent.TriggerEvent(_endMinigameEventTag);
        //TODO: make all the confirm buttons interactable.
        print("game over");
    }
}