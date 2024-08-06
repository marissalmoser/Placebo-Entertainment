using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAudioEvent : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference eventReference;
    [ContextMenu("Playsound")]
    private void DoThing()
    {
        AudioManager.PlaySound(eventReference, transform.position);
    }
}
