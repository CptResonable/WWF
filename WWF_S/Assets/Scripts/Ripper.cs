using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ripper : MonoBehaviour {
    [SerializeField] private SkinnedMeshRenderer smr;
    [SerializeField] private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        RaycastHitRenderer hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (SuperRaycast.Raycast(ray, smr, out hit)) {
                GizmoManager.i.DrawSphere(Time.deltaTime, Color.green, hit.point, 0.02f);
                Debug.Log(hit.uv);
                DrawTexture(hit.uv.x, 1 - hit.uv.y);
            }
        }
    }

    [SerializeField] private RenderTexture rt;
    public Texture2D brushTexture;
    public int resolution = 256;
    public float brushSize;

    private void DrawTexture(float posX, float posY)
    {
        RenderTexture.active = rt; // activate rendertexture for drawtexture;
        GL.PushMatrix();                       // save matrixes
        GL.LoadPixelMatrix(0, resolution, resolution, 0);      // setup matrix for correct size

        Texture2D bTex = brushTexture;
        // draw brushtexture
        Graphics.DrawTexture(new Rect(posX * 256 - 25, posY * 256 - (brushSize / 2), brushSize, brushSize), brushTexture);
        //Graphics.DrawTexture(new Rect(256 / 2 - 50, 256 / 2 - 50, 100, 100), brushTexture);
        //Graphics.DrawTexture(new Rect());
        //Graphics.DrawTexture(new Rect(posX - bTex.width / brushSize, (rt.height - posY) - bTex.height / brushSize, bTex.width / (brushSize * 0.5f), bTex.height / (brushSize * 0.5f)), bTex);
        GL.PopMatrix();
        RenderTexture.active = null;// turn off rendertexture
    }
}
