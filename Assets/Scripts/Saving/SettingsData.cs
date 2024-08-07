/******************************************************************
*    Author: Nick Grinstead
*    Contributors: 
*    Date Created: 8/6/24
*    Description: This class is used to save various settings values across
*       play sessions.
*******************************************************************/

[System.Serializable]
public class SettingsData
{
    public float MouseSensitivity;
    public float MasterVolume;
    public float MusicVolume;
    public float SfxVolume;

    public SettingsData(float mouseSens, float masterVol, float musicVol, float sfxVol)
    {
        MouseSensitivity = mouseSens;
        MasterVolume = masterVol;
        MusicVolume = musicVol;
        SfxVolume = sfxVol;
    }
}
