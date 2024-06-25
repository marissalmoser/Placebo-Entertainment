/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: June 24, 2024
*    Description: This is the script for the dial station's screen. It rotates
*    the on screen dial images depending based on the randomly sequence.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen2 : ScreenBehavior
{
    [SerializeField] private List<Transform> _targetDialTransforms;
    private int _currentInputIndex = 0;

    /// <summary>
    /// Sets on screen dials to randomly generated sequence.
    /// </summary>
    public override void SetOrderToRandom()
    {
        SetOrderToDefault();

        _currentInputIndex = 0;
        for (int i = 0; i < _screenObjsOrder.Count && _currentInputIndex < _targetDialTransforms.Count; i++)
        {
            switch (_screenObjsOrder[i])
            {
                // Up
                case 0:
                    _targetDialTransforms[_currentInputIndex].localEulerAngles = new Vector3(0, 0, 180);
                    break;

                // Right
                case 1:
                    _targetDialTransforms[_currentInputIndex].localEulerAngles = new Vector3(0, 0, 90);
                    break;

                // Down
                case 2:
                    _targetDialTransforms[_currentInputIndex].localEulerAngles = new Vector3(0, 0, 0);
                    break;

                // Left
                case 3:
                    _targetDialTransforms[_currentInputIndex].localEulerAngles = new Vector3(0, 0, 270);
                    break;
            }

            _currentInputIndex++;
        }
    }

    /// <summary>
    /// Resets all on screen dials to their up position.
    /// </summary>
    public override void SetOrderToDefault()
    {
        for (int i = 0; i < _targetDialTransforms.Count; i++)
        {
            _targetDialTransforms[i].localEulerAngles = new Vector3(0, 0, 180);
        }
    }
}
