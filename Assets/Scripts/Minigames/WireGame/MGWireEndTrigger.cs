using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGWireEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        MGWire wire = GetComponentInParent<MGWire>();
        MGWireSlot slot = other.GetComponent<MGWireSlot>();
        if (wire && slot)
        {
            wire.EndTriggerEnter(slot);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MGWire wire = other.GetComponent<MGWire>();
        if (wire)
        {
            wire.EndTriggerExit();
        }
    }
}
