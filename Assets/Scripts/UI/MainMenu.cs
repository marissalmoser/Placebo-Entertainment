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

    #region Constants
    private const string StartButtonName = "StartGameButton";
    private const string SettingsButtonName = "SettingsButton";
    private const string CreditsButtonName = "CreditsButton";
    private const string QuitButtonName = "QuitButton";
    private const string SettingsBackButtonName = "SettingsBackButton";
    private const string CreditsBackButtonName = "CreditsBackButton";
    private const string MainScreenName = "MainMenuBackground";
    private const string SettingsScreenName = "SettingsBackground";
    private const string CreditsScreenName = "CreditsBackground";
    #endregion

    #region Private
    private Button _startButton;
    private Button _settingsButton;
    private Button _creditsButton;
    private Button _quitButton;
    private Button _settingsBackButton;
    private Button _creditsBackButton;
    private VisualElement _mainMenuScreen;
    private VisualElement _settingsScreen;
    private VisualElement _creditsScreen;
    #endregion

    /// <summary>
    /// Registering button callbacks and finding visual elements
    /// </summary>
    private void Awake()
    {
        _mainMenuScreen = _mainMenuDoc.rootVisualElement.Q(MainScreenName);
        _settingsScreen = _mainMenuDoc.rootVisualElement.Q(SettingsScreenName);
        _creditsScreen = _mainMenuDoc.rootVisualElement.Q(CreditsScreenName);

        _startButton = _mainMenuDoc.rootVisualElement.Q<Button>(StartButtonName);
        _settingsButton = _mainMenuDoc.rootVisualElement.Q<Button>(SettingsButtonName);
        _creditsButton = _mainMenuDoc.rootVisualElement.Q<Button>(CreditsButtonName);
        _quitButton = _mainMenuDoc.rootVisualElement.Q<Button>(QuitButtonName);
        _settingsBackButton = _mainMenuDoc.rootVisualElement.Q<Button>(SettingsBackButtonName);
        _creditsBackButton = _mainMenuDoc.rootVisualElement.Q<Button>(CreditsBackButtonName);

        _startButton.RegisterCallback<ClickEvent>(StartButtonClicked);
        _settingsButton.RegisterCallback<ClickEvent>(SettingsButtonClicked);
        _creditsButton.RegisterCallback<ClickEvent>(CreditsButtonClicked);
        _quitButton.RegisterCallback<ClickEvent>(QuitButtonClicked);
        _settingsBackButton.RegisterCallback<ClickEvent>(BackButtonClicked);
        _creditsBackButton.RegisterCallback<ClickEvent>(BackButtonClicked);
    }

    /// <summary>
    /// Unregistering button callbacks
    /// </summary>
    private void OnDisable()
    {
        _startButton.UnregisterCallback<ClickEvent>(StartButtonClicked);
        _settingsButton.UnregisterCallback<ClickEvent>(SettingsButtonClicked);
        _creditsButton.UnregisterCallback<ClickEvent>(CreditsButtonClicked);
        _quitButton.UnregisterCallback<ClickEvent>(QuitButtonClicked);
        _settingsBackButton.UnregisterCallback<ClickEvent>(BackButtonClicked);
        _creditsBackButton.UnregisterCallback<ClickEvent>(BackButtonClicked);
    }

    /// <summary>
    /// Loads game level
    /// </summary>
    /// <param name="clicked">Click event</param>
    private void StartButtonClicked(ClickEvent clicked)
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
    /// Opens credits menu
    /// </summary>
    /// <param name="clicked">Click event</param>
    private void CreditsButtonClicked(ClickEvent clicked)
    {
        _mainMenuScreen.style.display = DisplayStyle.None;
        _creditsScreen.style.display = DisplayStyle.Flex;
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
        _creditsScreen.style.display = DisplayStyle.None;
        _settingsScreen.style.display = DisplayStyle.None;
        _mainMenuScreen.style.display = DisplayStyle.Flex;
    }
}
