/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Elijah Vroman
*    Date Created: 5/30/24?
*    Description: Not even sure what this needs to do, but heres my best guess
*******************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

public class LoopController : MonoBehaviour
{
    private Timer loopTimer;

    private void Start()
    {
        //Creating a timer. 3 minutes is 180 seconds
        loopTimer = TimerManager.Instance.CreateTimer("LoopTimer", 180);
        loopTimer.TimesUp += HandleLoopTimerEnd;
    }

    private void HandleLoopTimerEnd()
    {
        TimerManager.Instance.RemoveTimer("LoopTimer");

        ResetLoop();

        loopTimer.TimesUp -= HandleLoopTimerEnd;
    }
    public void ResetLoop()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);

    }
    public void SaveInventory()
    {

    }
}
