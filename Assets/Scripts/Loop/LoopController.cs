/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Elijah Vroman
*    Date Created: 5/30/24?
*    Description: This monobehavior will be present in the scene to 
*    control when the scene resets. 
*******************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

public class LoopController : MonoBehaviour
{
    private Timer _loopTimer;
    [SerializeField] private int _loopTimerTime;
    public int LoopTimerTimer => _loopTimerTime;

    private void OnEnable()
    {
        //Creating a timer. 
        _loopTimer = TimerManager.Instance.CreateTimer("LoopTimer", _loopTimerTime);
        _loopTimer.TimesUp += HandleLoopTimerEnd;
    }
    /// <summary>
    /// Handler for the event
    /// </summary>
    private void HandleLoopTimerEnd()
    {
        TimerManager.Instance.RemoveTimer("LoopTimer");

        ResetLoop();

        _loopTimer.TimesUp -= HandleLoopTimerEnd;
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
