/******************************************************************
*    Author: Nick Grinstead
*    Contributors:
*    Date Created: 8/7/24
*    Description: This script needs to be attached to the gameobject containing
*       the cinemachine camera. It will handle updating the camera's max speed
*       as the mouse sensitivity setting changes.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MouseSensUpdater : MonoBehaviour
{
    [SerializeField] private float _cameraSpeedLowEnd = 150f;
    [SerializeField] private float _cameraSpeedHighEnd = 400f;

    private float _camSpeedRange;
    private SettingsManager _settingsManager;
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachinePOV _cameraPov;

    /// <summary>
    /// Setting references and initial camera speed
    /// </summary>
    private void Start()
    {
        _settingsManager = SettingsManager.Instance;

        _camSpeedRange = _cameraSpeedHighEnd - _cameraSpeedLowEnd;

        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cameraPov = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();

        UpdateCameraSpeed();

        if (_settingsManager != null)
            _settingsManager.MouseSensUpdated += UpdateCameraSpeed;
    }

    /// <summary>
    /// Unregistering from action
    /// </summary>
    ~MouseSensUpdater()
    {
        if (_settingsManager != null)
            _settingsManager.MouseSensUpdated -= UpdateCameraSpeed;
    }

    /// <summary>
    /// Updates virtual camera's horizontal and vertical max speed
    /// </summary>
    private void UpdateCameraSpeed()
    {
        if (_settingsManager != null)
        {
            float newSpeed = (_camSpeedRange * (_settingsManager.MouseSensitivity / 100)) + _cameraSpeedLowEnd;
            _cameraPov.m_HorizontalAxis.m_MaxSpeed = newSpeed;
            _cameraPov.m_VerticalAxis.m_MaxSpeed = newSpeed;
        }
    }
}
