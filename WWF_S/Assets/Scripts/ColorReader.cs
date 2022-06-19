using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorReader : MonoBehaviour{
    public static ColorReader i;

    public Color color;
    [SerializeField] private RenderTexture renderTexture;

    private void Awake() {
        i = this;
    }

    public void Move(Vector3 position, Vector3 forward) {
        transform.position = position - forward * 0.2f;
        transform.forward = forward;
    }

    public Color ReadColor() {
        Texture2D texture = new Texture2D(5, 5, TextureFormat.RGB24, false);
        Rect rectReadPicture = new Rect(0, 0, 5, 5);
        RenderTexture.active = renderTexture;

        // Read pixels
        texture.ReadPixels(rectReadPicture, 0, 0);
        texture.Apply();

        RenderTexture.active = null; // added to avoid errors 

        color = texture.GetPixel(3, 3);

        return color;
    }
}
