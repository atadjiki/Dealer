// GENERATED AUTOMATICALLY FROM 'Assets/Input/PlayerInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Mouse"",
            ""id"": ""e94f4ea3-c475-48c1-a410-2efad53976dd"",
            ""actions"": [
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""2bccbfbf-27d8-4aac-a193-a25b8a3f15b4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""ac7c791c-86c2-4d1d-8727-5ab3b8d7d359"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""00117c72-e4eb-47c6-91bb-8974ae843a1e"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ae624fc7-3a51-4732-849d-49ec0fc7e051"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""ControlScheme"",
            ""bindingGroup"": ""ControlScheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Mouse
        m_Mouse = asset.FindActionMap("Mouse", throwIfNotFound: true);
        m_Mouse_Select = m_Mouse.FindAction("Select", throwIfNotFound: true);
        m_Mouse_Aim = m_Mouse.FindAction("Aim", throwIfNotFound: true);
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

    // Mouse
    private readonly InputActionMap m_Mouse;
    private IMouseActions m_MouseActionsCallbackInterface;
    private readonly InputAction m_Mouse_Select;
    private readonly InputAction m_Mouse_Aim;
    public struct MouseActions
    {
        private @PlayerInputActions m_Wrapper;
        public MouseActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Select => m_Wrapper.m_Mouse_Select;
        public InputAction @Aim => m_Wrapper.m_Mouse_Aim;
        public InputActionMap Get() { return m_Wrapper.m_Mouse; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseActions set) { return set.Get(); }
        public void SetCallbacks(IMouseActions instance)
        {
            if (m_Wrapper.m_MouseActionsCallbackInterface != null)
            {
                @Select.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnSelect;
                @Aim.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnAim;
            }
            m_Wrapper.m_MouseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
            }
        }
    }
    public MouseActions @Mouse => new MouseActions(this);
    private int m_ControlSchemeSchemeIndex = -1;
    public InputControlScheme ControlSchemeScheme
    {
        get
        {
            if (m_ControlSchemeSchemeIndex == -1) m_ControlSchemeSchemeIndex = asset.FindControlSchemeIndex("ControlScheme");
            return asset.controlSchemes[m_ControlSchemeSchemeIndex];
        }
    }
    public interface IMouseActions
    {
        void OnSelect(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
    }
}
