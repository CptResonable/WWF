using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCameraControler : MonoBehaviour {
    [SerializeField] private Camera camera;

    private bool depthModeEnabled;
    private void Update() {
        if (UnityEngine.InputSystem.Keyboard.current.leftArrowKey.wasPressedThisFrame) {
            transform.Rotate(new Vector3(0, 30, 0));
        }

        if (UnityEngine.InputSystem.Keyboard.current.rightArrowKey.wasPressedThisFrame) {
            transform.Rotate(new Vector3(0, -30, 0));
        }

        if (UnityEngine.InputSystem.Keyboard.current.lKey.wasPressedThisFrame) {
            depthModeEnabled = !depthModeEnabled;

            if (depthModeEnabled)
                camera.depthTextureMode = DepthTextureMode.Depth;
            else
                camera.depthTextureMode = DepthTextureMode.None;
        }
    }
}
