// GENERATED AUTOMATICALLY FROM 'Assets/Controls/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Land"",
            ""id"": ""8b75f179-ab14-4eca-b55a-3ea38537984a"",
            ""actions"": [
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""be38557c-7997-4781-966c-037af55b518b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""085bbd2a-4ce2-4936-8a97-18a02b30220a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""55ab08cf-f526-46f2-ae77-7326af0d90fc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""b41c1cdb-38fd-4fdd-9b70-61b2afe78ebc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightStick"",
                    ""type"": ""Value"",
                    ""id"": ""45b25632-3553-4623-a906-8d9dba72823b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack_1"",
                    ""type"": ""Button"",
                    ""id"": ""5a06e6fb-59d9-4af1-9991-17b104fe69f2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LockTarget"",
                    ""type"": ""Button"",
                    ""id"": ""975b2439-0276-41a8-aa36-dab91873cd40"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseDelta"",
                    ""type"": ""Value"",
                    ""id"": ""fb424174-1a13-4cad-8787-0758700515be"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleAds"",
                    ""type"": ""Button"",
                    ""id"": ""0e771d47-104c-49f4-9124-e1829af7e1b6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Equip_S1"",
                    ""type"": ""Button"",
                    ""id"": ""ae98722b-795d-4209-956e-e7b6c17c44d2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""ea5d4668-67ba-497e-bdc1-f1c637d714a1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""28cf462f-020a-4c3c-a15c-df9148108a82"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""daeb1ed6-5888-49bc-a43f-7ba90fc34e24"",
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
                    ""id"": ""ec66b7fe-1188-49f5-b537-e39f0ced2556"",
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
                    ""id"": ""38dcda00-2390-41bf-a655-525febd9d4e9"",
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
                    ""id"": ""8d2f48d1-6c20-4b41-b4d5-9b718dffe136"",
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
                    ""id"": ""ecc51c02-2420-40f0-99ad-25eaf3571e21"",
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
                    ""id"": ""3d98085f-f23d-415b-af51-9c4aa47d15b4"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""660072de-1ec0-4fae-ad4b-7875be237d13"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightStick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e57eabfb-dfbd-4612-b729-9ddf3ef806d3"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack_1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9e81f368-2289-4aa5-82af-6839df955d88"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack_1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eb048232-cea6-40b5-b6db-67ff2d792e5f"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LockTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ffc5aa25-bba2-4f85-8e78-104a05ca25b2"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ec3071f-335e-4cce-af4c-bb1b27700e71"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleAds"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bb6f184a-4835-463a-a2b5-d1230192d954"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Equip_S1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2a080628-09e9-44a9-936d-050464231112"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""00a7272e-94ea-4383-abe8-6f8963b876b3"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8c825e67-1635-4756-a00c-3e8dc869614c"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Land
        m_Land = asset.FindActionMap("Land", throwIfNotFound: true);
        m_Land_MousePosition = m_Land.FindAction("MousePosition", throwIfNotFound: true);
        m_Land_Move = m_Land.FindAction("Move", throwIfNotFound: true);
        m_Land_Jump = m_Land.FindAction("Jump", throwIfNotFound: true);
        m_Land_Reload = m_Land.FindAction("Reload", throwIfNotFound: true);
        m_Land_RightStick = m_Land.FindAction("RightStick", throwIfNotFound: true);
        m_Land_Attack_1 = m_Land.FindAction("Attack_1", throwIfNotFound: true);
        m_Land_LockTarget = m_Land.FindAction("LockTarget", throwIfNotFound: true);
        m_Land_MouseDelta = m_Land.FindAction("MouseDelta", throwIfNotFound: true);
        m_Land_ToggleAds = m_Land.FindAction("ToggleAds", throwIfNotFound: true);
        m_Land_Equip_S1 = m_Land.FindAction("Equip_S1", throwIfNotFound: true);
        m_Land_Crouch = m_Land.FindAction("Crouch", throwIfNotFound: true);
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

    // Land
    private readonly InputActionMap m_Land;
    private ILandActions m_LandActionsCallbackInterface;
    private readonly InputAction m_Land_MousePosition;
    private readonly InputAction m_Land_Move;
    private readonly InputAction m_Land_Jump;
    private readonly InputAction m_Land_Reload;
    private readonly InputAction m_Land_RightStick;
    private readonly InputAction m_Land_Attack_1;
    private readonly InputAction m_Land_LockTarget;
    private readonly InputAction m_Land_MouseDelta;
    private readonly InputAction m_Land_ToggleAds;
    private readonly InputAction m_Land_Equip_S1;
    private readonly InputAction m_Land_Crouch;
    public struct LandActions
    {
        private @PlayerControls m_Wrapper;
        public LandActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MousePosition => m_Wrapper.m_Land_MousePosition;
        public InputAction @Move => m_Wrapper.m_Land_Move;
        public InputAction @Jump => m_Wrapper.m_Land_Jump;
        public InputAction @Reload => m_Wrapper.m_Land_Reload;
        public InputAction @RightStick => m_Wrapper.m_Land_RightStick;
        public InputAction @Attack_1 => m_Wrapper.m_Land_Attack_1;
        public InputAction @LockTarget => m_Wrapper.m_Land_LockTarget;
        public InputAction @MouseDelta => m_Wrapper.m_Land_MouseDelta;
        public InputAction @ToggleAds => m_Wrapper.m_Land_ToggleAds;
        public InputAction @Equip_S1 => m_Wrapper.m_Land_Equip_S1;
        public InputAction @Crouch => m_Wrapper.m_Land_Crouch;
        public InputActionMap Get() { return m_Wrapper.m_Land; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(LandActions set) { return set.Get(); }
        public void SetCallbacks(ILandActions instance)
        {
            if (m_Wrapper.m_LandActionsCallbackInterface != null)
            {
                @MousePosition.started -= m_Wrapper.m_LandActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnMousePosition;
                @Move.started -= m_Wrapper.m_LandActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_LandActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnJump;
                @Reload.started -= m_Wrapper.m_LandActionsCallbackInterface.OnReload;
                @Reload.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnReload;
                @Reload.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnReload;
                @RightStick.started -= m_Wrapper.m_LandActionsCallbackInterface.OnRightStick;
                @RightStick.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnRightStick;
                @RightStick.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnRightStick;
                @Attack_1.started -= m_Wrapper.m_LandActionsCallbackInterface.OnAttack_1;
                @Attack_1.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnAttack_1;
                @Attack_1.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnAttack_1;
                @LockTarget.started -= m_Wrapper.m_LandActionsCallbackInterface.OnLockTarget;
                @LockTarget.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnLockTarget;
                @LockTarget.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnLockTarget;
                @MouseDelta.started -= m_Wrapper.m_LandActionsCallbackInterface.OnMouseDelta;
                @MouseDelta.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnMouseDelta;
                @MouseDelta.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnMouseDelta;
                @ToggleAds.started -= m_Wrapper.m_LandActionsCallbackInterface.OnToggleAds;
                @ToggleAds.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnToggleAds;
                @ToggleAds.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnToggleAds;
                @Equip_S1.started -= m_Wrapper.m_LandActionsCallbackInterface.OnEquip_S1;
                @Equip_S1.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnEquip_S1;
                @Equip_S1.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnEquip_S1;
                @Crouch.started -= m_Wrapper.m_LandActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnCrouch;
            }
            m_Wrapper.m_LandActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Reload.started += instance.OnReload;
                @Reload.performed += instance.OnReload;
                @Reload.canceled += instance.OnReload;
                @RightStick.started += instance.OnRightStick;
                @RightStick.performed += instance.OnRightStick;
                @RightStick.canceled += instance.OnRightStick;
                @Attack_1.started += instance.OnAttack_1;
                @Attack_1.performed += instance.OnAttack_1;
                @Attack_1.canceled += instance.OnAttack_1;
                @LockTarget.started += instance.OnLockTarget;
                @LockTarget.performed += instance.OnLockTarget;
                @LockTarget.canceled += instance.OnLockTarget;
                @MouseDelta.started += instance.OnMouseDelta;
                @MouseDelta.performed += instance.OnMouseDelta;
                @MouseDelta.canceled += instance.OnMouseDelta;
                @ToggleAds.started += instance.OnToggleAds;
                @ToggleAds.performed += instance.OnToggleAds;
                @ToggleAds.canceled += instance.OnToggleAds;
                @Equip_S1.started += instance.OnEquip_S1;
                @Equip_S1.performed += instance.OnEquip_S1;
                @Equip_S1.canceled += instance.OnEquip_S1;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
            }
        }
    }
    public LandActions @Land => new LandActions(this);
    public interface ILandActions
    {
        void OnMousePosition(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnRightStick(InputAction.CallbackContext context);
        void OnAttack_1(InputAction.CallbackContext context);
        void OnLockTarget(InputAction.CallbackContext context);
        void OnMouseDelta(InputAction.CallbackContext context);
        void OnToggleAds(InputAction.CallbackContext context);
        void OnEquip_S1(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
    }
}
