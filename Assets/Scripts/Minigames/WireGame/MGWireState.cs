/*****************************************************************************
// File Name :         MGWireState.cs
// Author :            Andrea Swihart-DeCoster
// Creation Date :     05/21/24
//
// Brief Description : Controls the state of the wire minigame.
*****************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGWireState : MonoBehaviour
{
    public static Action WireGameWon;

    private const int _maxAttachments = 3;
    private int _currentAttachments = 0;

    private void OnEnable()
    {
        MGWireSlot.CorrectWire += AttachedWire;
    }

    private void OnDisable()
    {
        MGWireSlot.CorrectWire -= AttachedWire;
    }

    /// <summary>
    /// Called when a wire was placed in the correct slot. Checks to see if
    /// the end conditions for winning have been met.
    /// </summary>
    private void AttachedWire()
    {
        if(++_currentAttachments == _maxAttachments)
        {
            EndWireGame();
        }
    }

    /// <summary>
    /// Called when the wire game has been successfully won.
    /// </summary>
    private void EndWireGame()
    {
        print("Wire game won");
        WireGameWon?.Invoke();
        // TODO: Alert other systems that wire game has been won. Use a
        // static action for this maybe? Work with Nick Gs event system.
    }
}
