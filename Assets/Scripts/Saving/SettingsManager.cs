/******************************************************************
*    Author: Nick Grinstead
*    Contributors: Elijah Vroman
*    Date Created: 8/6/24
*    Description: This manager tracks the game's settings and can be accessed 
*       via a static instance. It also handles saving and loading thoses settings
*       across play sessions.
*******************************************************************/
using UnityEngine;
using System.IO;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    private const string SaveDirectory = "/Settings/";
    private const string FileName = "SettingsData.sav";

    private SettingsData _settingsData;

    private float _mouseSens = 50f;
    private float _masterVolume = 50f;
    private float _musicVolume = 50f;
    private float _sfxVolume = 50f;

    public float MouseSensitivity { get => _mouseSens; private set => _mouseSens = value; }
    public float MasterVolume { get => _masterVolume; private set => _masterVolume = value; }
    public float MusicVolume { get => _musicVolume; private set => _musicVolume = value; }
    public float SfxVolume { get => _sfxVolume; private set => _sfxVolume = value; }

    /// <summary>
    /// Ensures there's only one instance of this before loading settings data
    /// </summary>
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSettingsFromFile();
    }

    /// <summary>
    /// Assigns mouse sensitivity after clamping it
    /// </summary>
    /// <param name="newSens">new mouse sensitivity as float</param>
    public void SetMouseSensitivity(float newSens)
    {
        MouseSensitivity = Mathf.Clamp(newSens, 0f, 100f);
    }

    /// <summary>
    /// Assigns volume settings to variables after clamping them
    /// </summary>
    /// <param name="newMasterVol">float for master volume</param>
    /// <param name="newMusicVol">float for music volume</param>
    /// <param name="newSfxVol">float for sfx volume</param>
    public void SetVolumeValues(float newMasterVol, float newMusicVol, float newSfxVol)
    {
        MasterVolume = Mathf.Clamp(newMasterVol, 0f, 100f);
        MusicVolume = Mathf.Clamp(newMusicVol, 0f, 100f);
        SfxVolume = Mathf.Clamp(newSfxVol, 0f, 100f);
    }

    /// <summary>
    /// Saves settings data to a file
    /// Pulled from SaveLoadManager.cs
    /// </summary>
    public void SaveSettingsToFile()
    {
        string directoryName = Application.persistentDataPath + SaveDirectory;
        //creating a file at this location if it doesnt exist already. If it
        //does, we will overwrite it    
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        // COllecting data
        _settingsData = new SettingsData(MouseSensitivity, MasterVolume, MusicVolume, SfxVolume); 
        string jsonString = JsonUtility.ToJson(_settingsData, true);
        //prettyPrint is nice; organizes the file
        File.WriteAllText(directoryName + FileName, jsonString);
        GUIUtility.systemCopyBuffer = directoryName;
    }

    /// <summary>
    /// Using json utility to reconstruct the settings from the file 
    /// Pulled from SaveLoadManager.cs
    /// </summary>
    public void LoadSettingsFromFile()
    {
        string fullPath = Application.persistentDataPath + SaveDirectory + FileName;
        // Create loaded data with default values
        SettingsData loadedData = new SettingsData(50f, 50f, 50f, 50f);

        if (File.Exists(fullPath))//if a file exists at this path
        {
            string jsonString = File.ReadAllText(fullPath);
            loadedData = JsonUtility.FromJson<SettingsData>(jsonString);
        }

        // Assigning data
        SetMouseSensitivity(loadedData.MouseSensitivity);
        SetVolumeValues(loadedData.MasterVolume, loadedData.MusicVolume, loadedData.SfxVolume);
    }

    /// <summary>
    /// Saves settings upon closing the game
    /// </summary>
    private void OnDisable()
    {
        SaveSettingsToFile();
    }
}
