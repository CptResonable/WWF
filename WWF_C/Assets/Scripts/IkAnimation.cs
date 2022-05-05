using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

[System.Serializable]
public class IkAnimation {
    [SerializeField] private Transform centre;
    [SerializeField] private Transform[] nodes;

    private BezierPath bPath;
    private VertexPath vPath;

    public void Initialize() {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < nodes.Length; i++) {
            points.Add(nodes[i].localPosition);
        }

        bPath = new BezierPath(points, false, PathSpace.xyz);
        vPath = new VertexPath(bPath, centre);
    }

    public Vector3 Evaluate(float t) {
        if (Mathf.Abs(t) >= 1)
            t /= Mathf.Abs(t);

        t = (t * 0.5f) + 0.5f;

        PathCreatorHelpers.VizualizePath(vPath, Color.red, 0.02f); // Visualize path
        return vPath.GetPointAtTime(t, EndOfPathInstruction.Stop);
    }
}
