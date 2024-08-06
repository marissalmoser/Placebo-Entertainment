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
    private const string SettingsButtonName = "SettingsButton";
    private const string ExitButtonName = "ExitButton";
    private const string AudioButtonName = "AudioButton";
    private const string ControlsButtonName = "ControlsButton";
    private const string PauseHolderName = "PauseBackground";
    private const string SelectionHolderName = "SettingsSelection";
    private const string AudioHolderName = "AudioHolder";
    private const string ControlsHolderName = "ControlsHolder";

    private Button _continueButton;
    private Button _settingsButton;
    private Button _exitButton;
    private Button _audioButton;
    private Button _controlsButton;
    private VisualElement _pauseHolder;
    private VisualElement _selectionHolder;
    private VisualElement _audioHolder;
    private VisualElement _controlsHolder;
    private bool _isGamePaused = false;

    // 0 = pause, 1 = settings selection, 2 = settings submenu
    private int _currentScreenIndex = 0;

    /// <summary>
    /// Registering callbacks
    /// </summary>
    private void Awake()
    {
        _pauseMenu.rootVisualElement.style.display = DisplayStyle.None;

        // Getting references to buttons
        _continueButton = _pauseMenu.rootVisualElement.Q<Button>(ContinueButtonName);
        _settingsButton = _pauseMenu.rootVisualElement.Q<Button>(SettingsButtonName);
        _exitButton = _pauseMenu.rootVisualElement.Q<Button>(ExitButtonName);
        _audioButton = _pauseMenu.rootVisualElement.Q<Button>(AudioButtonName);
        _controlsButton = _pauseMenu.rootVisualElement.Q<Button>(ControlsButtonName);

        // Getting references to screen holders
        _pauseHolder = _pauseMenu.rootVisualElement.Q(PauseHolderName);
        _selectionHolder = _pauseMenu.rootVisualElement.Q(SelectionHolderName);
        _audioHolder = _pauseMenu.rootVisualElement.Q(AudioHolderName);
        _controlsHolder = _pauseMenu.rootVisualElement.Q(ControlsHolderName);

        // Registering button callbacks
        _continueButton.RegisterCallback<ClickEvent>(ContinuePressed);
        _settingsButton.RegisterCallback<ClickEvent>(SettingsButtonClicked);
        _audioButton.RegisterCallback<ClickEvent>(AudioButtonClicked);
        _controlsButton.RegisterCallback<ClickEvent>(ControlsButtonClicked);
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
    /// Invoked by player pause input. Either toggles pause menu or navigates
    /// back through submenus.
    /// </summary>
    /// <param name="obj">Callback from input</param>
    private void PauseGamePerformed(InputAction.CallbackContext obj)
    {
        // Game isn't paused and should pause
        if (!_isGamePaused)
        {
            TogglePauseMenu(true);
        }
        // Game is on main pause screen and should unpause
        else if (_currentScreenIndex == 0)
        {
            TogglePauseMenu(false);
        }
        // Return to main pause screen from settings selection
        else if (_currentScreenIndex == 1)
        {
            _currentScreenIndex = 0;
            _selectionHolder.style.display = DisplayStyle.None;
            _pauseHolder.style.display = DisplayStyle.Flex;
        }
        // Return to settings selection from settings submenu
        else if (_currentScreenIndex == 2)
        {
            _currentScreenIndex = 1;
            _audioHolder.style.display = DisplayStyle.None;
            _controlsHolder.style.display = DisplayStyle.None;
            _selectionHolder.style.display = DisplayStyle.Flex;
        }
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
    /// Opens settings selection menu
    /// </summary>
    /// <param name="clicked">Click event</param>
    private void SettingsButtonClicked(ClickEvent clicked)
    {
        _currentScreenIndex = 1;
        _pauseHolder.style.display = DisplayStyle.None;
        _selectionHolder.style.display = DisplayStyle.Flex;
    }

    /// <summary>
    /// Opens audio options submenu
    /// </summary>
    /// <param name="clicked">Click event</param>
    private void AudioButtonClicked(ClickEvent clicked)
    {
        _currentScreenIndex = 2;
        _selectionHolder.style.display = DisplayStyle.None;
        _audioHolder.style.display = DisplayStyle.Flex;
    }

    /// <summary>
    /// Opens controls options submenu
    /// </summary>
    /// <param name="clicked">Click event</param>
    private void ControlsButtonClicked(ClickEvent clicked)
    {
        _currentScreenIndex = 2;
        _selectionHolder.style.display = DisplayStyle.None;
        _controlsHolder.style.display = DisplayStyle.Flex;
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
