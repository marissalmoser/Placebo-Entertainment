/******************************************************************
 *    Author: Nick Grinstead
 *    Contributors: 
 *    Date Created: 7/11/2024
 *    Description: A menu controller script for the main menu and its various functions.
 *******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UIDocument _mainMenuDoc;
    [SerializeField] private int _introVideoBuildIndex;
    [SerializeField] private SaveLoadManager _savingManager;
    [SerializeField] private float _tabAnimationTime;

    #region Constants
    private const string NewGameButtonName = "NewGameButton";
    private const string ContinueButtonName = "ContinueButton";
    private const string SettingsButtonName = "SettingsButton";
    private const string QuitButtonName = "QuitButton";
    private const string ConfirmNoButtonName = "ConfirmNoButton";
    private const string ConfirmYesButtonName = "ConfirmYesButton";
    private const string ConfirmationTextName = "ProceedText";
    private const string AudioButtonName = "AudioButton";
    private const string ControlsButtonName = "ControlsButton";
    private const string SplashScreenName = "SplashScreenHolder";
    private const string MainScreenName = "MainMenuHolder";
    private const string MainButtonsHolderName = "MainButtonHolder";
    private const string SettingsSelectionName = "SettingsSelectionHolder";
    private const string SettingsBackPromptName = "SelectionBackPrompt";
    private const string ControlsScreenName = "ControlsHolder";
    private const string AudioScreenName = "AudioHolder";
    private const string NewGameTabName = "NewGameTab";
    private const string ContinueTabName = "ContinueTab";
    private const string SettingsTabName = "SettingsTab";
    private const string QuitTabName = "QuitTab";
    private const string MouseSensSliderName = "MouseSensSlider";
    private const string MasterSliderName = "MasterSlider";
    private const string MusicSliderName = "MusicSlider";
    private const string SfxSliderName = "SfxSlider";
    #endregion

    #region Private
    private Button _newGameButton;
    private Button _continueButton;
    private Button _settingsButton;
    private Button _quitButton;
    private Button _confirmNoButton;
    private Button _confirmYesButton;
    private Label _confirmText;
    private Button _controlsButton;
    private Button _audioButton;
    private VisualElement _splashScreen;
    private VisualElement _mainMenuScreen;
    private VisualElement _mainButtonHolder;
    private VisualElement _controlsScreen;
    private VisualElement _audioScreen;
    private VisualElement _settingsSelectionHolder;
    private VisualElement _settingsBackPrompt;
    private VisualElement _newGameTab;
    private VisualElement _continueTab;
    private VisualElement _settingsTab;
    private VisualElement _quitTab;

    private Coroutine _activeCoroutine;
    private bool _canAnimateTabs = true;

    // 0 = splash, 1 = main, 2 = settings selection, 3 = settings submenu
    private int _currentScreenIndex = 0;

    private PlayerControls _playerControls;
    private InputAction _startGame;
    private InputAction _backInput;

    private List<Slider> _sliders = new List<Slider>();
    private List<VisualElement> _defaultDraggers;
    private List<VisualElement> _newDraggers = new List<VisualElement>();
    #endregion

    #region ButtonCallbacks
    /// <summary>
    /// Registering button callbacks and finding visual elements
    /// </summary>
    private void Awake()
    {
        _playerControls = new PlayerControls();
        _playerControls.BasicControls.Enable();
        _startGame = _playerControls.FindAction("StartGame");
        _backInput = _playerControls.FindAction("PauseGame");
        _startGame.performed += ctx => CloseSplashScreen();
        _backInput.performed += ctx => BackButtonClicked();

        _splashScreen = _mainMenuDoc.rootVisualElement.Q(SplashScreenName);
        _mainMenuScreen = _mainMenuDoc.rootVisualElement.Q(MainScreenName);
        _mainButtonHolder = _mainMenuDoc.rootVisualElement.Q(MainButtonsHolderName);
        _settingsSelectionHolder = _mainMenuDoc.rootVisualElement.Q(SettingsSelectionName);
        _settingsBackPrompt = _mainMenuDoc.rootVisualElement.Q(SettingsBackPromptName);
        _controlsScreen = _mainMenuDoc.rootVisualElement.Q(ControlsScreenName);
        _audioScreen = _mainMenuDoc.rootVisualElement.Q(AudioScreenName);

        _newGameButton = _mainMenuDoc.rootVisualElement.Q<Button>(NewGameButtonName);
        _settingsButton = _mainMenuDoc.rootVisualElement.Q<Button>(SettingsButtonName);
        _continueButton = _mainMenuDoc.rootVisualElement.Q<Button>(ContinueButtonName);
        _quitButton = _mainMenuDoc.rootVisualElement.Q<Button>(QuitButtonName);
        _confirmNoButton = _mainMenuDoc.rootVisualElement.Q<Button>(ConfirmNoButtonName);
        _confirmYesButton = _mainMenuDoc.rootVisualElement.Q<Button>(ConfirmYesButtonName);
        _confirmText = _mainMenuDoc.rootVisualElement.Q<Label>(ConfirmationTextName);
        _audioButton = _mainMenuDoc.rootVisualElement.Q<Button>(AudioButtonName);
        _controlsButton = _mainMenuDoc.rootVisualElement.Q<Button>(ControlsButtonName);

        _newGameTab = _mainMenuDoc.rootVisualElement.Q(NewGameTabName);
        _continueTab = _mainMenuDoc.rootVisualElement.Q(ContinueTabName);
        _settingsTab = _mainMenuDoc.rootVisualElement.Q(SettingsTabName);
        _quitTab = _mainMenuDoc.rootVisualElement.Q(QuitTabName);

        _newGameButton.RegisterCallback<ClickEvent>(NewGameButtonClicked);
        _settingsButton.RegisterCallback<ClickEvent>(SettingsButtonClicked);
        _continueButton.RegisterCallback<ClickEvent>(ContinueButtonClicked);
        _quitButton.RegisterCallback<ClickEvent>(QuitButtonClicked);
        _confirmNoButton.RegisterCallback<ClickEvent>(ConfirmNoButtonClicked);
        _confirmYesButton.RegisterCallback<ClickEvent>(StartNewGame);
        _audioButton.RegisterCallback<ClickEvent>(AudioButtonClicked);
        _controlsButton.RegisterCallback<ClickEvent>(ControlsButtonClicked);

        _newGameButton.RegisterCallback<MouseOverEvent>(evt => { AnimateTab(_newGameTab, true); });
        _newGameButton.RegisterCallback<MouseOutEvent>(evt => { AnimateTab(_newGameTab, false); });
        _audioButton.RegisterCallback<MouseOverEvent>(evt => { AnimateTab(_settingsTab, true); });
        _audioButton.RegisterCallback<MouseOutEvent>(evt => { AnimateTab(_settingsTab, false); });
        _controlsButton.RegisterCallback<MouseOverEvent>(evt => { AnimateTab(_quitTab, true); });
        _controlsButton.RegisterCallback<MouseOutEvent>(evt => { AnimateTab(_quitTab, false); });

        // Makes continue button visible if saved data exists
        if (_savingManager != null && _savingManager.DoesSaveFileExist())
        {
            _continueButton.style.display = DisplayStyle.Flex;

            _continueButton.RegisterCallback<MouseOverEvent>(evt => { AnimateTab(_continueTab, true); });
            _continueButton.RegisterCallback<MouseOutEvent>(evt => { AnimateTab(_continueTab, false); });
            _settingsButton.RegisterCallback<MouseOverEvent>(evt => { AnimateTab(_settingsTab, true); });
            _settingsButton.RegisterCallback<MouseOutEvent>(evt => { AnimateTab(_settingsTab, false); });
            _quitButton.RegisterCallback<MouseOverEvent>(evt => { AnimateTab(_quitTab, true); });
            _quitButton.RegisterCallback<MouseOutEvent>(evt => { AnimateTab(_quitTab, false); });
        }
        else
        {
            // Using different tabs to account for the lack of a continue button
            _continueButton.style.display = DisplayStyle.None;

            _settingsButton.RegisterCallback<MouseOverEvent>(evt => { AnimateTab(_continueTab, true); });
            _settingsButton.RegisterCallback<MouseOutEvent>(evt => { AnimateTab(_continueTab, false); });
            _quitButton.RegisterCallback<MouseOverEvent>(evt => { AnimateTab(_settingsTab, true); });
            _quitButton.RegisterCallback<MouseOutEvent>(evt => { AnimateTab(_settingsTab, false); });
        }
    }

    /// <summary>
    /// Unregistering button callbacks
    /// </summary>
    private void OnDisable()
    {
        _startGame.performed -= ctx => CloseSplashScreen();
        _backInput.performed -= ctx => BackButtonClicked();

        _newGameButton.UnregisterCallback<ClickEvent>(NewGameButtonClicked);
        _continueButton.UnregisterCallback<ClickEvent>(ContinueButtonClicked);
        _settingsButton.UnregisterCallback<ClickEvent>(SettingsButtonClicked);
        _quitButton.UnregisterCallback<ClickEvent>(QuitButtonClicked);
        _confirmNoButton.UnregisterCallback<ClickEvent>(ConfirmNoButtonClicked);
        _confirmYesButton.UnregisterCallback<ClickEvent>(StartNewGame);
        _audioButton.UnregisterCallback<ClickEvent>(AudioButtonClicked);
        _controlsButton.UnregisterCallback<ClickEvent>(ControlsButtonClicked);

        _newGameButton.UnregisterCallback<MouseOverEvent>(evt => { AnimateTab(_newGameTab, true); });
        _newGameButton.UnregisterCallback<MouseOutEvent>(evt => { AnimateTab(_newGameTab, false); });
        _audioButton.UnregisterCallback<MouseOverEvent>(evt => { AnimateTab(_settingsTab, true); });
        _audioButton.UnregisterCallback<MouseOutEvent>(evt => { AnimateTab(_settingsTab, false); });
        _controlsButton.UnregisterCallback<MouseOverEvent>(evt => { AnimateTab(_quitTab, true); });
        _controlsButton.UnregisterCallback<MouseOutEvent>(evt => { AnimateTab(_quitTab, false); });

        if (_savingManager != null && _savingManager.DoesSaveFileExist())
        {
            _continueButton.UnregisterCallback<MouseOverEvent>(evt => { AnimateTab(_continueTab, true); });
            _continueButton.UnregisterCallback<MouseOutEvent>(evt => { AnimateTab(_continueTab, false); });
            _settingsButton.UnregisterCallback<MouseOverEvent>(evt => { AnimateTab(_settingsTab, true); });
            _settingsButton.UnregisterCallback<MouseOutEvent>(evt => { AnimateTab(_settingsTab, false); });
            _quitButton.UnregisterCallback<MouseOverEvent>(evt => { AnimateTab(_quitTab, true); });
            _quitButton.UnregisterCallback<MouseOutEvent>(evt => { AnimateTab(_quitTab, false); });
        }
        else
        {
            _settingsButton.UnregisterCallback<MouseOverEvent>(evt => { AnimateTab(_continueTab, true); });
            _settingsButton.UnregisterCallback<MouseOutEvent>(evt => { AnimateTab(_continueTab, false); });
            _quitButton.UnregisterCallback<MouseOverEvent>(evt => { AnimateTab(_settingsTab, true); });
            _quitButton.UnregisterCallback<MouseOutEvent>(evt => { AnimateTab(_settingsTab, false); });
        }
    }
    #endregion

    #region ButtonFunctions
    /// <summary>
    /// Closes the splash screen when enter is pressed
    /// </summary>
    private void CloseSplashScreen()
    {
        if (_currentScreenIndex == 0)
        {
            _currentScreenIndex = 1;
            _startGame.performed -= ctx => CloseSplashScreen();
            _splashScreen.style.display = DisplayStyle.None;
            _mainMenuScreen.style.display = DisplayStyle.Flex;
        }
    }

    /// <summary>
    /// Loads intro cutscene
    /// </summary>
    /// <param name="clicked">Click event</param>
    private void ContinueButtonClicked(ClickEvent clicked)
    {
        SceneManager.LoadScene(_introVideoBuildIndex);
    }

    /// <summary>
    /// Opens settings selection menu
    /// </summary>
    /// <param name="clicked">Click event</param>
    private void SettingsButtonClicked(ClickEvent clicked)
    {
        _currentScreenIndex = 2;
        _mainButtonHolder.style.display = DisplayStyle.None;
        _settingsSelectionHolder.style.display = DisplayStyle.Flex;
        _settingsBackPrompt.style.display = DisplayStyle.Flex;
    }

    /// <summary>
    /// Opens audio options submenu
    /// </summary>
    /// <param name="clicked">Click event</param>
    private void AudioButtonClicked(ClickEvent clicked)
    {
        _currentScreenIndex = 3;
        _settingsSelectionHolder.style.display = DisplayStyle.None;
        _settingsBackPrompt.style.display = DisplayStyle.None;
        _audioScreen.style.display = DisplayStyle.Flex;
    }

    /// <summary>
    /// Opens controls options submenu
    /// </summary>
    /// <param name="clicked">Click event</param>
    private void ControlsButtonClicked(ClickEvent clicked)
    {
        _currentScreenIndex = 3;
        _settingsSelectionHolder.style.display = DisplayStyle.None;
        _settingsBackPrompt.style.display = DisplayStyle.None;
        _controlsScreen.style.display = DisplayStyle.Flex;
    }

    /// <summary>
    /// Pulls up confirmation UI
    /// </summary>
    /// <param name="clicked">Click event</param>
    private void NewGameButtonClicked(ClickEvent clicked)
    {
        AnimateTab(_newGameTab, true, true);
        _canAnimateTabs = false;

        _continueButton.SetEnabled(false);
        _settingsButton.SetEnabled(false);
        _quitButton.SetEnabled(false);
    }

    /// <summary>
    /// Deletes existing save data and loads the intro scene
    /// </summary>
    /// <param name="clicked">Click event</param>
    private void StartNewGame(ClickEvent clicked)
    {
        if (_savingManager != null)
        {
            _savingManager.DeleteSaveData();
        }

        SceneManager.LoadScene(_introVideoBuildIndex);
    }

    /// <summary>
    /// Pulls up new game confirmation options
    /// </summary>
    /// <param name="clicked">Click event</param>
    private void ConfirmNoButtonClicked(ClickEvent clicked)
    {
        _canAnimateTabs = true;
        AnimateTab(_newGameTab, false);

        _confirmText.style.display = DisplayStyle.None;
        _confirmNoButton.style.display = DisplayStyle.None;
        _confirmYesButton.style.display = DisplayStyle.None;

        _continueButton.SetEnabled(true);
        _settingsButton.SetEnabled(true);
        _quitButton.SetEnabled(true);
    }

    /// <summary>
    /// Closes application
    /// </summary>
    /// <param name="clicked">Click event</param>
    private void QuitButtonClicked(ClickEvent clicked)
    {
        Application.Quit();
    }

    /// <summary>
    /// Returns to main menu screen from submenu
    /// </summary>
    private void BackButtonClicked()
    {
        if (_currentScreenIndex == 2)
        {
            _currentScreenIndex = 1;
            _settingsSelectionHolder.style.display = DisplayStyle.None;
            _settingsBackPrompt.style.display = DisplayStyle.None;
            _mainButtonHolder.style.display = DisplayStyle.Flex;
        }
        else if (_currentScreenIndex == 3)
        {
            _currentScreenIndex = 2;
            _controlsScreen.style.display = DisplayStyle.None;
            _audioScreen.style.display = DisplayStyle.None;
            _settingsSelectionHolder.style.display = DisplayStyle.Flex;
            _settingsBackPrompt.style.display = DisplayStyle.Flex;
        }
    }
    #endregion

    #region TabAnimationFunctions
    /// <summary>
    /// Called when hovering over or off of a main menu button to start an animation
    /// </summary>
    /// <param name="tabToAnimate">Tab that needs to animate</param>
    /// <param name="isActive">True if tab should move on screen</param>
    /// <param name="extendNGButton">Normally false, true when extending new game tab out for confirmation</param>
    private void AnimateTab(VisualElement tabToAnimate, bool isActive, bool extendNGButton = false)
    {
        if (_canAnimateTabs)
        {
            if (_activeCoroutine != null && !isActive)
            {
                StopCoroutine(_activeCoroutine);
            }

            if (extendNGButton)
            {
                _activeCoroutine = StartCoroutine(ScaleTabWidth(tabToAnimate, 1171f, (float)tabToAnimate.resolvedStyle.width));
            }
            else if (isActive)
            {
                _activeCoroutine = StartCoroutine(ScaleTabWidth(tabToAnimate, 322f, (float)tabToAnimate.resolvedStyle.width));
            }
            else
            {
                _activeCoroutine = StartCoroutine(ScaleTabWidth(tabToAnimate, 0f, (float)tabToAnimate.resolvedStyle.width));
            }
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
        float time;

        while (elapsedTime < _tabAnimationTime)
        {
            time = elapsedTime / _tabAnimationTime;
            tabToAnimate.style.width = Mathf.Lerp(startingWidth, targetWidth, time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tabToAnimate.style.width = targetWidth;
        
        // Displays proceed options if new game tab was extended
        if (targetWidth > 322f)
        {
            _confirmNoButton.style.display = DisplayStyle.Flex;
            _confirmYesButton.style.display = DisplayStyle.Flex;
            _confirmText.style.display = DisplayStyle.Flex;
        }
    }
    #endregion
}
