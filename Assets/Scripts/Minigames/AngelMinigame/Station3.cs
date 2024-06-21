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

    [SerializeField] private int _sequenceLength;
    private List<Direction> _inputSequence;
    private List<Direction> _correctSequence;
    private List<int> _correctSequenceInt;

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
        _inputSequence.Add(arrowType);
        // TODO: add to input row on screen
    }

    /// <summary>
    /// Compares the inputs to the correct sequence.
    /// </summary>
    /// <returns>True if sequences match</returns>
    public override bool CheckStates()
    {
        if (_inputSequence == _correctSequence)
        {
            RestartStationState();
            return true;
        }

        return false;
    }

    public override void RestartStationState()
    {
        _inputSequence.Clear();
        // TODO: Clear input row on screen
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
            int val = UnityEngine.Random.Range(0, 3);

            _correctSequence.Add((Direction)val);
            _correctSequenceInt.Add(val);
        }

        return _correctSequenceInt;
    }
}
