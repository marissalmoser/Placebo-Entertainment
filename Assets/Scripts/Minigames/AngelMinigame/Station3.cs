/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: June 20, 2024
*    Description: This is the script for the arrows station in the angel minigame. It
*    overwrites the functionality for the functions from the base class to determine the
*    random order, then copies that list into one of bools to be compared with the 
*    inputed arrows.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Station3 : StationBehavior
{
    public static Action<Direction> ButtonClicked;

    public enum Direction
    { 
        Up,
        Right,
        Down,
        Left
    }

    private const int _sequenceLength = 5;
    private List<Direction> _inputSequence = new List<Direction>();
    private List<Direction> _correctSequence = new List<Direction>();
    private List<int> _correctSequenceInt = new List<int>();

    #region ActionRegistering
    private void OnEnable()
    {
        ButtonClicked += ArrowButtonPressed;
    }

    private void OnDisable()
    {
        ButtonClicked -= ArrowButtonPressed;
    }
    #endregion

    /// <summary>
    /// Called when an arrow button is pressed. Adds the input to the input sequence.
    /// </summary>
    /// <param name="arrowType">Direction of arrow being pressed</param>
    public void ArrowButtonPressed(Direction arrowType)
    {
        if (_inputSequence.Count < _sequenceLength)
            _inputSequence.Add(arrowType);
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
    /// <returns>Sequence of ints representing correct answer. 
    ///     0 = up, 1 = right, 2 = down, 3 = left</returns>
    public override List<int> SetRandomToMatch()
    {
        _correctSequence.Clear();
        _correctSequenceInt.Clear();

        for (int i = 0; i < _sequenceLength; i++)
        {
            int val = UnityEngine.Random.Range(0, 4);

            _correctSequence.Add((Direction)val);
            _correctSequenceInt.Add(val);
        }

        for (int i = 0; i < _correctSequenceInt.Count; i++)
        {
            Debug.Log(_correctSequenceInt[i]);
        }

        return _correctSequenceInt;
    }
}
