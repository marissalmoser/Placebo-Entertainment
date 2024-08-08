using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class GameMusicController : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference mainGameMusic;
    [SerializeField] private FMODUnity.EventReference loopMusic;
    private const float LoopEndTime = 33f;
    private const float LoopTime = 600f;
    private Timer _shipFailTimer;
    private Timer _generatorExplodeTimer;
    private Timer _fireFailTimer;
    private EventInstance _musicInstance;
    private bool _hasEndedTime;

    private void Start()
    {
        _shipFailTimer = TimerManager.Instance.GetTimer("ShipFailTimer");
        _fireFailTimer = TimerManager.Instance.GetTimer("FireFailTimer");
        _generatorExplodeTimer = TimerManager.Instance.GetTimer("GeneratorExplodeTimer");
        _musicInstance = AudioManager.PlaySound(mainGameMusic, Vector3.zero);
    }

    public void SetMusic(bool loop)
    {
        if (loop)
        {
            AudioManager.StopSound(_musicInstance);
            //start loop music
            _musicInstance = AudioManager.PlaySound(loopMusic, Vector3.zero);
        }
        else
        {
            AudioManager.StopSound(_musicInstance);
            //start loop music
            _musicInstance = AudioManager.PlaySound(mainGameMusic, Vector3.zero);
        }
    }
}