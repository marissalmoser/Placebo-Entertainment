/******************************************************************
*    Author: Marissa Moser
*    Contributors: 
*    Date Created: June 19, 2024
*    Description: This is the script for the first station in the angel minigame. It
*    writes the functionality for the functions from the base class to determine the
*    random order, then copies that list into one of bools to be compared with the 
*    lever states.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using FMOD;

public class Station1 : StationBehavior
{
    [SerializeField] private List<GameObject> _levers;

    private List<bool> _leversCorrectState = new List<bool> { false, false, false, false, false};
    private List<int> _leversCorrectStateInt = new List<int> { 0, 0, 0, 0, 0};

    private void Start()
    {
        AngelMinigameManager.TriggerStart += RestartStationState;
        AngelMinigameManager.TriggerFail += RestartStationState;
    }

    public override bool CheckStates()
    {
        for(int i = 0; i < 5; i++)
        {
            if (_levers[i].GetComponent<LeverInteraction>()._isOn != _leversCorrectState[i])
            {
                RestartStationState();
                return false;
            }
            //if all levers have been checked and are correct
            if(i == 4)
            {
                RestartStationState();
                return true;
            }
        }
        return false;
    }

    public override void RestartStationState()
    {
        foreach (GameObject lever in _levers)
        {
            lever.GetComponent<LeverInteraction>().SetLever(false);
        }
    }


    public override List<int> SetRandomToMatch()
    {
        for (int i = 0; i < 5; i++)
        {
            int val = UnityEngine.Random.Range(0, 2);

            //duplicates list into another list of bools to be compared to lever states.
            switch (val)
            {
                case 0:
                    _leversCorrectState[i] = false;
                    _leversCorrectStateInt[i] = 0;
                    break;
                case 1:
                    _leversCorrectState[i] = true;
                    _leversCorrectStateInt[i] = 1;
                    break;
            }
        }

        return _leversCorrectStateInt;
    }

    private void OnDisable()
    {
        AngelMinigameManager.TriggerStart -= RestartStationState;
        AngelMinigameManager.TriggerFail -= RestartStationState;
    }
}
