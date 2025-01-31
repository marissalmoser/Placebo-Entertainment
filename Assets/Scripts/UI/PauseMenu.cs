/******************************************************************
 *    Author: Nick Grinstead
 *    Contributors:
 *    Date Created: 7/12/2024
 *    Description: Manager script for the pause menu. Automatically handles
 *                 registering of button callbacks.
 *******************************************************************/
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using PlaceboEntertainment.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Users;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private UIDocument _pauseMenu;
    [SerializeField] private float _tabAnimationTime = 0.25f;
    [SerializeField] private EventReference confirmEvent;

    #region Constants
    private const string ContinueButtonName = "ContinueButton";
    private const string SettingsButtonName = "SettingsButton";
    private const string ExitButtonName = "ExitButton";
    private const string AudioButtonName = "AudioButton";
    private const string ControlsButtonName = "ControlsButton";
    private const string PauseHolderName = "PauseBackground";
    private const string SelectionHolderName = "SettingsSelection";
    private const string AudioHolderName = "AudioHolder";
    private const string ControlsHolderName = "ControlsHolder";
    private const string MouseSensSliderName = "MouseSensSlider";
    private const string MasterSliderName = "MasterSlider";
    private const string MusicSliderName = "MusicSlider";
    private const string SfxSliderName = "SFXSlider";
    private const string TopTabName = "TopTab";
    private const string MiddleTabName = "MiddleTab";
    private const string BottomTabName = "BottomTab";
    #endregion

    #region Private
    private TabbedMenu _tabbedMenu;
    private Button _continueButton;
    private Button _settingsButton;
    private Button _exitButton;
    private Button _audioButton;
    private Button _controlsButton;
    private VisualElement _pauseHolder;
    private VisualElement _selectionHolder;
    private VisualElement _audioHolder;
    private VisualElement _controlsHolder;
    private Slider _mouseSensSlider;
    private Slider _masterVolSlider;
    private Slider _musicVolSlider;
    private Slider _sfxVolSlider;
    private VisualElement _topTab;
    private VisualElement _middleTab;
    private VisualElement _bottomTab;
    private bool _isGamePaused = false;
    private SettingsManager _settingsManager;
    private Coroutine _activeCoroutine;
    private UQueryBuilder<Button> _allButtons;

    private GameObject _lastFocusedElement;
    private Button _lastFocusedVisualElement;

    private List<Slider> _sliders = new List<Slider>();
    // 0 = pause, 1 = settings selection, 2 = settings submenu
    private int _currentScreenIndex = 0;
    #endregion

    /// <summary>
    /// Registering callbacks
    /// </summary>
    private void Awake()
    {
        InputUser.onChange += UpdateFocusOnInputChange;

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

        // Assigning slider references
        _mouseSensSlider = _pauseMenu.rootVisualElement.Q<Slider>(MouseSensSliderName);
        _masterVolSlider = _pauseMenu.rootVisualElement.Q<Slider>(MasterSliderName);
        _musicVolSlider = _pauseMenu.rootVisualElement.Q<Slider>(MusicSliderName);
        _sfxVolSlider = _pauseMenu.rootVisualElement.Q<Slider>(SfxSliderName);

        // Assigning animated tab references
        _topTab = _pauseMenu.rootVisualElement.Q(TopTabName);
        _middleTab = _pauseMenu.rootVisualElement.Q(MiddleTabName);
        _bottomTab = _pauseMenu.rootVisualElement.Q(BottomTabName);

        // Registering button callbacks
        _continueButton.RegisterCallback<NavigationSubmitEvent>(ContinuePressed);
        _settingsButton.RegisterCallback<NavigationSubmitEvent>(SettingsButtonClicked);
        _audioButton.RegisterCallback<NavigationSubmitEvent>(AudioButtonClicked);
        _controlsButton.RegisterCallback<NavigationSubmitEvent>(ControlsButtonClicked);
        _exitButton.RegisterCallback<NavigationSubmitEvent>(ExitToMenu);

        _continueButton.RegisterCallback<MouseOverEvent>(evt => { ChangeButtonFocus(_continueButton); });
        _audioButton.RegisterCallback<MouseOverEvent>(evt => { ChangeButtonFocus(_audioButton); });
        _controlsButton.RegisterCallback<MouseOverEvent>(evt => { ChangeButtonFocus(_controlsButton); });
        _settingsButton.RegisterCallback<MouseOverEvent>(evt => { ChangeButtonFocus(_settingsButton); });
        _exitButton.RegisterCallback<MouseOverEvent>(evt => { ChangeButtonFocus(_exitButton); });

        _continueButton.RegisterCallback<MouseOutEvent>(evt => { ClearButtonFocus(); });
        _audioButton.RegisterCallback<MouseOutEvent>(evt => { ClearButtonFocus(); });
        _controlsButton.RegisterCallback<MouseOutEvent>(evt => { ClearButtonFocus(); });
        _settingsButton.RegisterCallback<MouseOutEvent>(evt => { ClearButtonFocus(); });
        _exitButton.RegisterCallback<MouseOutEvent>(evt => { ClearButtonFocus(); });

        // Registering animated tab related callbacks
        _continueButton.RegisterCallback<MouseOverEvent>(evt => { AnimateTab(_topTab, true); });
        _continueButton.RegisterCallback<MouseOutEvent>(evt => { AnimateTab(_topTab, false); });
        _audioButton.RegisterCallback<MouseOverEvent>(evt => { AnimateTab(_topTab, true); });
        _audioButton.RegisterCallback<MouseOutEvent>(evt => { AnimateTab(_topTab, false); });
        _controlsButton.RegisterCallback<MouseOverEvent>(evt => { AnimateTab(_middleTab, true); });
        _controlsButton.RegisterCallback<MouseOutEvent>(evt => { AnimateTab(_middleTab, false); });
        _settingsButton.RegisterCallback<MouseOverEvent>(evt => { AnimateTab(_middleTab, true); });
        _settingsButton.RegisterCallback<MouseOutEvent>(evt => { AnimateTab(_middleTab, false); });
        _exitButton.RegisterCallback<MouseOverEvent>(evt => { AnimateTab(_bottomTab, true); });
        _exitButton.RegisterCallback<MouseOutEvent>(evt => { AnimateTab(_bottomTab, false); });

        _continueButton.RegisterCallback<FocusInEvent>(evt => { AnimateTab(_topTab, true); });
        _continueButton.RegisterCallback<FocusOutEvent>(evt => { AnimateTab(_topTab, false); });
        _audioButton.RegisterCallback<FocusInEvent>(evt => { AnimateTab(_topTab, true); });
        _audioButton.RegisterCallback<FocusOutEvent>(evt => { AnimateTab(_topTab, false); });
        _controlsButton.RegisterCallback<FocusInEvent>(evt => { AnimateTab(_middleTab, true); });
        _controlsButton.RegisterCallback<FocusOutEvent>(evt => { AnimateTab(_middleTab, false); });
        _settingsButton.RegisterCallback<FocusInEvent>(evt => { AnimateTab(_middleTab, true); });
        _settingsButton.RegisterCallback<FocusOutEvent>(evt => { AnimateTab(_middleTab, false); });
        _exitButton.RegisterCallback<FocusInEvent>(evt => { AnimateTab(_bottomTab, true); });
        _exitButton.RegisterCallback<FocusOutEvent>(evt => { AnimateTab(_bottomTab, false); });

        _allButtons = _pauseMenu.rootVisualElement.Query<Button>();
        _allButtons.ForEach(button =>
        {
            button.RegisterCallback<NavigationSubmitEvent>(PlayConfirmSound);
        });

        if (_tabAnimationTime <= 0)
            _tabAnimationTime = 0.25f;
        
        _sliders = _audioHolder.Query<Slider>().ToList();
        FMODUnity.RuntimeManager.StudioSystem.getParameterByName("MasterVolume", out float volume);
        _sliders[0].value = volume;
        _sliders[0].RegisterCallback<ChangeEvent<float>>(MasterAudioSliderChanged);
        FMODUnity.RuntimeManager.StudioSystem.getParameterByName("SFXVolume", out volume);
        _sliders[1].value = volume;
        _sliders[1].RegisterCallback<ChangeEvent<float>>(SFXAudioSliderChanged);
        FMODUnity.RuntimeManager.StudioSystem.getParameterByName("MusicVolume", out volume);
        _sliders[2].value = volume;
        _sliders[2].RegisterCallback<ChangeEvent<float>>(MusicAudioSliderChanged);
    }

    private void PlayConfirmSound(NavigationSubmitEvent evt)
    {
        AudioManager.PlaySound(confirmEvent, transform.position);
    }

    /// <summary>
    /// Setting up player inputs and slider values
    /// </summary>
    private void Start()
    {
        PlayerController.Instance.PlayerControls.BasicControls.PauseGame.performed += PauseGamePerformed;
        _tabbedMenu = TabbedMenu.Instance;

        _settingsManager = SettingsManager.Instance;
        if (_settingsManager != null)
        {
            _mouseSensSlider.value = _settingsManager.MouseSensitivity;
            _masterVolSlider.value = _settingsManager.MasterVolume;
            _musicVolSlider.value = _settingsManager.MusicVolume;
            _sfxVolSlider.value = _settingsManager.SfxVolume;
        }
    }

    /// <summary>
    /// Unregistering callbacks and player inputs
    /// </summary>
    private void OnDisable()
    {
        InputUser.onChange -= UpdateFocusOnInputChange;

        // Unregistering button callbacks
        _continueButton.UnregisterCallback<NavigationSubmitEvent>(ContinuePressed);
        _settingsButton.UnregisterCallback<NavigationSubmitEvent>(SettingsButtonClicked);
        _audioButton.UnregisterCallback<NavigationSubmitEvent>(AudioButtonClicked);
        _controlsButton.UnregisterCallback<NavigationSubmitEvent>(ControlsButtonClicked);
        _exitButton.UnregisterCallback<NavigationSubmitEvent>(ExitToMenu);
        _exitButton.UnregisterCallback<NavigationSubmitEvent>(PlayConfirmSound);

        _continueButton.UnregisterCallback<MouseOverEvent>(evt => { ChangeButtonFocus(_continueButton); });
        _audioButton.UnregisterCallback<MouseOverEvent>(evt => { ChangeButtonFocus(_audioButton); });
        _controlsButton.UnregisterCallback<MouseOverEvent>(evt => { ChangeButtonFocus(_controlsButton); });
        _settingsButton.UnregisterCallback<MouseOverEvent>(evt => { ChangeButtonFocus(_settingsButton); });
        _exitButton.UnregisterCallback<MouseOverEvent>(evt => { ChangeButtonFocus(_exitButton); });

        _continueButton.UnregisterCallback<MouseOutEvent>(evt => { ClearButtonFocus(); });
        _audioButton.UnregisterCallback<MouseOutEvent>(evt => { ClearButtonFocus(); });
        _controlsButton.UnregisterCallback<MouseOutEvent>(evt => { ClearButtonFocus(); });
        _settingsButton.UnregisterCallback<MouseOutEvent>(evt => { ClearButtonFocus(); });
        _exitButton.UnregisterCallback<MouseOutEvent>(evt => { ClearButtonFocus(); });

        _allButtons.ForEach(button =>
        {
            button.UnregisterCallback<NavigationSubmitEvent>(PlayConfirmSound);
        });
        
        // Unregistering animated tab related callbacks
        _continueButton.UnregisterCallback<MouseOverEvent>(evt => { AnimateTab(_topTab, true); });
        _continueButton.UnregisterCallback<MouseOutEvent>(evt => { AnimateTab(_topTab, false); });
        _audioButton.UnregisterCallback<MouseOverEvent>(evt => { AnimateTab(_topTab, true); });
        _audioButton.UnregisterCallback<MouseOutEvent>(evt => { AnimateTab(_topTab, false); });
        _controlsButton.UnregisterCallback<MouseOverEvent>(evt => { AnimateTab(_middleTab, true); });
        _controlsButton.UnregisterCallback<MouseOutEvent>(evt => { AnimateTab(_middleTab, false); });
        _settingsButton.UnregisterCallback<MouseOverEvent>(evt => { AnimateTab(_middleTab, true); });
        _settingsButton.UnregisterCallback<MouseOutEvent>(evt => { AnimateTab(_middleTab, false); });
        _exitButton.UnregisterCallback<MouseOverEvent>(evt => { AnimateTab(_bottomTab, true); });
        _exitButton.UnregisterCallback<MouseOutEvent>(evt => { AnimateTab(_bottomTab, false); });

        _continueButton.UnregisterCallback<FocusInEvent>(evt => { AnimateTab(_topTab, true); });
        _continueButton.UnregisterCallback<FocusOutEvent>(evt => { AnimateTab(_topTab, false); });
        _audioButton.UnregisterCallback<FocusInEvent>(evt => { AnimateTab(_topTab, true); });
        _audioButton.UnregisterCallback<FocusOutEvent>(evt => { AnimateTab(_topTab, false); });
        _controlsButton.UnregisterCallback<FocusInEvent>(evt => { AnimateTab(_middleTab, true); });
        _controlsButton.UnregisterCallback<FocusOutEvent>(evt => { AnimateTab(_middleTab, false); });
        _settingsButton.UnregisterCallback<FocusInEvent>(evt => { AnimateTab(_middleTab, true); });
        _settingsButton.UnregisterCallback<FocusOutEvent>(evt => { AnimateTab(_middleTab, false); });
        _exitButton.UnregisterCallback<FocusInEvent>(evt => { AnimateTab(_bottomTab, true); });
        _exitButton.UnregisterCallback<FocusOutEvent>(evt => { AnimateTab(_bottomTab, false); });

        PlayerController.Instance.PlayerControls.BasicControls.PauseGame.performed -= PauseGamePerformed;
        _sliders[0].UnregisterCallback<ChangeEvent<float>>(MasterAudioSliderChanged);
        _sliders[1].UnregisterCallback<ChangeEvent<float>>(SFXAudioSliderChanged);
        _sliders[2].UnregisterCallback<ChangeEvent<float>>(MusicAudioSliderChanged);
    }

    /// <summary>
    /// Toggles pause menu UI
    /// </summary>
    /// <param name="isActive">True if menu should be visible</param>
    public void TogglePauseMenu(bool isActive)
    {
        _isGamePaused = isActive;

        if (isActive)
            _continueButton.Focus();
        
        // Ensures mouse is still visible when pausing during dialogue
        if (_tabbedMenu != null && _tabbedMenu.DialogueVisible)
        {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            UnityEngine.Cursor.visible = isActive;
            UnityEngine.Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        }

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
            if (_settingsManager != null)
            {
                _settingsManager.SetMouseSensitivity(_mouseSensSlider.value);
                _settingsManager.SetVolumeValues(_masterVolSlider.value, _musicVolSlider.value, _sfxVolSlider.value);
            }
            _audioHolder.style.display = DisplayStyle.None;
            _controlsHolder.style.display = DisplayStyle.None;
            _selectionHolder.style.display = DisplayStyle.Flex;
        }
    }

    /// <summary>
    /// Clears button focus when mouse stops hovering over a button
    /// </summary>
    private void UpdateFocusOnInputChange(InputUser user, InputUserChange change, InputDevice device)
    {
        if (device is Gamepad)
        {
            EventSystem.current.SetSelectedGameObject(_lastFocusedElement);
            //_lastFocusedVisualElement.Focus();

            if (_currentScreenIndex == 0)
                _continueButton.Focus();
            else if (_currentScreenIndex == 1)
                _audioButton.Focus();
        }
        else if (device is Keyboard)
        {
            _lastFocusedElement = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    /// <summary>
    /// Called when mouse leaves a button
    /// </summary>
    private void ClearButtonFocus()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// Called when the mouse hovers over a button after using a controller
    /// to change what button is focused
    /// </summary>
    private void ChangeButtonFocus(Button buttonToFocus)
    {
        buttonToFocus.Focus();
        _lastFocusedVisualElement = buttonToFocus;
    }

    /// <summary>
    /// Invoked when continue button is pressed to disable menu
    /// </summary>
    /// <param name="click">NavigationSubmitEvent from button</param>
    private void ContinuePressed(NavigationSubmitEvent click)
    {
        TogglePauseMenu(false);
    }

    /// <summary>
    /// Opens settings selection menu
    /// </summary>
    /// <param name="clicked">Click event</param>
    private void SettingsButtonClicked(NavigationSubmitEvent clicked)
    {
        _currentScreenIndex = 1;
        _pauseHolder.style.display = DisplayStyle.None;
        _selectionHolder.style.display = DisplayStyle.Flex;
    }

    /// <summary>
    /// Opens audio options submenu
    /// </summary>
    /// <param name="clicked">Click event</param>
    private void AudioButtonClicked(NavigationSubmitEvent clicked)
    {
        _currentScreenIndex = 2;
        if (_settingsManager != null)
        {
            _masterVolSlider.value = _settingsManager.MasterVolume;
            _musicVolSlider.value = _settingsManager.MusicVolume;
            _sfxVolSlider.value = _settingsManager.SfxVolume;
        }
        _selectionHolder.style.display = DisplayStyle.None;
        _audioHolder.style.display = DisplayStyle.Flex;
    }

    /// <summary>
    /// Opens controls options submenu
    /// </summary>
    /// <param name="clicked">Click event</param>
    private void ControlsButtonClicked(NavigationSubmitEvent clicked)
    {
        _currentScreenIndex = 2;
        if (_settingsManager != null)
        {
            _mouseSensSlider.value = _settingsManager.MouseSensitivity;
        }
        _selectionHolder.style.display = DisplayStyle.None;
        _controlsHolder.style.display = DisplayStyle.Flex;
    }

    /// <summary>
    /// Invoked when main menu button is pressed to load that scene
    /// </summary>
    /// <param name="click">NavigationSubmitEvent from button</param>
    private void ExitToMenu(NavigationSubmitEvent click)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    #region TabAnimationFunctions
    /// <summary>
    /// Called when hovering over or off of a main menu button to start an animation
    /// </summary>
    /// <param name="tabToAnimate">Tab that needs to animate</param>
    /// <param name="isActive">True if tab should move on screen</param>
    /// <param name="extendNGButton">Normally false, true when extending new game tab out for confirmation</param>
    private void AnimateTab(VisualElement tabToAnimate, bool isActive)
    {
        if (_activeCoroutine != null && !isActive)
        {
            StopCoroutine(_activeCoroutine);
        }

        if (isActive)
        {
            _activeCoroutine = StartCoroutine(ScaleTabWidth(tabToAnimate, 322f, (float)tabToAnimate.resolvedStyle.width));
        }
        else
        {
            _activeCoroutine = StartCoroutine(ScaleTabWidth(tabToAnimate, 0f, (float)tabToAnimate.resolvedStyle.width));
        }
    }

    /// <summary>
    /// Lerps a tab's width over a period of time
    /// </summary>
    /// <param name="tabToAnimate">Tab being animated</param>
    /// <param name="targetWidth">Width to reach</param>
    /// <param name="startingWidth">The starting width of the tab</param>
    /// <returns></returns>
    private IEnumerator ScaleTabWidth(VisualElement tabToAnimate, float targetWidth, float startingWidth)
    {
        float elapsedTime = 0f;
        float lerpingTime;

        while (elapsedTime < _tabAnimationTime)
        {
            lerpingTime = elapsedTime / _tabAnimationTime;
            tabToAnimate.style.width = Mathf.Lerp(startingWidth, targetWidth, lerpingTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        tabToAnimate.style.width = targetWidth;
    }
    
    private void MasterAudioSliderChanged(ChangeEvent<float> evt)
    {
        //0-100 value expected.
        var newVolume = evt.newValue;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MasterVolume", newVolume);
    }

    private void SFXAudioSliderChanged(ChangeEvent<float> evt)
    {
        //0-100 value expected.
        var newVolume = evt.newValue;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("SFXVolume", newVolume);
    }

    private void MusicAudioSliderChanged(ChangeEvent<float> evt)
    {
        //0-100 value expected.
        var newVolume = evt.newValue;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MusicVolume", newVolume);
    }
    #endregion
}
