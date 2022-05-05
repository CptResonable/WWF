using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_camera : MonoBehaviour {

    private void Update() {
        Debug.Log("UPDATE");
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Debug.Log("RENDER!!");
    }
}
