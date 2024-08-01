/******************************************************************
 *    Author: Nick Grinstead
 *    Contributors:
 *    Date Created: 7/12/2024
 *    Description: Manager script for the pause menu. Automatically handles
 *                 registering of button callbacks.
 *******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private UIDocument _pauseMenu;

    private const string ContinueButtonName = "ContinueButton";
    private const string ExitButtonName = "ExitButton";

    private Button _continueButton;
    private Button _exitButton;
    private bool _isGamePaused = false;

    /// <summary>
    /// Registering callbacks
    /// </summary>
    private void Awake()
    {
        _pauseMenu.rootVisualElement.style.display = DisplayStyle.None;

        _continueButton = _pauseMenu.rootVisualElement.Q<Button>(ContinueButtonName);
        _exitButton = _pauseMenu.rootVisualElement.Q<Button>(ExitButtonName);

        _continueButton.RegisterCallback<ClickEvent>(ContinuePressed);
        _exitButton.RegisterCallback<ClickEvent>(ExitToMenu);
    }

    /// <summary>
    /// Setting up player inputs
    /// </summary>
    private void Start()
    {
        PlayerController.Instance.PlayerControls.BasicControls.PauseGame.performed += PauseGamePerformed;
    }

    /// <summary>
    /// Unregistering callbacks and player inputs
    /// </summary>
    private void OnDisable()
    {
        _continueButton.UnregisterCallback<ClickEvent>(ContinuePressed);
        _exitButton.UnregisterCallback<ClickEvent>(ExitToMenu);

        PlayerController.Instance.PlayerControls.BasicControls.PauseGame.performed -= PauseGamePerformed;
    }

    /// <summary>
    /// Toggles pause menu UI
    /// </summary>
    /// <param name="isActive">True if menu should be visible</param>
    public void TogglePauseMenu(bool isActive)
    {
        _isGamePaused = isActive;
        UnityEngine.Cursor.visible = isActive;
        UnityEngine.Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;

        _pauseMenu.rootVisualElement.style.display = isActive ? DisplayStyle.Flex : DisplayStyle.None;
        Time.timeScale = isActive ? 0 : 1;
    }

    /// <summary>
    /// Invoked by player pause input
    /// </summary>
    /// <param name="obj">Callback from input</param>
    private void PauseGamePerformed(InputAction.CallbackContext obj)
    {
        TogglePauseMenu(!_isGamePaused);
    }

    /// <summary>
    /// Invoked when continue button is pressed to disable menu
    /// </summary>
    /// <param name="click">ClickEvent from button</param>
    private void ContinuePressed(ClickEvent click)
    {
        TogglePauseMenu(false);
    }

    /// <summary>
    /// Invoked when main menu button is pressed to load that scene
    /// </summary>
    /// <param name="click">ClickEvent from button</param>
    private void ExitToMenu(ClickEvent click)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
