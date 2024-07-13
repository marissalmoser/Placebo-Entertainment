/******************************************************************
 *    Author: Nick Grinstead
 *    Contributors:
 *    Date Created: 7/12/2024
 *    Description: 
 *******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private UIDocument _pauseMenu;

    private const string ContinueButtonName = "ContinueButton";
    private const string ExitButtonName = "ExitButton";

    private Button _continueButton;
    private Button _exitButton;

    private void Awake()
    {
        _continueButton = _pauseMenu.rootVisualElement.Q<Button>(ContinueButtonName);
        _exitButton = _pauseMenu.rootVisualElement.Q<Button>(ExitButtonName);

        _continueButton.RegisterCallback<ClickEvent>(ContinuePressed);
        _exitButton.RegisterCallback<ClickEvent>(ExitToMenu);
    }

    private void Start()
    {
        //PlayerController.Instance.PlayerControls.BasicControls.
    }

    private void OnDisable()
    {
        _continueButton.UnregisterCallback<ClickEvent>(ContinuePressed);
        _exitButton.UnregisterCallback<ClickEvent>(ExitToMenu);
    }

    public void TogglePauseMenu(bool isActive)
    {
        _pauseMenu.rootVisualElement.style.display = isActive ? DisplayStyle.Flex : DisplayStyle.None;
        Time.timeScale = isActive ? 0 : 1;
    }

    private void ContinuePressed(ClickEvent click)
    {
        TogglePauseMenu(false);
    }

    private void ExitToMenu(ClickEvent click)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
