/*****************************************************************************
// File Name :         MGWireState.cs
// Author :            Andrea Swihart-DeCoster
// Creation Date :     05/21/24
//
// Brief Description : Controls the state of the wire minigame
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGWireState : MonoBehaviour
{
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
    /// Called when a wire was placed in the correct slot
    /// </summary>
    private void AttachedWire()
    {
        if(++_currentAttachments == _maxAttachments)
        {
            EndWireGame();
        }
    }

    /// <summary>
    /// Wire game has been successfully won.
    /// </summary>
    private void EndWireGame()
    {
        print("Wire game won");

        // TODO: Alert other systems that wire game has been won.
    }
}
