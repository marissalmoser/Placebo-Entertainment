/******************************************************************
*    Author: Alec Pizziferro
*    Contributors: Nullptr
*    Date Created: 5/20/2024
*    Description: A menu controller script that contains a minimap and schedule.
*******************************************************************/

using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace PlaceboEntertainment.UI
{
    /// <summary>
    /// Menu controller for the tabbed UI system containing the schedule and the map.
    /// </summary>
    public class TabbedMenu : MonoBehaviour
    {
        [SerializeField] private UIDocument tabMenu;
        [SerializeField] private UIDocument interactPromptMenu;
        [SerializeField] private UIDocument notificationPopupMenu;
        [Tooltip("The player object to base position & rotation off of for the mini-map.")]
        [SerializeField] private Transform playerTransform;
        [Tooltip("Rotation offset of the player's y-axis euler angles.")]
        [SerializeField] private float miniMapIndicatorEulerOffset = -90f;
        [Tooltip("Boundaries for the dimensions of the map.")]
        [SerializeField] private Transform mapBoundaryUpperLeft,
            mapBoundaryUpperRight,
            mapBoundaryLowerLeft,
            mapBoundaryLowerRight;
        private VisualElement _tabMenuRoot;
        private VisualElement _playerObject;
        private Label _interactText;
        private const string TalkPromptName = "TalkPrompt";
        private const string TabClassName = "tab";
        private const string SelectedTabClassName = "currentlySelectedTab";
        private const string UnSelectedTabClassName = "currentlyUnSelectedTab";
        private const string TabNameSuffix = "Tab";
        private const string ContentNameSuffix = "Content";
        private const string HideClassName = "unselectedContent";
        private const string PlayerName = "Player";

        /// <summary>
        /// Visualizes the boundaries of the world space map.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (!mapBoundaryLowerLeft || !mapBoundaryLowerRight || !mapBoundaryUpperLeft ||
                !mapBoundaryUpperRight) return;
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(mapBoundaryUpperLeft.position, mapBoundaryUpperRight.position);
            Gizmos.DrawLine(mapBoundaryUpperRight.position, mapBoundaryLowerRight.position);
            Gizmos.DrawLine(mapBoundaryLowerRight.position, mapBoundaryLowerLeft.position);
            Gizmos.DrawLine(mapBoundaryLowerLeft.position, mapBoundaryUpperLeft.position);
        }

        /// <summary>
        /// Grabs the UI document and player object.
        /// </summary>
        private void Awake()
        {
            _tabMenuRoot = tabMenu.rootVisualElement;
            _playerObject = _tabMenuRoot.Q(PlayerName);
            _interactText = interactPromptMenu.rootVisualElement.Q<Label>(TalkPromptName);
        }

        /// <summary>
        /// Invokes the register callbacks function.
        /// </summary>
        private void OnEnable()
        {
            RegisterTabCallbacks();
            //note(alec): unity recommends setting their styles to hide rather than enabling/disabling the behavior.
            //stupid IMO but this way the callbacks stay registered.
            tabMenu.rootVisualElement.style.display = DisplayStyle.None;
            interactPromptMenu.rootVisualElement.style.display = DisplayStyle.None;
            notificationPopupMenu.rootVisualElement.style.display = DisplayStyle.None;
        }

        /// <summary>
        /// Invokes the un-register callbacks function
        /// </summary>
        private void OnDisable()
        {
            UnRegisterTabCallbacks();
        }

        /// <summary>
        /// Unity update method. Invokes updating the mini map.
        /// </summary>
        private void Update()
        {
            UpdateMinimapPlayer();
        }

        /// <summary>
        /// Positions the player icon on the mini map.
        /// </summary>
        private void UpdateMinimapPlayer()
        {
            if (!mapBoundaryLowerLeft || !mapBoundaryLowerRight || !mapBoundaryUpperLeft ||
                !mapBoundaryUpperRight || _playerObject == null || !playerTransform) return;
            //get a 0-1 value along the x axis
            Vector3 playerPos = playerTransform.position;
            float xAxis = Mathf.InverseLerp(mapBoundaryUpperLeft.position.x,
                mapBoundaryUpperRight.position.x, playerPos.x);
            //get a 0-1 value along the y axis
            float yAxis = Mathf.InverseLerp(mapBoundaryUpperLeft.position.z, mapBoundaryLowerLeft.position.z,
                playerPos.z);
            var parent = _playerObject.parent;
            //map position = width of element * % of each axis. rotation is just the vertical rotation of the player.
            _playerObject.style.translate =
                new Translate(xAxis * parent.resolvedStyle.width, yAxis * parent.resolvedStyle.height);
            _playerObject.style.rotate = new Rotate(Angle.Degrees(playerTransform.eulerAngles.y + miniMapIndicatorEulerOffset));
        }

        /// <summary>
        /// Registers the click callback of tabs to the tabOnClick method.
        /// </summary>
        private void RegisterTabCallbacks()
        {
            UQueryBuilder<Label> tabs = GetAllTabs();
            tabs.ForEach(tab => { tab.RegisterCallback<ClickEvent>(TabOnClick); });
        }
        
        /// <summary>
        /// Un-Registers the click callback of tabs to the tabOnClick method.
        /// </summary>
        private void UnRegisterTabCallbacks()
        {
            UQueryBuilder<Label> tabs = GetAllTabs();
            tabs.ForEach(tab => { tab.UnregisterCallback<ClickEvent>(TabOnClick); });
        }

        /// <summary>
        /// Fired when the tab is clicked. Will switch selected tab if possible.
        /// </summary>
        /// <param name="evt">The tab that was clicked on.</param>
        private void TabOnClick(ClickEvent evt)
        {
            Label clickedTab = evt.currentTarget as Label;
            if (!TabIsCurrentlySelected(clickedTab)) //tab is not currently selectd, change it.
            {
                //set all selected tabs to be unselected.
                GetAllTabs().Where(
                        (tab) =>
                            tab != clickedTab && TabIsCurrentlySelected(tab))
                    .ForEach(UnSelectTab);
                SelectTab(clickedTab);
            }
        }

        /// <summary>
        /// A tab is selected if it contains the selected USS class.
        /// </summary>
        /// <param name="tab">The tab to check.</param>
        /// <returns>True if the tab is selected.</returns>
        private static bool TabIsCurrentlySelected(Label tab)
        {
            return tab.ClassListContains(SelectedTabClassName);
        }

        /// <summary>
        /// Gets all labels that contain the tab USS class.
        /// </summary>
        /// <returns>A UQueryBuilder containing all found tabs.</returns>
        private UQueryBuilder<Label> GetAllTabs()
        {
            return _tabMenuRoot.Query<Label>(className: TabClassName);
        }

        /// <summary>
        /// De-selects the current tab by changing its USS classes that are active.
        /// Also disables the content associated with it.
        /// </summary>
        /// <param name="tab">The tab to de-select.</param>
        private void UnSelectTab(Label tab)
        {
            tab.RemoveFromClassList(SelectedTabClassName);
            tab.AddToClassList(UnSelectedTabClassName);
            VisualElement content = FindContent(tab);
            content.AddToClassList(HideClassName);
        }

        /// <summary>
        /// Selects the new tab by changing its USS classes that are active.
        /// Enables the content associated with it.
        /// </summary>
        /// <param name="tab"></param>
        private void SelectTab(Label tab)
        {
            tab.RemoveFromClassList(UnSelectedTabClassName);
            tab.AddToClassList(SelectedTabClassName);
            VisualElement content = FindContent(tab);
            content.RemoveFromClassList(HideClassName);
        }

        /// <summary>
        /// Finds the content associated with the tab.
        /// </summary>
        /// <param name="tab">The tab to search via.</param>
        /// <returns>A visual element containing the tab's content. Can be an empty visual element.</returns>
        private VisualElement FindContent(Label tab)
        {
            return _tabMenuRoot.Q(ContentName(tab));
        }

        /// <summary>
        /// Creates a string that represents the tabs content so we don't have to keep a dictionary
        /// of tabs and their respective contents. This works by replacing the suffix of "Tab" to "Content"
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        private static string ContentName(Label tab)
        {
            return tab.name.Replace(TabNameSuffix, ContentNameSuffix);
        }

        /// <summary>
        /// Enables or disables the schedule menu.
        /// </summary>
        /// <param name="show">Whether or not to show the menu.</param>
        public void ToggleSchedule(bool show)
        {
            if (tabMenu == null) return;
            tabMenu.rootVisualElement.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }

        [ContextMenu("ShowSchedule")]
        private void ShowSchedule() => ToggleSchedule(true);
        [ContextMenu("HideSchedule")]
        private void HideSchedule() => ToggleSchedule(false);
        
        /// <summary>
        /// Enables or disables the interact prompt.
        /// Allows setting the text of the prompt.
        /// </summary>
        /// <param name="show">Whether or not to show the prompt.</param>
        /// <param name="text">The text to set the prompt as. Defaults to "TALK"</param>
        public void ToggleInteractPrompt(bool show, string text = "TALK")
        {
            if (interactPromptMenu == null) return;
            if (show)
            {
                _interactText.text = text;
            }
            interactPromptMenu.rootVisualElement.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }

        /// <summary>
        /// Enables or disables the schedule updated notification.
        /// </summary>
        /// <param name="show">Whether or not to show the pop-up.</param>
        public void ToggleScheduleNotification(bool show)
        {
            if (notificationPopupMenu == null) return;
            notificationPopupMenu.rootVisualElement.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}