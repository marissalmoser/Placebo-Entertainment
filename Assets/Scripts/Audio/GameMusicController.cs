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
    private Timer _timer;
    private EventInstance _musicInstance;
    private bool _hasEndedTime;

    private void Start()
    {
        _timer = TimerManager.Instance.GetTimer("Example");
        _musicInstance = AudioManager.PlaySound(mainGameMusic, Vector3.zero);
    }

    private void Update()
    {
        float currentTime = _timer.GetCurrentTimeInSeconds(); //how i wish we had a callback for this...

        if (currentTime < LoopEndTime + 1)
        {
            if (!_hasEndedTime)
            {
                //stop the main gameplay music
                AudioManager.StopSound(_musicInstance);
                //start loop music
                AudioManager.PlaySound(loopMusic, Vector3.zero);
                _hasEndedTime = true;
            }
        }
        else if (currentTime > LoopEndTime)
        {
            if (_hasEndedTime)
            {
                _musicInstance = AudioManager.PlaySound(mainGameMusic, Vector3.zero);
                _hasEndedTime = false;
            }
        }
    }
}