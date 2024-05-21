using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace PlaceboEntertainment.UI
{
    public class TabbedMenu : MonoBehaviour
    {
        private VisualElement _root;
        private VisualElement _playerObject;
        private const string TabClassName = "tab";
        private const string SelectedTabClassName = "currentlySelectedTab";
        private const string UnSelectedTabClassName = "currentlyUnSelectedTab";
        private const string TabNameSuffix = "Tab";
        private const string ContentNameSuffix = "Content";
        private const string HideClassName = "unselectedContent";
        private const string PlayerName = "Player";
        [SerializeField] private Transform playerTransform;

        [SerializeField] private Transform mapBoundaryUpperLeft,
            mapBoundaryUpperRight,
            mapBoundaryLowerLeft,
            mapBoundaryLowerRight;

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

        private void Awake()
        {
            var menu = GetComponent<UIDocument>();
            _root = menu.rootVisualElement;
            _playerObject = _root.Q(PlayerName);
        }

        private void OnEnable()
        {
            RegisterTabCallbacks();
        }

        private void OnDisable()
        {
            UnRegisterTabCallbacks();
        }

        private void Update()
        {
            if (!mapBoundaryLowerLeft || !mapBoundaryLowerRight || !mapBoundaryUpperLeft ||
                !mapBoundaryUpperRight || _playerObject == null) return;
            float xAxis = Mathf.InverseLerp(mapBoundaryUpperLeft.position.x,
                mapBoundaryUpperRight.position.x,playerTransform.position.x);
            float yAxis = Mathf.InverseLerp(mapBoundaryUpperLeft.position.z, mapBoundaryLowerLeft.position.z,
                playerTransform.position.z);
            var parent = _playerObject.parent;
            _playerObject.style.translate =
                new Translate((xAxis * parent.resolvedStyle.width), (yAxis * parent.resolvedStyle.height));
        }

        private void RegisterTabCallbacks()
        {
            UQueryBuilder<Label> tabs = GetAllTabs();
            tabs.ForEach(tab => { tab.RegisterCallback<ClickEvent>(TabOnClick); });
        }
        
        private void UnRegisterTabCallbacks()
        {
            UQueryBuilder<Label> tabs = GetAllTabs();
            tabs.ForEach(tab => { tab.UnregisterCallback<ClickEvent>(TabOnClick); });
        }

        private void TabOnClick(ClickEvent evt)
        {
            Label clickedTab = evt.currentTarget as Label;
            if (!TabIsCurrentlySelected(clickedTab))
            {
                GetAllTabs().Where(
                        (tab) =>
                            tab != clickedTab && TabIsCurrentlySelected(tab))
                    .ForEach(UnSelectTab);
                SelectTab(clickedTab);
            }
        }

        private static bool TabIsCurrentlySelected(Label tab)
        {
            return tab.ClassListContains(SelectedTabClassName);
        }

        private UQueryBuilder<Label> GetAllTabs()
        {
            return _root.Query<Label>(className: TabClassName);
        }

        private void UnSelectTab(Label tab)
        {
            tab.RemoveFromClassList(SelectedTabClassName);
            tab.AddToClassList(UnSelectedTabClassName);
            VisualElement content = FindContent(tab);
            content.AddToClassList(HideClassName);
        }

        private void SelectTab(Label tab)
        {
            tab.RemoveFromClassList(UnSelectedTabClassName);
            tab.AddToClassList(SelectedTabClassName);
            VisualElement content = FindContent(tab);
            content.RemoveFromClassList(HideClassName);
        }

        private VisualElement FindContent(Label tab)
        {
            return _root.Q(ContentName(tab));
        }

        private static string ContentName(Label tab)
        {
            return tab.name.Replace(TabNameSuffix, ContentNameSuffix);
        }
    }
}