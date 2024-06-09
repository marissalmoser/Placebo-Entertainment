/******************************************************************
 *    Author: Alec Pizziferro
 *    Contributors: Nullptr
 *    Date Created: 5/20/2024
 *    Description: A menu controller script that contains a minimap and schedule.
 *******************************************************************/

using System;
using System.Collections.Generic;
using UI.Components;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace PlaceboEntertainment.UI
{
    /// <summary>
    /// Menu controller for the tabbed UI system containing the schedule and the map.
    /// </summary>
    public class TabbedMenu : MonoBehaviour
    {
        #region Singleton

        public static TabbedMenu Instance { get; private set; }

        #endregion

        #region Serialized

        [SerializeField] private UIDocument tabMenu;
        [SerializeField] private UIDocument interactPromptMenu;
        [SerializeField] private UIDocument notificationPopupMenu;
        [SerializeField] private UIDocument dialogueMenu;

        [Tooltip("The player object to base position & rotation off of for the mini-map.")] [SerializeField]
        private Transform playerTransform;

        [Tooltip("Rotation offset of the player's y-axis euler angles.")] [SerializeField]
        private float miniMapIndicatorEulerOffset = -90f;

        [Tooltip("Boundaries for the dimensions of the map.")] [SerializeField]
        private Transform mapBoundaryUpperLeft,
            mapBoundaryUpperRight,
            mapBoundaryLowerLeft,
            mapBoundaryLowerRight;

        [SerializeField] private VisualTreeAsset emptyScheduleEntry;
        [SerializeField] private VisualTreeAsset fulfilledScheduleEntry;
        [SerializeField] private VisualTreeAsset dialogueButton;

        #endregion

        #region Public

        /// <summary>
        /// Helper struct for populating entries of the schedule.
        /// </summary>
        public struct ScheduleEntry
        {
            public readonly string TimeStamp;
            public readonly string Description;
            public readonly string ItemName;
            public readonly Texture2D Icon;

            /// <summary>
            /// Constructor for making an entry.
            /// </summary>
            /// <param name="timeStamp">The time of the event in the format "XX:XX"</param>
            /// <param name="description">The description of the event.</param>
            /// <param name="itemName">The name of the item found, can be empty.</param>
            /// <param name="icon">The texture for the icon found, can be null.</param>
            public ScheduleEntry(string timeStamp, string description, string itemName, Texture2D icon)
            {
                TimeStamp = timeStamp ?? string.Empty;
                Description = description ?? string.Empty;
                ItemName = itemName ?? string.Empty;
                Icon = icon;
            }
        }

        #endregion

        #region Private

        private VisualElement _tabMenuRoot;
        private VisualElement _playerObject;
        private Label _interactText;
        private VisualElement _scheduleContainer;
        private VisualElement _dialogueButtonContainer;
        private Dictionary<string, VisualElement> _scheduleEntries = new();
        private bool _scheduleVisible = false;
        private Label _dialogueText;
        private bool _dialogueVisible;
        private AutoFitLabelControl _labelControl;

        #endregion

        #region Constants

        private const string TalkPromptName = "TextPrompt";
        private const string TabClassName = "tab";
        private const string SelectedTabClassName = "currentlySelectedTab";
        private const string UnSelectedTabClassName = "currentlyUnSelectedTab";
        private const string TabNameSuffix = "Tab";
        private const string ContentNameSuffix = "Content";
        private const string HideClassName = "unselectedContent";
        private const string PlayerName = "Player";
        private const string ScheduleContainerName = "ScheduleList";
        private const string ScheduleTimeStampName = "TimeStamp";
        private const string ScheduleDescriptionName = "Description";
        private const string ScheduleItemName = "ItemName";
        private const string ScheduleIconName = "Icon";
        private const string ScheduleEntryName = "ScheduleEntry";
        private const string DialogueOptionContainerName = "DialogueOptionContainer";
        private const string DialogueLabelName = "BottomBar";
        private const string DialogueOptionName = "DialogueOption";

        #endregion

        #region Unity Methods

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
            if (Instance != null && Instance != this)
            {
                Destroy(Instance.gameObject);
            }

            Instance = this;
            _tabMenuRoot = tabMenu.rootVisualElement;
            _playerObject = _tabMenuRoot.Q(PlayerName);
            _interactText = interactPromptMenu.rootVisualElement.Q<Label>(TalkPromptName);
            _scheduleContainer = _tabMenuRoot.Q(ScheduleContainerName);
            _dialogueButtonContainer = dialogueMenu.rootVisualElement.Q(DialogueOptionContainerName);
            _dialogueText = dialogueMenu.rootVisualElement.Q<Label>(DialogueLabelName);
            //auto sizers for the text. Unity does not provide one out of the box...WTF?
            _labelControl = new AutoFitLabelControl(_dialogueText, 35f, 75f);
        }

        /// <summary>
        /// Invokes the register callbacks function.
        /// </summary>
        private void OnEnable()
        {
            RegisterTabCallbacks();
            //note(alec): unity recommends setting their styles to hide rather than enabling/disabling the behavior.
            //stupid IMO but this way the callbacks stay registered.
            dialogueMenu.rootVisualElement.style.display = DisplayStyle.None;
            tabMenu.rootVisualElement.style.display = DisplayStyle.None;
            interactPromptMenu.rootVisualElement.style.display = DisplayStyle.None;
            notificationPopupMenu.rootVisualElement.style.display = DisplayStyle.None;
        }

        /// <summary>
        /// Assigns the performed action of the tab key to opening/closing the schedule UI.
        /// </summary>
        private void Start()
        {
            PlayerController.Instance.PlayerControls.BasicControls.OpenSchedule.performed += OpenScheduleOnPerformed;
        }

        /// <summary>
        /// Invokes the un-register callbacks function
        /// </summary>
        private void OnDisable()
        {
            UnRegisterTabCallbacks();
            PlayerController.Instance.PlayerControls.BasicControls.OpenSchedule.performed -= OpenScheduleOnPerformed;
        }

        /// <summary>
        /// Unity update method. Invokes updating the mini map.
        /// </summary>
        private void Update()
        {
            UpdateMinimapPlayer();
        }

        #endregion

        #region Schedule

        /// <summary>
        /// Callback that gets fired when the tab key is pressed.
        /// </summary>
        /// <param name="obj">Callback context of the key that was presed.</param>
        private void OpenScheduleOnPerformed(InputAction.CallbackContext obj)
        {
            _scheduleVisible = !_scheduleVisible;
            Cursor.visible = _scheduleVisible;
            Cursor.lockState = _scheduleVisible ? CursorLockMode.None : CursorLockMode.Locked;
            ToggleSchedule(_scheduleVisible);
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
            _playerObject.style.rotate =
                new Rotate(Angle.Degrees(playerTransform.eulerAngles.y + miniMapIndicatorEulerOffset));
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
        /// <returns>A string in the format TabNameContent.</returns>
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

        /// <summary>
        /// Adds a schedule entry with the appropriate data, adds it to the schedule, then sorts it.
        /// </summary>
        /// <param name="entry">The entry to add.</param>
        public void AddScheduleEntry(ScheduleEntry entry)
        {
            TryRemoveScheduleItem(entry.TimeStamp);
            var newEntry = fulfilledScheduleEntry.Instantiate();
            newEntry.Q<Label>(ScheduleTimeStampName).text = entry.TimeStamp;
            newEntry.Q<Label>(ScheduleDescriptionName).text = entry.Description;
            newEntry.Q<Label>(ScheduleItemName).text = entry.ItemName;
            newEntry.Q(ScheduleIconName).style.backgroundImage = entry.Icon;
            _scheduleContainer.Add(newEntry);
            _scheduleContainer.Sort(CompareTimeStamps);
            _scheduleEntries.Add(entry.TimeStamp, newEntry);
        }

        /// <summary>
        /// Adds an empty schedule entry.
        /// </summary>
        /// <param name="timeStamp">The time of the entry.</param>
        public void AddEmptyScheduleEntry(string timeStamp)
        {
            TryRemoveScheduleItem(timeStamp);
            var newEntry = emptyScheduleEntry.Instantiate();
            newEntry.Q<Label>(ScheduleTimeStampName).text = timeStamp;
            _scheduleContainer.Add(newEntry);
            _scheduleEntries.Add(timeStamp, newEntry);
        }

        /// <summary>
        /// Attempts to remove a schedule entry with the matching timestamp.
        /// </summary>
        /// <param name="timeStamp">The timestamp of the entry to remove. Works for both empty and filled.</param>
        public void TryRemoveScheduleItem(string timeStamp)
        {
            if (!_scheduleEntries.TryGetValue(timeStamp, out var element)) return;
            _scheduleContainer.Remove(element);
            _scheduleEntries.Remove(timeStamp);
        }

        /// <summary>
        /// Removes all schedule entries. Adds in a single empty entry, could add more if needed.
        /// </summary>
        public void RemoveAllScheduleItems()
        {
            _scheduleContainer.Query(ScheduleEntryName).ForEach(element => { element.parent.Remove(element); });
            //TODO figure out how long the timeline will be and add empty entries to populate
            AddEmptyScheduleEntry("00:00");
        }


        /// <summary>
        /// Comparision function for schedule entries. Sorts by timestamp.
        /// </summary>
        /// <param name="schedule1">The first entry to compare against.</param>
        /// <param name="schedule2">The second entry to compare against.</param>
        /// <returns>A comparision int for the side that is the higher. Returns 0 otherwise.</returns>
        private static int CompareTimeStamps(VisualElement schedule1, VisualElement schedule2)
        {
            //guard cluases for empty comparisons or missing elements
            if (schedule1 == null || schedule2 == null) return 0;
            var label1 = schedule1.Q<Label>(ScheduleTimeStampName);
            var label2 = schedule2.Q<Label>(ScheduleTimeStampName);
            if (label1 == null || label2 == null) return 0;

            //"00:00" -> 0000
            string stamp1 = label1.text;
            string stamp2 = label2.text;
            stamp1 = stamp1.Replace(":", "");
            stamp2 = stamp2.Replace(":", "");
            int stampNum = int.Parse(stamp1);
            int otherNum = int.Parse(stamp2);

            if (stampNum > otherNum)
            {
                return 1;
            }

            if (stampNum < otherNum)
            {
                return -1;
            }

            return 0;
        }

        #endregion

        #region PopUps

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

        #endregion

        #region Dialogue

        /// <summary>
        /// Enables or disables the dialogue display.
        /// </summary>
        /// <param name="show">Whether or not to show the dialogue display.</param>
        public void ToggleDialogue(bool show)
        {
            if (dialogueMenu == null) return;
            dialogueMenu.rootVisualElement.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
            _dialogueVisible = show;
            Cursor.visible = _dialogueVisible;
            Cursor.lockState = _dialogueVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }

        /// <summary>
        /// Displays the dialogue for the character. Auto formats the text to be orange for the name.
        /// </summary>
        /// <param name="charName"></param>
        /// <param name="dialogueText"></param>
        public void DisplayDialogue(string charName, string dialogueText)
        {
            if (_dialogueText == null) return;
            _dialogueText.text = $"<color=\"orange\">{charName} <color=\"white\">- {dialogueText}";
        }
        
        /// <summary>
        /// Creates a dialogue option.
        /// </summary>
        /// <param name="text">The text you wish to display.</param>
        /// <param name="click">An action that gets invoked on click.</param>
        public void DisplayDialogueOption(string text, Action click)
        {
            if (string.IsNullOrEmpty(text)) return;
            var newButton = dialogueButton.Instantiate().Q<Button>();
            newButton.text = text;
            //no clue if this'll stick haha
            AutoFitLabelControl control = new AutoFitLabelControl(newButton, 16f, 30f);
            // newButton.AddManipulator(new Clickable(click));
            newButton.RegisterCallback<ClickEvent>(evt => click?.Invoke());
            _dialogueButtonContainer.Add(newButton);
        }

        /// <summary>
        /// Clears all dialogue options and makes them go away.
        /// </summary>
        public void ClearDialogueOptions()
        {
            dialogueMenu.rootVisualElement.Query(DialogueOptionName)
                .ForEach(option => { option.parent.Remove(option); });
        }
        
        #endregion
    }
}