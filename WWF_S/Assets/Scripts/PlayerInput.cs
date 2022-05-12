using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput {
    public Vector2 vecMoveXZ = Vector2.zero;
    public Vector3 v3vecMoveXZ = Vector3.zero;
    public Vector2 headPitchYaw = Vector2.zero;
    public Action[] actions;
    public Action toggleAds;
    public Action attack_1;
    public Action equip_s1;
    public Action equip_s2;
    public Action equip_s3;
    public Action equip_s4;
    public Action jump;
    public Action crouch;
    public Action reload;

    protected Character character;

    public PlayerInput(Character character) {
    }

    public class Action {
        public bool isTriggered;
        public event Delegates.EmptyDelegate keyDownEvent;
        public event Delegates.EmptyDelegate keyUpEvent;

        // Local character constructor
        public Action(InputAction inputAction) {
            inputAction.started += InputAction_started;
            inputAction.canceled += InputAction_canceled;
        }

        // Server character constructor
        public Action() {
        }

        private void InputAction_started(InputAction.CallbackContext obj) {
            Started();
        }

        private void InputAction_canceled(InputAction.CallbackContext obj) {
            Canceled();
        }

        /// <summary> Used on server to set state </summary>
        public void Set(bool state) {
            if (state != isTriggered) {
                if (state)
                    Started();
                else
                    Canceled();
            }
        }

        private void Started() {
            isTriggered = true;
            keyDownEvent?.Invoke();
        }

        private void Canceled() {
            isTriggered = false;
            keyUpEvent?.Invoke();
        }
    }
}
