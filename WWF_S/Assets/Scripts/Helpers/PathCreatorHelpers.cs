using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public static class PathCreatorHelpers {
    public static void VizualizePath(VertexPath vPath, Color color, float width) {
        if (vPath != null) {
            for (int i = 0; i < vPath.NumPoints; i++) {
                GizmoManager.i.DrawSphere(Time.deltaTime, color, vPath.GetPoint(i), width);
            }
        }
    }

    public static void VizualizePath(BezierPath bPath, VertexPath vPath, Vector3 vPos, List<Vector3> points, Color color, float width) {
        if (vPath != null) {
            for (int i = 0; i < vPath.NumPoints; i++) {
                GizmoManager.i.DrawSphere(Time.deltaTime, color, vPath.GetPoint(i), width);
            }

            GizmoManager.i.DrawSphere(Time.deltaTime, Color.cyan, points[0] + vPos, width * 2f);
            GizmoManager.i.DrawSphere(Time.deltaTime, Color.yellow, points[1] + vPos, width * 1.5f);
            GizmoManager.i.DrawSphere(Time.deltaTime, Color.yellow, points[2] + vPos, width * 1.5f);
            GizmoManager.i.DrawSphere(Time.deltaTime, Color.black, points[3] + vPos, width * 1.5f);
        }
    }
}
