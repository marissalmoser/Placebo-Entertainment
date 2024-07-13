using UnityEngine;

public class DummyScript : MonoBehaviour
{
    [SerializeField] private int _timeAmountOfTimerMadeWithCode;
    [SerializeField] private string _nameOfTimerMadeWithCode;
    [Header("Events")]
    [SerializeField] private NpcEvent _eventToMakeTimer;
    [SerializeField] private NpcEventTags _NPCToAlert;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _eventToMakeTimer.TriggerEvent(_NPCToAlert);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TimerManager.Instance.CreateTimer(_nameOfTimerMadeWithCode, _timeAmountOfTimerMadeWithCode, null, NpcEventTags.Game);
        }
    }
    public void Print()
    {
        print("A Timer successfully triggered this through events");
    }
}
