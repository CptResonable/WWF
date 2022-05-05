using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    public int selectedCamera = 0;
    public GameObject[] cameras;

    [HideInInspector] public Camera fpCamera; 
    //private float targetFOV = Settings.FOV;

    public event Delegates.IntDelegate cameraToggledEvent;

    private void Awake() {

        // Disable all cameras.
        for (int i = 0; i < cameras.Length; i++) {
            cameras[i].SetActive(false);
        }

        // Enable selected camera.
        cameras[selectedCamera].SetActive(true);

        // Get first person camera.
        fpCamera = cameras[0].GetComponent<Camera>();
    }

    private void Update() {

        // Toggle camera.
        if (UnityEngine.InputSystem.Keyboard.current.pKey.wasPressedThisFrame) {
            cameras[selectedCamera].SetActive(false);

            selectedCamera++;
            selectedCamera %= cameras.Length;

            cameras[selectedCamera].SetActive(true);

            cameraToggledEvent?.Invoke(selectedCamera);
        }
    }
}
