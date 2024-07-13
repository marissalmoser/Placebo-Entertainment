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
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private UIDocument _pauseMenu;

    private const string ContinueButtonName = "ContinueButton";
    private const string ExitButtonName = "ExitButton";

    private Button _continueButton;
    private Button _exitButton;

    private void Awake()
    {
        _pauseMenu.rootVisualElement.style.display = DisplayStyle.None;

        _continueButton = _pauseMenu.rootVisualElement.Q<Button>(ContinueButtonName);
        _exitButton = _pauseMenu.rootVisualElement.Q<Button>(ExitButtonName);

        _continueButton.RegisterCallback<ClickEvent>(ContinuePressed);
        _exitButton.RegisterCallback<ClickEvent>(ExitToMenu);
    }

    private void Start()
    {
        PlayerController.Instance.PlayerControls.BasicControls.PauseGame.performed += PauseGamePerformed;
    }

    private void OnDisable()
    {
        _continueButton.UnregisterCallback<ClickEvent>(ContinuePressed);
        _exitButton.UnregisterCallback<ClickEvent>(ExitToMenu);

        PlayerController.Instance.PlayerControls.BasicControls.PauseGame.performed -= PauseGamePerformed;
    }

    public void TogglePauseMenu(bool isActive)
    {
        UnityEngine.Cursor.visible = isActive;
        UnityEngine.Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;

        _pauseMenu.rootVisualElement.style.display = isActive ? DisplayStyle.Flex : DisplayStyle.None;
        Time.timeScale = isActive ? 0 : 1;
    }

    private void PauseGamePerformed(InputAction.CallbackContext obj)
    {
        TogglePauseMenu(true);
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
