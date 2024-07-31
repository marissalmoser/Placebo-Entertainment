/*****************************************************************************
// File Name :         GearBehavior.cs
// Author :            Mark Hanson
// Contributors :      Marissa Moser, Nick Grinstead
// Creation Date :     5/24/2024
//
// Brief Description : Any function to do for the gears mini game will be found here. Includes swapping slots, Correct slot pattern with all bad ones, and selecting gears for each slot.
*****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlaceboEntertainment.UI;

public class GearBehavior : MonoBehaviour, IInteractable
{
    public static Action CorrectGear;

    [SerializeField] private string _interactPromptText = "GEAR";

    [Header("Individual Gear")]
    [SerializeField] private GameObject[] _gearSizes;
    [SerializeField] private int _startingGearIndex;
    private int _currentGearSizeIndex;

    [Header("Correct Gear")]
    [SerializeField] private int _rightGearNum;
    private bool _isComplete = false;
    public bool IsComplete { get => _isComplete; private set => _isComplete = value; }

    /// <summary>
    /// Ensures only starting gear is active
    /// </summary>
    void Start()
    {
        if (_startingGearIndex >= _gearSizes.Length || _startingGearIndex < 0)
        {
            _startingGearIndex = 0;
        }
        _currentGearSizeIndex = _startingGearIndex;

        for (int i = 0; i < _gearSizes.Length; ++i)
        {
            if (i == _startingGearIndex)
                _gearSizes[i].SetActive(true);
            else
                _gearSizes[i].SetActive(false);
        }
    }

    /// <summary>
    /// Cycles through gears if this slot has not been completed
    /// </summary>
    /// <param name="player">Player interacting</param>
    public void Interact(GameObject player)
    {
        if (!_isComplete)
        {
            int previousIndex = _currentGearSizeIndex;
            _currentGearSizeIndex++;
            _currentGearSizeIndex %= _gearSizes.Length;

            if (previousIndex < _gearSizes.Length && previousIndex >= 0)
                _gearSizes[previousIndex].SetActive(false);
            if (_currentGearSizeIndex < _gearSizes.Length && _currentGearSizeIndex >= 0)
                 _gearSizes[_currentGearSizeIndex].SetActive(true);
            
            CheckGearCompletion();
        }
    }

    /// <summary>
    /// Updates _isComplete to true if the current gear matches the correct one
    /// </summary>
    private void CheckGearCompletion()
    {
        if (_currentGearSizeIndex == _rightGearNum)
        {
            _isComplete = true;
            CorrectGear?.Invoke();
            HideInteractUI();
        }
    }

    /// <summary>
    /// Called by GearCompletionCheck to force gears into their completed state
    /// </summary>
    public void SetGearToComplete()
    {
        _isComplete = true;
        if (_currentGearSizeIndex < _gearSizes.Length && _currentGearSizeIndex >= 0)
            _gearSizes[_currentGearSizeIndex].SetActive(false);
        if (_rightGearNum < _gearSizes.Length && _rightGearNum >= 0)
            _gearSizes[_rightGearNum].SetActive(true);
    }

    /// <summary>
    /// Shows UI prompt to interact with gears.
    /// </summary>
    public void DisplayInteractUI()
    {
        if (!_isComplete)
        {
            TabbedMenu.Instance.ToggleInteractPrompt(true, _interactPromptText);
        }
    }

    /// <summary>
    /// Hides UI prompt to interact with gears.
    /// </summary>
    public void HideInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(false);
    }
}
