    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;
    
    //https://forum.unity.com/threads/scrollview-with-drag-scrolling.1195807/
    namespace UI.Components
    {
        public class DragScrollView : ScrollView
        {
            public new class UxmlFactory : UxmlFactory<DragScrollView, UxmlTraits> { }
     
            public new class UxmlTraits : ScrollView.UxmlTraits
            {
                UxmlBoolAttributeDescription Interactable = new UxmlBoolAttributeDescription { name = "Interactable", defaultValue = true };
                UxmlBoolAttributeDescription IgnoreChildren = new UxmlBoolAttributeDescription { name = "IgnoreChildren", defaultValue = false };
     
                public UxmlTraits() : base() { Interactable.defaultValue = true; }
     
                public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
                {
                    get
                    {
                        yield return new UxmlChildElementDescription(typeof(VisualElement));
                    }
                }
     
                public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
                {
                    base.Init(ve, bag, cc);
                    ((DragScrollView)ve).Interactable = Interactable.GetValueFromBag(bag, cc);
                    ((DragScrollView)ve).IgnoreChildren = IgnoreChildren.GetValueFromBag(bag, cc);
                }
            }
     
            public bool Interactable = true;
            public bool IgnoreChildren
            {
                get => ignoreChildren; set
                {
                    if (ignoreChildren != value)
                    {
                        if (value) UnregisterChildrenCallbacks(); else RegisterChildrenCallbacks();
                    }
                    ignoreChildren = value;
                }
            }
            public bool ContainsMouse { get; private set; } = false;
            public bool MouseDown { get; private set; } = false;
            public Vector2 ScrollRootOffset { get; private set; }
            public Vector2 MouseDownLocation { get; private set; }
     
            private bool ignoreChildren;
            private List<VisualElement> registered = new();
     
    #if DRAG_LOGGING
        static int _nextId;
        int _id;
    #endif
     
            public DragScrollView() : this(ScrollViewMode.Vertical) { }
            public DragScrollView(ScrollViewMode scrollViewMode) : base(scrollViewMode)
            {
                DoRegisterCallbacks();
            }
     
            VisualElement MouseOwner => this;
     
            protected virtual void DoRegisterCallbacks()
            {
                MouseOwner.RegisterCallback<MouseUpEvent>(OnMouseUp);
                MouseOwner.RegisterCallback<MouseDownEvent>(OnMouseDown);
                MouseOwner.RegisterCallback<MouseMoveEvent>(OnMouseMove);
     
                if (!IgnoreChildren)
                {
                    RegisterChildrenCallbacks();
                }
            }
            protected virtual void UnregisterCallbacks()
            {
                MouseOwner.RegisterCallback<MouseUpEvent>(OnMouseUp);
                MouseOwner.RegisterCallback<MouseDownEvent>(OnMouseDown);
                MouseOwner.RegisterCallback<MouseMoveEvent>(OnMouseMove);
     
                if (!IgnoreChildren)
                {
                    UnregisterChildrenCallbacks();
                }
            }
     
            protected virtual void RegisterChildrenCallbacks()
            {
                List<VisualElement> childrenVE = this.Query().ToList();
     
                foreach (var b in childrenVE)
                {
                    b.RegisterCallback<MouseUpEvent>(OnMouseUp, TrickleDown.TrickleDown);
                    b.RegisterCallback<MouseDownEvent>(OnMouseDown, TrickleDown.TrickleDown);
                    b.RegisterCallback<MouseMoveEvent>(OnMouseMove, TrickleDown.TrickleDown);
                    registered.Add(b);
                }
            }
            protected virtual void UnregisterChildrenCallbacks()
            {
                List<VisualElement> childrenVE = this.Query().ToList();
     
                foreach (var b in childrenVE)
                {
                    b.UnregisterCallback<MouseUpEvent>(OnMouseUp, TrickleDown.TrickleDown);
                    b.UnregisterCallback<MouseDownEvent>(OnMouseDown, TrickleDown.TrickleDown);
                    b.UnregisterCallback<MouseMoveEvent>(OnMouseMove, TrickleDown.TrickleDown);
                    registered.Remove(b);
                }
                registered.Clear();
            }
     
            void HandleDrag(IMouseEvent e)
            {
                Vector2 deltaPos = e.mousePosition - MouseDownLocation;
    #if DRAG_LOGGING
            Debug.Log($"DragScroll #{_id}: Drag delta = {deltaPos}");
    #endif
     
                deltaPos = ScrollRootOffset - deltaPos;
                switch (mode)
                {
                    case ScrollViewMode.Vertical:
                        deltaPos.x = scrollOffset.x;
                        break;
                    case ScrollViewMode.Horizontal:
                        deltaPos.y = scrollOffset.y;
                        break;
                    default:
                        break;
                }
                scrollOffset = deltaPos;
            }
     
            protected virtual void OnMouseMove(MouseMoveEvent e)
            {
                if (MouseDown && Interactable)
                {
                    if (MouseCaptureController.HasMouseCapture(MouseOwner))
                        HandleDrag(e);
                    else
                    {
    #if DRAG_LOGGING
                    Debug.Log($"DragScroll #{_id}: Lost Mouse Capture; IsMouseCaptured = {MouseCaptureController.IsMouseCaptured()}");
    #endif
                    }
                }
                e.StopPropagation();
            }
     
            protected virtual void OnMouseUp(MouseUpEvent e)
            {
    #if DRAG_LOGGING
            Debug.Log($"DragScroll #{_id}: OnMouseUp {e.mousePosition}");
    #endif
                MouseCaptureController.ReleaseMouse(MouseOwner);
     
                // Update elastic behavior
                if (touchScrollBehavior == TouchScrollBehavior.Elastic)
                {
                    m_LowBounds = new Vector2(
                        Mathf.Min(horizontalScroller.lowValue, horizontalScroller.highValue),
                        Mathf.Min(verticalScroller.lowValue, verticalScroller.highValue));
                    m_HighBounds = new Vector2(
                        Mathf.Max(horizontalScroller.lowValue, horizontalScroller.highValue),
                        Mathf.Max(verticalScroller.lowValue, verticalScroller.highValue));
     
                    ExecuteElasticSpringAnimation();
                }
     
                MouseDown = false;
                e.StopPropagation();
            }
     
            protected virtual void OnMouseDown(MouseDownEvent e)
            {
    #if DRAG_LOGGING
            Debug.Log($"DragScroll #{_id}: OnMouseDown {e.mousePosition}");
    #endif
                if (!worldBound.Contains(e.mousePosition))
                {
    #if DRAG_LOGGING
                Debug.Log($"DragScroll #{_id}: Release Mouse {e.mousePosition}");
    #endif
                    MouseCaptureController.ReleaseMouse(MouseOwner);
                }
                else if (Interactable)
                {
    #if DRAG_LOGGING
                Debug.Log($"DragScroll #{_id}: Capture Mouse {e.mousePosition}");
    #endif
                    MouseOwner.CaptureMouse();
                    MouseDownLocation = e.mousePosition;
                    ScrollRootOffset = scrollOffset;
                    MouseDown = true;
                    e.StopPropagation();
                }
            }
     
            // Copied from Unity scource code: https://github.com/Unity-Technologies/UnityCsReference/blob/master/ModuleOverrides/com.unity.ui/Core/Controls/ScrollView.cs
            private bool hasInertia => scrollDecelerationRate > 0f;
            private Vector2 m_Velocity;
            private Vector2 m_SpringBackVelocity;
            private Vector2 m_LowBounds;
            private Vector2 m_HighBounds;
            private IVisualElementScheduledItem m_PostPointerUpAnimation;
     
            void ExecuteElasticSpringAnimation()
            {
                ComputeInitialSpringBackVelocity();
     
                if (m_PostPointerUpAnimation == null)
                {
                    m_PostPointerUpAnimation = schedule.Execute(PostPointerUpAnimation).Every(30);
                }
                else
                {
                    m_PostPointerUpAnimation.Resume();
                }
            }
     
            private void PostPointerUpAnimation()
            {
                ApplyScrollInertia();
                SpringBack();
     
                // This compares with epsilon.
                if (m_SpringBackVelocity == Vector2.zero && m_Velocity == Vector2.zero)
                {
                    m_PostPointerUpAnimation.Pause();
                }
            }
     
            private void ComputeInitialSpringBackVelocity()
            {
                if (touchScrollBehavior != TouchScrollBehavior.Elastic)
                {
                    m_SpringBackVelocity = Vector2.zero;
                    return;
                }
     
                if (scrollOffset.x < m_LowBounds.x)
                {
                    m_SpringBackVelocity.x = m_LowBounds.x - scrollOffset.x;
                }
                else if (scrollOffset.x > m_HighBounds.x)
                {
                    m_SpringBackVelocity.x = m_HighBounds.x - scrollOffset.x;
                }
                else
                {
                    m_SpringBackVelocity.x = 0;
                }
     
                if (scrollOffset.y < m_LowBounds.y)
                {
                    m_SpringBackVelocity.y = m_LowBounds.y - scrollOffset.y;
                }
                else if (scrollOffset.y > m_HighBounds.y)
                {
                    m_SpringBackVelocity.y = m_HighBounds.y - scrollOffset.y;
                }
                else
                {
                    m_SpringBackVelocity.y = 0;
                }
            }
     
            private void SpringBack()
            {
                if (touchScrollBehavior != TouchScrollBehavior.Elastic)
                {
                    m_SpringBackVelocity = Vector2.zero;
                    return;
                }
     
                var newOffset = scrollOffset;
     
                if (newOffset.x < m_LowBounds.x)
                {
                    newOffset.x = Mathf.SmoothDamp(newOffset.x, m_LowBounds.x, ref m_SpringBackVelocity.x, elasticity,
                        Mathf.Infinity, Time.unscaledDeltaTime);
                    if (Mathf.Abs(m_SpringBackVelocity.x) < 1)
                    {
                        m_SpringBackVelocity.x = 0;
                    }
                }
                else if (newOffset.x > m_HighBounds.x)
                {
                    newOffset.x = Mathf.SmoothDamp(newOffset.x, m_HighBounds.x, ref m_SpringBackVelocity.x, elasticity,
                        Mathf.Infinity, Time.unscaledDeltaTime);
                    if (Mathf.Abs(m_SpringBackVelocity.x) < 1)
                    {
                        m_SpringBackVelocity.x = 0;
                    }
                }
                else
                {
                    m_SpringBackVelocity.x = 0;
                }
     
                if (newOffset.y < m_LowBounds.y)
                {
                    newOffset.y = Mathf.SmoothDamp(newOffset.y, m_LowBounds.y, ref m_SpringBackVelocity.y, elasticity,
                        Mathf.Infinity, Time.unscaledDeltaTime);
                    if (Mathf.Abs(m_SpringBackVelocity.y) < 1)
                    {
                        m_SpringBackVelocity.y = 0;
                    }
                }
                else if (newOffset.y > m_HighBounds.y)
                {
                    newOffset.y = Mathf.SmoothDamp(newOffset.y, m_HighBounds.y, ref m_SpringBackVelocity.y, elasticity,
                        Mathf.Infinity, Time.unscaledDeltaTime);
                    if (Mathf.Abs(m_SpringBackVelocity.y) < 1)
                    {
                        m_SpringBackVelocity.y = 0;
                    }
                }
                else
                {
                    m_SpringBackVelocity.y = 0;
                }
     
                scrollOffset = newOffset;
            }
     
            // Internal for tests.
            internal void ApplyScrollInertia()
            {
                if (hasInertia && m_Velocity != Vector2.zero)
                {
                    m_Velocity *= Mathf.Pow(scrollDecelerationRate, Time.unscaledDeltaTime);
     
                    if (Mathf.Abs(m_Velocity.x) < 1 ||
                        touchScrollBehavior == TouchScrollBehavior.Elastic && (scrollOffset.x < m_LowBounds.x || scrollOffset.x > m_HighBounds.x))
                    {
                        m_Velocity.x = 0;
                    }
     
                    if (Mathf.Abs(m_Velocity.y) < 1 ||
                        touchScrollBehavior == TouchScrollBehavior.Elastic && (scrollOffset.y < m_LowBounds.y || scrollOffset.y > m_HighBounds.y))
                    {
                        m_Velocity.y = 0;
                    }
     
                    scrollOffset += m_Velocity * Time.unscaledDeltaTime;
                }
                else
                {
                    m_Velocity = Vector2.zero;
                }
            }
     
        }
    }
