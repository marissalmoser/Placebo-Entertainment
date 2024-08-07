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
using UnityEngine.UI;

public class Screen4 : ScreenBehavior
{
    [SerializeField] private List<Image> _targetNumImages;
    [SerializeField] private List<Image> _inputNumImages;

    // Sprites will be order 0, 1, 2 ... 9
    [SerializeField] private List<Sprite> _numSprites;

    private int _currentSequenceLength = 0;

    #region ActionRegistering
    private void OnEnable()
    {
        Station4.NumberButtonClicked += InputNumber;
        Station4.ClearNumbers += ClearInputLine;
    }

    private void OnDisable()
    {
        Station4.NumberButtonClicked -= InputNumber;
        Station4.ClearNumbers -= ClearInputLine;
    }
    #endregion

    /// <summary>
    /// Invoked when button pressed action is triggered for station 4 to add 
    /// the new number on screen.
    /// </summary>
    /// <param name="num">Number being added to inputs</param>
    public void InputNumber(int num)
    {
        if (_currentSequenceLength < _inputNumImages.Count)
        {
            _inputNumImages[_currentSequenceLength].sprite = _numSprites[num];
            _inputNumImages[_currentSequenceLength].enabled = true;
            _currentSequenceLength++;
        }
    }

    /// <summary>
    /// Sets target sequence to the randomly generated sequence and clears the input
    /// line.
    /// </summary>
    public override void SetOrderToRandom()
    {
        for (int i = 0; i < _screenObjsOrder.Count && i < _targetNumImages.Count; i++)
        {
            _targetNumImages[i].sprite = _numSprites[ _screenObjsOrder[i] ];
        }

        ClearInputLine();
    }

    /// <summary>
    /// Called to clear inputs from screen.
    /// </summary>
    private void ClearInputLine()
    {
        _currentSequenceLength = 0;

        foreach (Image inputNum in _inputNumImages)
        {
            inputNum.sprite = _numSprites[0];
            inputNum.enabled = false;
        }
    }
}
