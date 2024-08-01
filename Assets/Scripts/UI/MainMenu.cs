/******************************************************************
 *    Author: Nick Grinstead
 *    Contributors: 
 *    Date Created: 7/11/2024
 *    Description: A menu controller script for the main menu and its buttons.
 *******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UIDocument _mainMenuDoc;
    [SerializeField] private int _introVideoBuildIndex;
    [SerializeField] private SaveLoadManager _savingManager;

    #region Constants
    private const string NewGameButtonName = "NewGameButton";
    private const string ContinueButtonName = "ContinueButton";
    private const string SettingsButtonName = "SettingsButton";
    private const string QuitButtonName = "QuitButton";
    private const string SettingsBackButtonName = "SettingsBackButton";
    private const string ConfirmNoButtonName = "ConfirmNoButton";
    private const string ConfirmYesButtonName = "ConfirmYesButton";
    private const string MainScreenName = "MainMenuBackground";
    private const string SettingsScreenName = "SettingsBackground";
    private const string ConfirmationScreenName = "ConfirmationBackground";
    #endregion

    #region Private
    private Button _newGameButton;
    private Button _continueButton;
    private Button _settingsButton;
    private Button _quitButton;
    private Button _settingsBackButton;
    private Button _confirmNoButton;
    private Button _confirmYesButton;
    private VisualElement _mainMenuScreen;
    private VisualElement _settingsScreen;
    private VisualElement _confirmationScreen;
    #endregion

    /// <summary>
    /// Registering button callbacks and finding visual elements
    /// </summary>
    private void Awake()
    {
        _mainMenuScreen = _mainMenuDoc.rootVisualElement.Q(MainScreenName);
        _settingsScreen = _mainMenuDoc.rootVisualElement.Q(SettingsScreenName);
        _confirmationScreen = _mainMenuDoc.rootVisualElement.Q(ConfirmationScreenName);

        _newGameButton = _mainMenuDoc.rootVisualElement.Q<Button>(NewGameButtonName);
        _settingsButton = _mainMenuDoc.rootVisualElement.Q<Button>(SettingsButtonName);
        _continueButton = _mainMenuDoc.rootVisualElement.Q<Button>(ContinueButtonName);
        _quitButton = _mainMenuDoc.rootVisualElement.Q<Button>(QuitButtonName);
        _settingsBackButton = _mainMenuDoc.rootVisualElement.Q<Button>(SettingsBackButtonName);
        _confirmNoButton = _mainMenuDoc.rootVisualElement.Q<Button>(ConfirmNoButtonName);
        _confirmYesButton = _mainMenuDoc.rootVisualElement.Q<Button>(ConfirmYesButtonName);

        _newGameButton.RegisterCallback<ClickEvent>(NewGameButtonClicked);
        _settingsButton.RegisterCallback<ClickEvent>(SettingsButtonClicked);
        _continueButton.RegisterCallback<ClickEvent>(ContinueButtonClicked);
        _quitButton.RegisterCallback<ClickEvent>(QuitButtonClicked);
        _settingsBackButton.RegisterCallback<ClickEvent>(BackButtonClicked);
        _confirmNoButton.RegisterCallback<ClickEvent>(BackButtonClicked);
        _confirmYesButton.RegisterCallback<ClickEvent>(StartNewGame);

        // Makes continue button visible if saved data exists
        if (_savingManager != null && _savingManager.DoesSaveFileExist())
        {
            _continueButton.style.display = DisplayStyle.Flex;
        }
    }

    /// <summary>
    /// Unregistering button callbacks
    /// </summary>
    private void OnDisable()
    {
        _newGameButton.UnregisterCallback<ClickEvent>(NewGameButtonClicked);
        _continueButton.UnregisterCallback<ClickEvent>(ContinueButtonClicked);
        _settingsButton.UnregisterCallback<ClickEvent>(SettingsButtonClicked);
        _quitButton.UnregisterCallback<ClickEvent>(QuitButtonClicked);
        _settingsBackButton.UnregisterCallback<ClickEvent>(BackButtonClicked);
        _confirmNoButton.UnregisterCallback<ClickEvent>(BackButtonClicked);
        _confirmYesButton.UnregisterCallback<ClickEvent>(StartNewGame);
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
    /// Opens settings menu
    /// </summary>
    /// <param name="clicked">Click event</param>
    private void SettingsButtonClicked(ClickEvent clicked)
    {
        _mainMenuScreen.style.display = DisplayStyle.None;
        _settingsScreen.style.display = DisplayStyle.Flex;
    }

    /// <summary>
    /// Pulls up confirmation UI
    /// </summary>
    /// <param name="clicked">Click event</param>
    private void NewGameButtonClicked(ClickEvent clicked)
    {
        _mainMenuScreen.style.display = DisplayStyle.None;
        _confirmationScreen.style.display = DisplayStyle.Flex;
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
    /// <param name="clicked">Click event</param>
    private void BackButtonClicked(ClickEvent clicked)
    {
        _confirmationScreen.style.display = DisplayStyle.None;
        _settingsScreen.style.display = DisplayStyle.None;
        _mainMenuScreen.style.display = DisplayStyle.Flex;
    }
}
