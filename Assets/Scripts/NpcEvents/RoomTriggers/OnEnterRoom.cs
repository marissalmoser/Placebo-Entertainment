/*****************************************************************************
// File Name :         OnEnterRoom.cs
// Author :            Andrea Swihart-DeCoster
// Contributors :      Marissa Moser
// Creation Date :     06/10/24
//
// Brief Description : Sends out events when the player enters a room.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterRoom : MonoBehaviour
{
    [SerializeField] private NpcEvent _onEnterRoom;
    [SerializeField] private NpcEventTags _eventTag;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            _onEnterRoom.TriggerEvent(_eventTag);
        }
    }
}
