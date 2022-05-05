using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockCursor : MonoBehaviour {
    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update() {
        Cursor.lockState = CursorLockMode.Locked;
        if (Keyboard.current.bKey.wasPressedThisFrame) {
            Debug.Log("LOG!!");
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }
    }
}