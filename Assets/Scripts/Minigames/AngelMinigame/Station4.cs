/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: June 24, 2024
*    Description: This is the script for the numbers station in the angel minigame. It
*    overwrites the functionality for the functions from the base class to determine the
*    random order, and compars that list to an inputted set of ints.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Station4 : StationBehavior
{
    public static Action<int> NumberButtonClicked;
    public static Action ClearNumbers;

    [SerializeField] private int _sequenceLength = 6;
    private List<int> _inputSequence = new List<int>();
    private List<int> _correctSequence = new List<int>();

    #region ActionRegistering
    private void OnEnable()
    {
        NumberButtonClicked += NumberButtonPressed;
        ClearNumbers += RestartStationState;
    }

    private void OnDisable()
    {
        NumberButtonClicked -= NumberButtonPressed;
        ClearNumbers -= RestartStationState;
    }
    #endregion

    /// <summary>
    /// Invokes ClearNumbers event when lever is pulled.
    /// </summary>
    public override void InvokeClearEvent()
    {
        ClearNumbers?.Invoke();
    }

    /// <summary>
    /// Called when an number button is pressed. Adds the input to the input sequence.
    /// </summary>
    /// <param name="num">Number from button being pressed</param>
    public void NumberButtonPressed(int num)
    {
        _inputSequence.Add(num);
    }

    /// <summary>
    /// Compares the inputs to the correct sequence.
    /// </summary>
    /// <returns>True if sequences match</returns>
    public override bool CheckStates()
    {
        for (int i = 0; i < _sequenceLength; i++)
        {
            if (i >= _inputSequence.Count || i >= _correctSequence.Count || _inputSequence[i] != _correctSequence[i])
            {
                RestartStationState();
                return false;
            }
        }

        RestartStationState();
        return true;
    }

    /// <summary>
    /// Clears input sequence
    /// </summary>
    public override void RestartStationState()
    {
        _inputSequence.Clear();
    }

    /// <summary>
    /// Clears correct sequences before creating a new random sequence.
    /// </summary>
    /// <returns>Sequence of ints representing correct answer</returns>
    public override List<int> SetRandomToMatch()
    {
        _correctSequence.Clear();

        for (int i = 0; i < _sequenceLength; i++)
        {
            int val = UnityEngine.Random.Range(0, 10);
            
            _correctSequence.Add(val);
        }

        return _correctSequence;
    }
}
