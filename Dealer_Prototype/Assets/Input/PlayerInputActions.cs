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
            ""name"": ""Default"",
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
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""ed3302d4-193b-439f-9e3f-c8e7f7e28586"",
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
                },
                {
                    ""name"": ""Up"",
                    ""type"": ""Button"",
                    ""id"": ""680f0111-1c1c-4607-8370-45dbf1c63444"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Down"",
                    ""type"": ""Button"",
                    ""id"": ""40ab9f81-3242-46e6-a8de-20686fbbee05"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Left"",
                    ""type"": ""Button"",
                    ""id"": ""14fdbd19-f1b3-4df4-8b66-00ca055ec554"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Right"",
                    ""type"": ""Button"",
                    ""id"": ""d51d5f87-b00c-483a-9f12-3038e191a445"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reveal"",
                    ""type"": ""Button"",
                    ""id"": ""2f20b0f8-f254-45f7-9f6d-f11ef4ad30bd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""00117c72-e4eb-47c6-91bb-8974ae843a1e"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""ca610da8-b4c3-44df-82f5-e2e5140d1b37"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme"",
                    ""action"": ""Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""54477028-c32e-443a-a88d-51602b68c457"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme"",
                    ""action"": ""Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be69152a-aeb8-4ef5-b35b-1cc775e25070"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme"",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d19f7f76-07bb-4281-b6e8-a6aeace42506"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme"",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""56f2168b-9333-40c2-9717-ef77709d9f48"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme"",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5a458cbc-d3b6-477b-a6e5-0ab9b1dbf3c9"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme"",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""85710184-5b8e-4d54-9e22-1bd4149f10a3"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme"",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""142f47f7-979a-45ab-9389-144cb43023b3"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme"",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""930f42a0-a7c5-4d72-b70e-2ec3ec08c303"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1770af3b-a9ae-49fd-b0a1-95b5ec22e2ef"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme"",
                    ""action"": ""Reveal"",
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
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Default
        m_Default = asset.FindActionMap("Default", throwIfNotFound: true);
        m_Default_Select = m_Default.FindAction("Select", throwIfNotFound: true);
        m_Default_Cancel = m_Default.FindAction("Cancel", throwIfNotFound: true);
        m_Default_Aim = m_Default.FindAction("Aim", throwIfNotFound: true);
        m_Default_Up = m_Default.FindAction("Up", throwIfNotFound: true);
        m_Default_Down = m_Default.FindAction("Down", throwIfNotFound: true);
        m_Default_Left = m_Default.FindAction("Left", throwIfNotFound: true);
        m_Default_Right = m_Default.FindAction("Right", throwIfNotFound: true);
        m_Default_Reveal = m_Default.FindAction("Reveal", throwIfNotFound: true);
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

    // Default
    private readonly InputActionMap m_Default;
    private IDefaultActions m_DefaultActionsCallbackInterface;
    private readonly InputAction m_Default_Select;
    private readonly InputAction m_Default_Cancel;
    private readonly InputAction m_Default_Aim;
    private readonly InputAction m_Default_Up;
    private readonly InputAction m_Default_Down;
    private readonly InputAction m_Default_Left;
    private readonly InputAction m_Default_Right;
    private readonly InputAction m_Default_Reveal;
    public struct DefaultActions
    {
        private @PlayerInputActions m_Wrapper;
        public DefaultActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Select => m_Wrapper.m_Default_Select;
        public InputAction @Cancel => m_Wrapper.m_Default_Cancel;
        public InputAction @Aim => m_Wrapper.m_Default_Aim;
        public InputAction @Up => m_Wrapper.m_Default_Up;
        public InputAction @Down => m_Wrapper.m_Default_Down;
        public InputAction @Left => m_Wrapper.m_Default_Left;
        public InputAction @Right => m_Wrapper.m_Default_Right;
        public InputAction @Reveal => m_Wrapper.m_Default_Reveal;
        public InputActionMap Get() { return m_Wrapper.m_Default; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DefaultActions set) { return set.Get(); }
        public void SetCallbacks(IDefaultActions instance)
        {
            if (m_Wrapper.m_DefaultActionsCallbackInterface != null)
            {
                @Select.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnSelect;
                @Cancel.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnCancel;
                @Aim.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnAim;
                @Up.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnUp;
                @Up.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnUp;
                @Up.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnUp;
                @Down.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnDown;
                @Down.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnDown;
                @Down.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnDown;
                @Left.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnLeft;
                @Left.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnLeft;
                @Left.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnLeft;
                @Right.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnRight;
                @Right.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnRight;
                @Right.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnRight;
                @Reveal.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnReveal;
                @Reveal.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnReveal;
                @Reveal.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnReveal;
            }
            m_Wrapper.m_DefaultActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @Up.started += instance.OnUp;
                @Up.performed += instance.OnUp;
                @Up.canceled += instance.OnUp;
                @Down.started += instance.OnDown;
                @Down.performed += instance.OnDown;
                @Down.canceled += instance.OnDown;
                @Left.started += instance.OnLeft;
                @Left.performed += instance.OnLeft;
                @Left.canceled += instance.OnLeft;
                @Right.started += instance.OnRight;
                @Right.performed += instance.OnRight;
                @Right.canceled += instance.OnRight;
                @Reveal.started += instance.OnReveal;
                @Reveal.performed += instance.OnReveal;
                @Reveal.canceled += instance.OnReveal;
            }
        }
    }
    public DefaultActions @Default => new DefaultActions(this);
    private int m_ControlSchemeSchemeIndex = -1;
    public InputControlScheme ControlSchemeScheme
    {
        get
        {
            if (m_ControlSchemeSchemeIndex == -1) m_ControlSchemeSchemeIndex = asset.FindControlSchemeIndex("ControlScheme");
            return asset.controlSchemes[m_ControlSchemeSchemeIndex];
        }
    }
    public interface IDefaultActions
    {
        void OnSelect(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnUp(InputAction.CallbackContext context);
        void OnDown(InputAction.CallbackContext context);
        void OnLeft(InputAction.CallbackContext context);
        void OnRight(InputAction.CallbackContext context);
        void OnReveal(InputAction.CallbackContext context);
    }
}
