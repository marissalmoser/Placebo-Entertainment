using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private NpcEvent _eventToMakeTimer;
    [SerializeField] private NpcEventTags _NPCToAlert;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            _eventToMakeTimer.TriggerEvent(_NPCToAlert);
        }
    }
    public void Print()
    {
        print("A Timer successfully triggered this through events");
    }
}
