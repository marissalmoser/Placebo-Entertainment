/******************************************************************
*    Author: Alec Pizziferro
*    Contributors: Nullptr
*    Date Created: 5/16/2024
*    Description: A sample script that showcases how to setup events with UI Toolkit.
*******************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace PlaceboEntertainment.UI
{
    /// <summary>
    /// Sample UI Event script for future reference.
    /// </summary>
    public class SampleUIEvents : MonoBehaviour
    {
        private UIDocument _document;
        private Button _button;
        private const string StartButton = "StartButton";
        private List<Button> _menuButtons = new();
        
        /// <summary>
        /// Finds buttons on the object.
        /// </summary>
        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            
            _button = _document.rootVisualElement.Q(StartButton) as Button;
            _menuButtons = _document.rootVisualElement.Query<Button>().ToList();
        }

        /// <summary>
        /// Assigns callbacks to the start button and every button in the menu.
        /// </summary>
        private void OnEnable()
        {
            _button.RegisterCallback<ClickEvent>(OnPlayGameClick);
            foreach (var button in _menuButtons)
            {
                button.RegisterCallback<ClickEvent>(OnAllButtonClick);
            }
        }

        /// <summary>
        /// Un-assigns callbacks to the start button and every button in the menu.
        /// </summary>
        private void OnDisable()
        {
            _button.UnregisterCallback<ClickEvent>(OnPlayGameClick);
            foreach (var button in _menuButtons)
            {
                button.UnregisterCallback<ClickEvent>(OnAllButtonClick);
            }
        }

        /// <summary>
        /// Called when the start game button is clicked.
        /// </summary>
        /// <param name="evt">The click event that was fired.</param>
        private void OnPlayGameClick(ClickEvent evt)
        {
            Debug.Log("You hit Start Game!");
        }

        /// <summary>
        /// Called when any button is pressed.
        /// </summary>
        /// <param name="evt">The click event that was fired.</param>
        private void OnAllButtonClick(ClickEvent evt)
        {
            Debug.Log("Clicked any button.");
        }

        /// <summary>
        /// Tries to create a button in the list, just an experiment to see how runtime additions work.
        /// </summary>
        [ContextMenu("Try Duplicate Button")]
        private void TryDuplicateButton()
        {
            var parent = _menuButtons[1].parent;
            var newButton = new Button();
            newButton.text = "New Button";
            parent.Add(newButton);
        }
    }
}