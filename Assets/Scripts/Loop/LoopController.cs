/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Elijah Vroman, Alec Pizziferro
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
    [SerializeField] private int endScreenDelay;

    [SerializeField] private NpcEvent temporaryLoop;
    [SerializeField] private NpcEventTags temporaryTag;
    public int LoopTimerTimer => _loopTimerTime;

    private void Start()
    {
        //Creating a timer. 
        _loopTimer = TimerManager.Instance.CreateTimer("LoopTimer", _loopTimerTime + endScreenDelay, temporaryLoop, temporaryTag);
        //_loopTimer.TimesUp += HandleLoopTimerEnd;
        LoadSave();
    }
    /// <summary>
    /// Handler for the event
    /// </summary>
    private void HandleLoopTimerEnd()
    {
        TimerManager.Instance.RemoveTimer("LoopTimer");

        ResetLoop();

        //_loopTimer.TimesUp -= HandleLoopTimerEnd;
    }
    /// <summary>
    /// Saving, loading the new scene, loading saved data
    /// </summary>
    public void ResetLoop()
    {
        SaveLoadManager.Instance.SaveGameToSaveFile();
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
    }

    /// <summary>
    /// Called to load the game
    /// </summary>
    private void LoadSave()
    {
        SaveLoadManager.Instance.LoadGameFromSaveFile();
    }
}
