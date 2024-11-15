//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/_Project/Scripts/Core/Character/PlayerInput.inputactions
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

public partial class @PlayerInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""PlayerControl"",
            ""id"": ""ab140a0f-0436-4cb3-a05a-f0b0d03f790e"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""0fa6fc6d-84d8-404c-afac-a34208978df8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""08d72b35-706d-4dfd-aa20-e7fd63954957"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""7582dec2-15ab-452d-b06a-becdff655db9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""EquipAbility"",
                    ""type"": ""Button"",
                    ""id"": ""65709bd6-921b-42fe-b98e-c6f5d1b58f7d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Switch Weapon"",
                    ""type"": ""Value"",
                    ""id"": ""947e856e-d9fb-426d-8c53-56602f63e4e4"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": ""Clamp(min=-1,max=1)"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""b5df890d-46b5-4b60-ac03-1f98fc12f8d5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""4c4b8c54-d22a-43c3-aa2e-b98a95f5e9c8"",
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
                    ""id"": ""35810097-f1e1-4519-baa6-36b671efaeab"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""659411be-7164-44f5-ba7c-c960e1a45e37"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""882934a7-4dce-4df4-8515-b5f2c821a1c2"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2a854395-8b05-439d-9732-289e8fc43137"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""80d6357f-39d7-4d3f-b806-535430211d69"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboad"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c642d864-52ec-41e9-ab99-023c897554b2"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2(invertX=false),ScaleVector2(x=0.05,y=0.05)"",
                    ""groups"": ""Keyboad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""06101149-2cf9-443c-bd04-e444fb0a30b3"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboad"",
                    ""action"": ""EquipAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e7e93e19-f61b-42b1-a3b8-a45ed5a875d2"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Switch Weapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""357309eb-1527-434f-bbf6-bc673f2da20b"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboad"",
            ""bindingGroup"": ""Keyboad"",
            ""devices"": []
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": []
        }
    ]
}");
        // PlayerControl
        m_PlayerControl = asset.FindActionMap("PlayerControl", throwIfNotFound: true);
        m_PlayerControl_Move = m_PlayerControl.FindAction("Move", throwIfNotFound: true);
        m_PlayerControl_Look = m_PlayerControl.FindAction("Look", throwIfNotFound: true);
        m_PlayerControl_Attack = m_PlayerControl.FindAction("Attack", throwIfNotFound: true);
        m_PlayerControl_EquipAbility = m_PlayerControl.FindAction("EquipAbility", throwIfNotFound: true);
        m_PlayerControl_SwitchWeapon = m_PlayerControl.FindAction("Switch Weapon", throwIfNotFound: true);
        m_PlayerControl_Reload = m_PlayerControl.FindAction("Reload", throwIfNotFound: true);
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

    // PlayerControl
    private readonly InputActionMap m_PlayerControl;
    private List<IPlayerControlActions> m_PlayerControlActionsCallbackInterfaces = new List<IPlayerControlActions>();
    private readonly InputAction m_PlayerControl_Move;
    private readonly InputAction m_PlayerControl_Look;
    private readonly InputAction m_PlayerControl_Attack;
    private readonly InputAction m_PlayerControl_EquipAbility;
    private readonly InputAction m_PlayerControl_SwitchWeapon;
    private readonly InputAction m_PlayerControl_Reload;
    public struct PlayerControlActions
    {
        private @PlayerInput m_Wrapper;
        public PlayerControlActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerControl_Move;
        public InputAction @Look => m_Wrapper.m_PlayerControl_Look;
        public InputAction @Attack => m_Wrapper.m_PlayerControl_Attack;
        public InputAction @EquipAbility => m_Wrapper.m_PlayerControl_EquipAbility;
        public InputAction @SwitchWeapon => m_Wrapper.m_PlayerControl_SwitchWeapon;
        public InputAction @Reload => m_Wrapper.m_PlayerControl_Reload;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerControlActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerControlActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerControlActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
            @EquipAbility.started += instance.OnEquipAbility;
            @EquipAbility.performed += instance.OnEquipAbility;
            @EquipAbility.canceled += instance.OnEquipAbility;
            @SwitchWeapon.started += instance.OnSwitchWeapon;
            @SwitchWeapon.performed += instance.OnSwitchWeapon;
            @SwitchWeapon.canceled += instance.OnSwitchWeapon;
            @Reload.started += instance.OnReload;
            @Reload.performed += instance.OnReload;
            @Reload.canceled += instance.OnReload;
        }

        private void UnregisterCallbacks(IPlayerControlActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
            @EquipAbility.started -= instance.OnEquipAbility;
            @EquipAbility.performed -= instance.OnEquipAbility;
            @EquipAbility.canceled -= instance.OnEquipAbility;
            @SwitchWeapon.started -= instance.OnSwitchWeapon;
            @SwitchWeapon.performed -= instance.OnSwitchWeapon;
            @SwitchWeapon.canceled -= instance.OnSwitchWeapon;
            @Reload.started -= instance.OnReload;
            @Reload.performed -= instance.OnReload;
            @Reload.canceled -= instance.OnReload;
        }

        public void RemoveCallbacks(IPlayerControlActions instance)
        {
            if (m_Wrapper.m_PlayerControlActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerControlActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerControlActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerControlActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerControlActions @PlayerControl => new PlayerControlActions(this);
    private int m_KeyboadSchemeIndex = -1;
    public InputControlScheme KeyboadScheme
    {
        get
        {
            if (m_KeyboadSchemeIndex == -1) m_KeyboadSchemeIndex = asset.FindControlSchemeIndex("Keyboad");
            return asset.controlSchemes[m_KeyboadSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IPlayerControlActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnEquipAbility(InputAction.CallbackContext context);
        void OnSwitchWeapon(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
    }
}
