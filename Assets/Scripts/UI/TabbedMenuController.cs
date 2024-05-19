using UnityEngine.UIElements;

public class TabbedMenuController
{
    private VisualElement _root;
    private const string TabClassName = "tab";
    private const string SelectedTabClassName = "currentlySelectedTab";
    private const string UnSelectedTabClassName = "currentlyUnSelectedTab";
    private const string TabNameSuffix = "Tab";
    private const string ContentNameSuffix = "Content";
    private const string HideClassName = "unselectedContent";
    public TabbedMenuController(VisualElement root)
    {
        _root = root;
    }

    public void RegisterTabCallbacks()
    {
        UQueryBuilder<Label> tabs = GetAllTabs();
        tabs.ForEach(tab => { tab.RegisterCallback<ClickEvent>(TabOnClick); });
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