using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterRoom : MonoBehaviour
{
    [SerializeField] private NpcEvent _onEnterRoom;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            _onEnterRoom.TriggerEvent(NpcEventTags.Lab);
        }
    }
}
