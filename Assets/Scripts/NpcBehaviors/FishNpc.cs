using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishNpc : BaseNpc
{
    private bool _enteredFireRoom = false;
    [SerializeField] private int secondsUntilFailFireGame;

    public override void CheckForStateChange()
    {
        if(_currentState == NpcStates.DefaultIdle && _enteredFireRoom)
        {
            EnterMinigameReady();
        }
        else if (_currentState == NpcStates.PlayingMinigame)
        {
            EnterPostMinigame();
        }
    }
    protected override void EnterMinigameReady()
    {
        base.EnterMinigameReady(); 
    }
    protected override void EnterPostMinigame()
    {
        base.EnterPostMinigame();  
    }
    protected override void EnterIdle()
    {
        base.EnterIdle();
        TimerManager.Instance.CreateTimer("FireRoomMiniGameTimer", secondsUntilFailFireGame);
    }
}
