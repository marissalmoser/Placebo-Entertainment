using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace PlaceboEntertainment.UI
{
    public class TabbedMenu : MonoBehaviour
    {
        private TabbedMenuController _controller;

        private void OnEnable()
        {
            UIDocument menu = GetComponent<UIDocument>();
            VisualElement root = menu.rootVisualElement;
            _controller = new(root);
            _controller.RegisterTabCallbacks();
        }
    }
}