using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShowUV), true)]
public class ShowUVsOnClick : Editor
{
    Camera c;
    public Shader s;
    RenderTexture prov;
    public RenderTexture RenderTex;
    public float resolutionMultiplier = 2;
    public bool useUv2 = false;
    Texture2D Tex;

    void OnSceneGUI()
    {
        Event e = Event.current;
        if(e.type == EventType.MouseDown)
        {
            if(Event.current.button == 0)
            {
                c = Camera.current;
                if (c == null)
                    return;
                float w = c.scaledPixelWidth;
                float h = c.scaledPixelHeight;
                if(RenderTex == null)
                {
                    RenderTex = new RenderTexture(1, 1, 24);
                }
                RenderTex.Release();
                RenderTex.width = (int) (w * resolutionMultiplier);
                RenderTex.height = (int) (h * resolutionMultiplier);
                RenderTex.Create();
                Tex = new Texture2D(RenderTex.width,RenderTex.height);
                prov = c.targetTexture;
                c.targetTexture = RenderTex;
                Shader.SetGlobalInt("_UseUV2", useUv2?1:0);
                c.RenderWithShader(s, "");
                RenderTexture.active = RenderTex;
                Tex.ReadPixels(new Rect(0, 0, RenderTex.width, RenderTex.height), 0, 0);
                Vector2 pos = new Vector2(((e.mousePosition.x / w) * RenderTex.width), ((e.mousePosition.y / h) * RenderTex.height));
                Color col = Tex.GetPixel(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(RenderTex.height - pos.y));
                // if (col.a == 0.0f)
                // {
                    Debug.Log(col.r + ", " + col.g);
                //}
                c.targetTexture = prov;
                RenderTexture.active = null;
            }
        }
    }
}
