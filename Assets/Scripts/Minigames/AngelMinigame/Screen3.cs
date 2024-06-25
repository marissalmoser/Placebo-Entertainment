/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: June 20, 2024
*    Description: This is the script for the arrow station's screen. It sets the screen's
*    arrows to their directional state depending on the random order.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen3 : ScreenBehavior
{
    [SerializeField] private List<Transform> _targetArrowTransforms;
    [SerializeField] private List<Transform> _inputArrowTransforms;
    private List<Image> _inputArrowImages = new List<Image>();
    private int _currentInputIndex = 0;

    /// <summary>
    /// Getting Images for input arrows
    /// </summary>
    private void Awake()
    {
        for (int i = 0; i < _inputArrowTransforms.Count; i++)
        {
            Image tempImage = _inputArrowTransforms[i].GetComponent<Image>();
            _inputArrowImages.Add(tempImage);
        }
    }

    #region ActionRegistering
    private void OnEnable()
    {
        Station3.ButtonClicked += InputArrow;
        Station3.ClearArrows += ClearInputLine;
    }

    private void OnDisable()
    {
        Station3.ButtonClicked -= InputArrow;
        Station3.ClearArrows -= ClearInputLine;
    }
    #endregion

    /// <summary>
    /// Invoked when button pressed action is triggered for station 3 to set
    /// the next input arrow on screen.
    /// </summary>
    /// <param name="arrowDirection">The direction of the arrow</param>
    private void InputArrow(Station3.Direction arrowDirection)
    {
        if (_currentInputIndex < _inputArrowTransforms.Count)
        {
            int arrowDirectionInt = (int)arrowDirection;

            switch (arrowDirectionInt)
            {
                // Up
                case 0:
                    _inputArrowTransforms[_currentInputIndex].localEulerAngles = new Vector3(0, 0, 180);
                    break;
                
                // Right
                case 1:
                    _inputArrowTransforms[_currentInputIndex].localEulerAngles = new Vector3(0, 0, 90);
                    break;

                // Down
                case 2:
                    _inputArrowTransforms[_currentInputIndex].localEulerAngles = new Vector3(0, 0, 0);
                    break;

                // Left
                case 3:
                    _inputArrowTransforms[_currentInputIndex].localEulerAngles = new Vector3(0, 0, 270);
                    break;
            }

            _inputArrowImages[_currentInputIndex].enabled = true;

            _currentInputIndex++;
        }
    }

    /// <summary>
    /// Sets target arrows to the randomly generated sequence and clears the input
    /// line.
    /// </summary>
    public override void SetOrderToRandom()
    {
        for (int i = 0; i < _targetArrowTransforms.Count && i < _screenObjsOrder.Count; i++)
        {
            int arrowDirection = _screenObjsOrder[i];

            switch (arrowDirection)
            {
                // Up
                case 0:
                    _targetArrowTransforms[i].localEulerAngles = new Vector3(0, 0, 180);
                    break;

                // Right
                case 1:
                    _targetArrowTransforms[i].localEulerAngles = new Vector3(0, 0, 90);
                    break;

                // Down
                case 2:
                    _targetArrowTransforms[i].localEulerAngles = new Vector3(0, 0, 0);
                    break;

                // Left
                case 3:
                    _targetArrowTransforms[i].localEulerAngles = new Vector3(0, 0, 270);
                    break;
            }
        }

        ClearInputLine();
    }

    /// <summary>
    /// Clears inputted arrows.
    /// </summary>
    private void ClearInputLine()
    {
        _currentInputIndex = 0;

        for (int i = 0; i < _inputArrowTransforms.Count; i++)
        {
            _inputArrowImages[i].enabled = false;
        }
    }
}
