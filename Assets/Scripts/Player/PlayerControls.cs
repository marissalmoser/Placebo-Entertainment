//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripts/Player/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""BasicControls"",
            ""id"": ""95b726d1-517c-4ab3-9c94-1f8baf8910ee"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""4a36e5dc-a661-4bc0-890b-2f589ff5c68a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""ea50a89a-6528-4748-8000-854b8c9de46d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Reset"",
                    ""type"": ""Button"",
                    ""id"": ""3733f2d6-4a5e-4112-9766-a891b2dada18"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""07887c75-b144-4247-b615-bb93993a14ac"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""OpenSchedule"",
                    ""type"": ""Button"",
                    ""id"": ""6e5d57fd-153e-4580-af9e-37ecc4e953a5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LeftClick"",
                    ""type"": ""Button"",
                    ""id"": ""abadb28f-5c40-438d-ad19-0b007ee2669f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SkipPause"",
                    ""type"": ""Button"",
                    ""id"": ""c7673e9c-1263-498d-8f96-d5e08da4b134"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=2.4,pressPoint=0.2),Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PauseGame"",
                    ""type"": ""Button"",
                    ""id"": ""6ff5f603-80a8-4a0b-ac26-410d9dfc9f50"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""StartGame"",
                    ""type"": ""Button"",
                    ""id"": ""52cb5aca-6e92-470d-90f8-b494667f6273"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""21716b67-aa9e-46f0-8145-73c74ef0b855"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""762ec265-7f82-4995-b0b5-106dab80c9a5"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bd274557-4d22-42c3-bad8-470b4810cdb4"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5d0ee6a7-08d5-45fc-b816-47018e0e528b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1b580068-4cc0-4464-9669-4598b0499011"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ba598e74-743b-477e-8ff4-1d074debefca"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b56295b2-32da-4ab6-a254-6ae37ecb01f4"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b66050c0-0f4f-4d18-b88d-d12be2bdd116"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0d77db3f-2589-46d8-b2bc-a08dc24e28d6"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reset"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6fd1c497-0f5f-4d77-baf7-59baad257ab2"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reset"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ec8e7d4-56cf-40b4-98c2-e789d03b17d9"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7e6dd329-01c1-4b85-b290-2e7c281b8ba3"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""19e4cfdd-4194-4a5c-a007-85178a4d0174"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenSchedule"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9abaccd4-1a28-49b3-b4d8-cb617e7dffe6"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenSchedule"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""562cd299-17b4-4e22-8606-6189008bc72a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1ede4830-7263-4c2d-af08-25ed2bb5f113"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2e48fc0a-935a-479a-8c00-a32e43bb9d29"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SkipPause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f6a9bac0-158f-4246-9467-2cacb1b497cd"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SkipPause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""275c05ee-be63-478c-87cb-f706f2d4fb4a"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9dc13471-a491-461b-b352-02a44fe38aba"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""634258d2-2ccc-4fa8-8eb8-aae753c96105"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StartGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38bf8e0e-6e53-4cf7-a910-711fc455c7d5"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StartGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // BasicControls
        m_BasicControls = asset.FindActionMap("BasicControls", throwIfNotFound: true);
        m_BasicControls_Move = m_BasicControls.FindAction("Move", throwIfNotFound: true);
        m_BasicControls_Look = m_BasicControls.FindAction("Look", throwIfNotFound: true);
        m_BasicControls_Reset = m_BasicControls.FindAction("Reset", throwIfNotFound: true);
        m_BasicControls_Interact = m_BasicControls.FindAction("Interact", throwIfNotFound: true);
        m_BasicControls_OpenSchedule = m_BasicControls.FindAction("OpenSchedule", throwIfNotFound: true);
        m_BasicControls_LeftClick = m_BasicControls.FindAction("LeftClick", throwIfNotFound: true);
        m_BasicControls_SkipPause = m_BasicControls.FindAction("SkipPause", throwIfNotFound: true);
        m_BasicControls_PauseGame = m_BasicControls.FindAction("PauseGame", throwIfNotFound: true);
        m_BasicControls_StartGame = m_BasicControls.FindAction("StartGame", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // BasicControls
    private readonly InputActionMap m_BasicControls;
    private List<IBasicControlsActions> m_BasicControlsActionsCallbackInterfaces = new List<IBasicControlsActions>();
    private readonly InputAction m_BasicControls_Move;
    private readonly InputAction m_BasicControls_Look;
    private readonly InputAction m_BasicControls_Reset;
    private readonly InputAction m_BasicControls_Interact;
    private readonly InputAction m_BasicControls_OpenSchedule;
    private readonly InputAction m_BasicControls_LeftClick;
    private readonly InputAction m_BasicControls_SkipPause;
    private readonly InputAction m_BasicControls_PauseGame;
    private readonly InputAction m_BasicControls_StartGame;
    public struct BasicControlsActions
    {
        private @PlayerControls m_Wrapper;
        public BasicControlsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_BasicControls_Move;
        public InputAction @Look => m_Wrapper.m_BasicControls_Look;
        public InputAction @Reset => m_Wrapper.m_BasicControls_Reset;
        public InputAction @Interact => m_Wrapper.m_BasicControls_Interact;
        public InputAction @OpenSchedule => m_Wrapper.m_BasicControls_OpenSchedule;
        public InputAction @LeftClick => m_Wrapper.m_BasicControls_LeftClick;
        public InputAction @SkipPause => m_Wrapper.m_BasicControls_SkipPause;
        public InputAction @PauseGame => m_Wrapper.m_BasicControls_PauseGame;
        public InputAction @StartGame => m_Wrapper.m_BasicControls_StartGame;
        public InputActionMap Get() { return m_Wrapper.m_BasicControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BasicControlsActions set) { return set.Get(); }
        public void AddCallbacks(IBasicControlsActions instance)
        {
            if (instance == null || m_Wrapper.m_BasicControlsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_BasicControlsActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @Reset.started += instance.OnReset;
            @Reset.performed += instance.OnReset;
            @Reset.canceled += instance.OnReset;
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
            @OpenSchedule.started += instance.OnOpenSchedule;
            @OpenSchedule.performed += instance.OnOpenSchedule;
            @OpenSchedule.canceled += instance.OnOpenSchedule;
            @LeftClick.started += instance.OnLeftClick;
            @LeftClick.performed += instance.OnLeftClick;
            @LeftClick.canceled += instance.OnLeftClick;
            @SkipPause.started += instance.OnSkipPause;
            @SkipPause.performed += instance.OnSkipPause;
            @SkipPause.canceled += instance.OnSkipPause;
            @PauseGame.started += instance.OnPauseGame;
            @PauseGame.performed += instance.OnPauseGame;
            @PauseGame.canceled += instance.OnPauseGame;
            @StartGame.started += instance.OnStartGame;
            @StartGame.performed += instance.OnStartGame;
            @StartGame.canceled += instance.OnStartGame;
        }

        private void UnregisterCallbacks(IBasicControlsActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @Reset.started -= instance.OnReset;
            @Reset.performed -= instance.OnReset;
            @Reset.canceled -= instance.OnReset;
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
            @OpenSchedule.started -= instance.OnOpenSchedule;
            @OpenSchedule.performed -= instance.OnOpenSchedule;
            @OpenSchedule.canceled -= instance.OnOpenSchedule;
            @LeftClick.started -= instance.OnLeftClick;
            @LeftClick.performed -= instance.OnLeftClick;
            @LeftClick.canceled -= instance.OnLeftClick;
            @SkipPause.started -= instance.OnSkipPause;
            @SkipPause.performed -= instance.OnSkipPause;
            @SkipPause.canceled -= instance.OnSkipPause;
            @PauseGame.started -= instance.OnPauseGame;
            @PauseGame.performed -= instance.OnPauseGame;
            @PauseGame.canceled -= instance.OnPauseGame;
            @StartGame.started -= instance.OnStartGame;
            @StartGame.performed -= instance.OnStartGame;
            @StartGame.canceled -= instance.OnStartGame;
        }

        public void RemoveCallbacks(IBasicControlsActions instance)
        {
            if (m_Wrapper.m_BasicControlsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IBasicControlsActions instance)
        {
            foreach (var item in m_Wrapper.m_BasicControlsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_BasicControlsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public BasicControlsActions @BasicControls => new BasicControlsActions(this);
    public interface IBasicControlsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnReset(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnOpenSchedule(InputAction.CallbackContext context);
        void OnLeftClick(InputAction.CallbackContext context);
        void OnSkipPause(InputAction.CallbackContext context);
        void OnPauseGame(InputAction.CallbackContext context);
        void OnStartGame(InputAction.CallbackContext context);
    }
}
