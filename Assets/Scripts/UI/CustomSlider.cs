using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomSlider : MonoBehaviour
{
    [SerializeField] private UIDocument _rootDocument;
    private List<VisualElement> _sliders;
    private List<VisualElement> _defaultDraggers;
    private List<VisualElement> _newDraggers = new List<VisualElement>();

    private void Start()
    {
        _sliders = _rootDocument.rootVisualElement.Query("SettingsSlider").ToList();
        _defaultDraggers = _rootDocument.rootVisualElement.Query("unity-dragger").ToList();

        AddNewDraggers();

        for (int i = 0; i < _sliders.Count; ++i)
        {
            _sliders[i].RegisterCallback<ChangeEvent<float>>(evt => { UpdateDraggerPosition(i - 1); });
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _sliders.Count; ++i)
        {
            _sliders[i].UnregisterCallback<ChangeEvent<float>>(evt => { UpdateDraggerPosition(i - 1); });
        }
    }

    private void AddNewDraggers()
    {
        for (int i = 0; i < _sliders.Count; ++i)
        {
            VisualElement newDragger = new VisualElement();
            _newDraggers.Add(newDragger);
            _sliders[i].Add(newDragger);
            newDragger.name = "CircularDragger";
            newDragger.AddToClassList("circularDragger");
            newDragger.pickingMode = PickingMode.Ignore;
        }
    }

    private void UpdateDraggerPosition(int draggerIndex)
    {
        if (draggerIndex < _newDraggers.Count && draggerIndex < _defaultDraggers.Count)
        {
            VisualElement currentCircleDragger = _newDraggers[draggerIndex];
            VisualElement currentDefaultDragger = _defaultDraggers[draggerIndex];

            Vector2 distance = new Vector2((currentCircleDragger.layout.width - currentDefaultDragger.layout.width) / 2 - 8f, 
                (currentCircleDragger.layout.height - currentDefaultDragger.layout.height) / 2 - 8f);
            Vector2 position = currentDefaultDragger.parent.LocalToWorld(currentDefaultDragger.transform.position);
            currentCircleDragger.transform.position = currentCircleDragger.parent.WorldToLocal(position - distance);
        }
    }
}
