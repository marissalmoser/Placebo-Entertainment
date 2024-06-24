/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: June 24, 2024
*    Description: This is the script for the number station's screen. It updates the screen's
*    text show the target sequence and the player's inputs.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Screen4 : ScreenBehavior
{
    [SerializeField] private TextMeshProUGUI _targetSequenceText;
    [SerializeField] private TextMeshProUGUI _inputSequenceText;

    #region ActionRegistering
    private void OnEnable()
    {
        Station4.NumberButtonClicked += InputNumber;
    }

    private void OnDisable()
    {
        Station4.NumberButtonClicked -= InputNumber;
    }
    #endregion

    /// <summary>
    /// Invoked when button pressed action is triggered for station 4 to add 
    /// the new number on screen.
    /// </summary>
    /// <param name="num">Number being added to inputs</param>
    public void InputNumber(int num)
    {
        _inputSequenceText.text += num;
    }

    /// <summary>
    /// Sets target sequence to the randomly generated sequence and clears the input
    /// line.
    /// </summary>
    public override void SetOrderToRandom()
    {
        string newCorrectSequence = "Target: ";
        for (int i = 0; i < _screenObjsOrder.Count; i++)
        {
            newCorrectSequence += _screenObjsOrder[i];
        }

        _targetSequenceText.text = newCorrectSequence;
        _inputSequenceText.text = "Input: ";
    }
}
