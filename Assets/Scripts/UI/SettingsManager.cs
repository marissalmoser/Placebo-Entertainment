using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    private float _mouseSens = 50f;
    private float _masterVolume = 50f;
    private float _musicVolume = 50f;
    private float _sfxVolume = 50f;

    public float MouseSensitivity { get => _mouseSens; private set => _mouseSens = value; }
    public float MasterVolume { get => _masterVolume; private set => _masterVolume = value; }
    public float MusicVolume { get => _musicVolume; private set => _musicVolume = value; }
    public float SfxVolume { get => _sfxVolume; private set => _sfxVolume = value; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetMouseSensitivity(float newSens)
    {
        MouseSensitivity = Mathf.Clamp(newSens, 0f, 100f);
    }

    public void SetVolumeValues(float newMasterVol, float newMusicVol, float newSfxVol)
    {
        MasterVolume = Mathf.Clamp(newMasterVol, 0f, 100f);
        MusicVolume = Mathf.Clamp(newMusicVol, 0f, 100f);
        SfxVolume = Mathf.Clamp(newSfxVol, 0f, 100f);
    }
}
