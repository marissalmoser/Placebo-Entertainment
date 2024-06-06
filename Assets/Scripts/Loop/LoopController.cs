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
    [SerializeField] private int loopTimerTime;
    public int LoopTimerTimer => loopTimerTime;

    private void Start()
    {
        //Creating a timer. 3 minutes is 180 seconds
        loopTimer = TimerManager.Instance.CreateTimer("LoopTimer", 180);
        loopTimer.TimesUp += HandleLoopTimerEnd;
    }
    /// <summary>
    /// Handler for the event
    /// </summary>
    private void HandleLoopTimerEnd()
    {
        TimerManager.Instance.RemoveTimer("LoopTimer");

        ResetLoop();

        loopTimer.TimesUp -= HandleLoopTimerEnd;
    }
    /// <summary>
    /// Saving, loading the new scene, loading saved data
    /// </summary>
    public void ResetLoop()
    {
        SaveLoadManager.Instance.SaveGameToSaveFile();
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
        SaveLoadManager.Instance.LoadGameFromSaveFile();
    }
}
