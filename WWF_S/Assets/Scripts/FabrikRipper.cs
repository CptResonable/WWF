using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabrikRipper : MonoBehaviour {
    [SerializeField] private SkinnedMeshRenderer smr;
    [SerializeField] private RenderTexture rt;
    [SerializeField] List<VertexDistancePair> closestPoints;

    //public int T;
    // private void Update() {
    //     int offset = T % 3;
    //     Debug.Log(offset);
    // }

    // private void Update() {
    //     closestPoints = new List<VertexDistancePair>() {
    //         new VertexDistancePair(0, 999999, Vector3.zero), new VertexDistancePair(0, 999999, Vector3.zero), new VertexDistancePair(0, 999999, Vector3.zero)
    //     };
    //     for (int i = 0; i < smr.sharedMesh.vertexCount; i++) {
    //         float distance = Vector3.Distance(transform.position, smr.transform.TransformPoint(smr.sharedMesh.vertices[i]));
    //         if (distance < closestPoints[2].distance) {
    //             VertexDistancePair vertexDistancePair = new VertexDistancePair(i, distance,smr.transform.TransformPoint(smr.sharedMesh.vertices[i]));

    //             if (distance < closestPoints[1].distance) {
    //                 if (distance < closestPoints[0].distance) {
    //                     closestPoints.Insert(0, vertexDistancePair);
    //                 }
    //                 else {
    //                     closestPoints.Insert(1, vertexDistancePair);
    //                 }
    //             }
    //             else {
    //                 closestPoints.Insert(2, vertexDistancePair);
    //             }
    //             closestPoints.RemoveAt(3);
    //         }       
    //     }

    //     Vector3 p0 = smr.transform.TransformPoint(smr.sharedMesh.vertices[closestPoints[0].vertex]);
    //     Vector3 p1 = smr.transform.TransformPoint(smr.sharedMesh.vertices[closestPoints[1].vertex]);
    //     Vector3 p2 = smr.transform.TransformPoint(smr.sharedMesh.vertices[closestPoints[2].vertex]);
          
    //     Vector3 normal = Vector3.Cross((p1-p0).normalized, (p2-p0).normalized);

    //     GizmoManager.i.DrawSphere(Time.deltaTime, Color.red, p0, 0.02f);
    //     GizmoManager.i.DrawSphere(Time.deltaTime, Color.green, p1, 0.02f);
    //     GizmoManager.i.DrawSphere(Time.deltaTime, Color.blue, p2, 0.02f);

    //     //Project our point onto the plane
    //     Vector3 projectedPoint = smr.transform.InverseTransformPoint(transform.position + Vector3.Dot((p0 - transform.position), normal) * normal);

    //     //Vector3 projectedPoint = Vector3.ProjectOnPlane(Vector3.Cross(VectorUtils.FromToVector(smr.sharedMesh.vertices[closestPoints[0].vertex], smr.sharedMesh.vertices[closestPoints[1].vertex))
    //     Vector3 test = (smr.sharedMesh.vertices[closestPoints[0].vertex] + smr.sharedMesh.vertices[closestPoints[1].vertex] + smr.sharedMesh.vertices[closestPoints[2].vertex]) / 3;

    //     var b = new Barycentric(smr.sharedMesh.vertices[closestPoints[0].vertex], smr.sharedMesh.vertices[closestPoints[1].vertex], smr.sharedMesh.vertices[closestPoints[2].vertex], test);
    //     Vector2 uv_D = b.Interpolate(smr.sharedMesh.uv[closestPoints[0].vertex], smr.sharedMesh.uv[closestPoints[1].vertex], smr.sharedMesh.uv[closestPoints[2].vertex]);
    //     Debug.Log(uv_D.x);

    //     DrawTexture(uv_D.x, 1 - uv_D.y);
    //     //Debug.Log(closestPoints[0].distance + "   " + closestPoints[1].distance + "   " + closestPoints[2].distance);

    // }

    private void Update() {
        closestPoints = new List<VertexDistancePair>() {
            new VertexDistancePair(0, 999999, Vector3.zero), new VertexDistancePair(0, 999999, Vector3.zero), new VertexDistancePair(0, 999999, Vector3.zero)
        };

        Mesh mesh = smr.sharedMesh;

        var closestPointCalculator = new BaryCentricDistance(smr);
        var result = closestPointCalculator.GetClosestTriangleAndPoint(transform.position);
        var closest = result.closestPoint;

        VertexDistancePair closestPoint = new VertexDistancePair(0, 999999, Vector3.zero);
        for (int i = 0; i < smr.sharedMesh.vertexCount; i++) {
            float distance = Vector3.Distance(transform.position, smr.transform.TransformPoint(mesh.vertices[i]));
            if (distance < closestPoint.distance) {
                VertexDistancePair vertexDistancePair = new VertexDistancePair(i, distance, smr.transform.TransformPoint(mesh.vertices[i]));
                closestPoint = vertexDistancePair;
            }      
        }

        List<Vector3Int> tris = new List<Vector3Int>();
        for (int i = 0; i < mesh.triangles.Length; i++) {
            if (mesh.triangles[i] == closestPoint.vertex) {
                int offset = i % 3;

                Vector3Int ts = new Vector3Int(mesh.triangles[i] - offset, mesh.triangles[i] - offset + 1, mesh.triangles[i] - offset + 2);
                tris.Add(ts);

                Vector3 p0 = smr.transform.TransformPoint(smr.sharedMesh.vertices[mesh.triangles[ts.x]]);
                Vector3 p1 = smr.transform.TransformPoint(smr.sharedMesh.vertices[mesh.triangles[ts.y]]);
                Vector3 p2 = smr.transform.TransformPoint(smr.sharedMesh.vertices[mesh.triangles[ts.z]]);
                
                GizmoManager.i.DrawSphere(Time.deltaTime, Color.red, p0, 0.02f);
                GizmoManager.i.DrawSphere(Time.deltaTime, Color.green, p1, 0.02f);
                GizmoManager.i.DrawSphere(Time.deltaTime, Color.blue, p2, 0.02f);
                break;
                Vector3 normal = Vector3.Cross((p1-p0).normalized, (p2-p0).normalized);
                Vector3 p = smr.transform.InverseTransformPoint(transform.position + Vector3.Dot((p0 - transform.position), normal) * normal);
                GizmoManager.i.DrawSphere(Time.deltaTime, Color.red, transform.position + Vector3.Dot((p0 - transform.position), normal) * normal, 0.02f);

                float a0 = Vector3.Angle(VectorUtils.FromToVector(p, p0).normalized, VectorUtils.FromToVector(p, p1).normalized);
                float a1 = Vector3.Angle(VectorUtils.FromToVector(p, p1).normalized, VectorUtils.FromToVector(p, p2).normalized);
                float a2 = Vector3.Angle(VectorUtils.FromToVector(p, p2).normalized, VectorUtils.FromToVector(p, p0).normalized);

                Debug.Log((a0 + a1 + a2) * Mathf.Rad2Deg);
                if (a0 + a1 + a2 > 350) {
                    GizmoManager.i.DrawSphere(Time.deltaTime, Color.red, p0, 0.02f);
                    GizmoManager.i.DrawSphere(Time.deltaTime, Color.green, p1, 0.02f);
                    GizmoManager.i.DrawSphere(Time.deltaTime, Color.blue, p2, 0.02f);
                    break;
                }
            } 
        }

        // Vector3 p0 = smr.transform.TransformPoint(smr.sharedMesh.vertices[closestPoints[0].vertex]);
        // Vector3 p1 = smr.transform.TransformPoint(smr.sharedMesh.vertices[closestPoints[1].vertex]);
        // Vector3 p2 = smr.transform.TransformPoint(smr.sharedMesh.vertices[closestPoints[2].vertex]);
          
        // Vector3 normal = Vector3.Cross((p1-p0).normalized, (p2-p0).normalized);

        // GizmoManager.i.DrawSphere(Time.deltaTime, Color.red, p0, 0.02f);
        // GizmoManager.i.DrawSphere(Time.deltaTime, Color.green, p1, 0.02f);
        // GizmoManager.i.DrawSphere(Time.deltaTime, Color.blue, p2, 0.02f);

        // //Project our point onto the plane
        // Vector3 projectedPoint = smr.transform.InverseTransformPoint(transform.position + Vector3.Dot((p0 - transform.position), normal) * normal);

        // //Vector3 projectedPoint = Vector3.ProjectOnPlane(Vector3.Cross(VectorUtils.FromToVector(smr.sharedMesh.vertices[closestPoints[0].vertex], smr.sharedMesh.vertices[closestPoints[1].vertex))
        // Vector3 test = (smr.sharedMesh.vertices[closestPoints[0].vertex] + smr.sharedMesh.vertices[closestPoints[1].vertex] + smr.sharedMesh.vertices[closestPoints[2].vertex]) / 3;

        // var b = new Barycentric(smr.sharedMesh.vertices[closestPoints[0].vertex], smr.sharedMesh.vertices[closestPoints[1].vertex], smr.sharedMesh.vertices[closestPoints[2].vertex], test);
        // Vector2 uv_D = b.Interpolate(smr.sharedMesh.uv[closestPoints[0].vertex], smr.sharedMesh.uv[closestPoints[1].vertex], smr.sharedMesh.uv[closestPoints[2].vertex]);
        // //Debug.Log(uv_D.x);

        // DrawTexture(result.uvw.x, 1 - result.uvw.y);
        // //Debug.Log(closestPoints[0].distance + "   " + closestPoints[1].distance + "   " + closestPoints[2].distance);

    }

    // private void Update() {
    //     closestPoints = new List<VertexDistancePair>() {
    //         new VertexDistancePair(0, 999999, Vector3.zero), new VertexDistancePair(0, 999999, Vector3.zero), new VertexDistancePair(0, 999999, Vector3.zero)
    //     };

    //     Mesh mesh = smr.sharedMesh;

        


    //     var closestPointCalculator = new BaryCentricDistance(smr);
    //     var result = closestPointCalculator.GetClosestTriangleAndPoint(transform.position);
    //     var closest = result.closestPoint;

    //     var b = new Barycentric(smr.sharedMesh.vertices[mesh.triangles[result.triangle]], smr.sharedMesh.vertices[mesh.triangles[result.triangle + 1]], smr.sharedMesh.vertices[mesh.triangles[result.triangle] + 2], smr.transform.InverseTransformPoint(closest));
    //     Vector2 uv_D = b.Interpolate(smr.sharedMesh.uv[mesh.triangles[result.triangle]], smr.sharedMesh.uv[mesh.triangles[result.triangle + 1]], smr.sharedMesh.uv[mesh.triangles[result.triangle + 2]]);
    //     DrawTexture(1 - uv_D.x, 1 - uv_D.y);

    //     Debug.Log(uv_D);
    //     GizmoManager.i.DrawSphere(Time.deltaTime, Color.red, result.closestPoint, 0.02f);
    // }

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
        Graphics.DrawTexture(new Rect(posX * 256 - 25, posY * 256 - 25, 50, 50), brushTexture);
        //Graphics.DrawTexture(new Rect(256 / 2 - 50, 256 / 2 - 50, 100, 100), brushTexture);
        //Graphics.DrawTexture(new Rect());
        //Graphics.DrawTexture(new Rect(posX - bTex.width / brushSize, (rt.height - posY) - bTex.height / brushSize, bTex.width / (brushSize * 0.5f), bTex.height / (brushSize * 0.5f)), bTex);
        GL.PopMatrix();
        RenderTexture.active = null;// turn off rendertexture
    }

    [System.Serializable]
    private struct VertexDistancePair {
        public int vertex;
        public float distance;
        public Vector3 position;

        public VertexDistancePair(int vertex, float distance, Vector3 position) {
            this.vertex = vertex;
            this.distance = distance;
            this.position = position;
        }
    }
    // private void Update() {
    //     RaycastHit hit;
    //     hit.textureCoord
    // }    
}
