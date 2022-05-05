using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformUtils {
    //public Component GetComponentInParents(Transform transform, Type type) {
    //    transform.GetComponentInParent<type>
    //}

    /// <summary> Move t1 from t2 to t3 and then back again. </summary>
    public static IEnumerator LerpMotion(Transform t1, Transform t2, Transform t3, float motionTime) {
        float time = 0;
        while (time < motionTime) {
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;

            float t = time / motionTime;

            if (t < 0.5f) { // from t2 to t3.
                t *= 2;
                t1.position = Vector3.Lerp(t2.position, t3.position, t);
                t1.rotation = Quaternion.Slerp(t2.rotation, t3.rotation, t);
            }
            else { // back from t3 to t2.
                t = (t - 0.5f) * 2;
                t1.position = Vector3.Lerp(t3.position, t2.position, t);
                t1.rotation = Quaternion.Slerp(t3.rotation, t2.rotation, t);
            }
        }
        yield return null;
    }

    /// <summary> Move t1 from t2 to t3 and then to t4. </summary>
    public static IEnumerator LerpMotion(Transform t1, Transform t2, Transform t3, Transform t4, float motionTime) {
        float time = 0;
        while (time < motionTime) {
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;

            float t = time / motionTime;

            if (t < 0.5f) { // from t2 to t3.
                t *= 2;
                t1.position = Vector3.Lerp(t2.position, t3.position, t);
                t1.rotation = Quaternion.Slerp(t2.rotation, t3.rotation, t);
            }
            else { // back from t3 to t4.
                t = (t - 0.5f) * 2;
                t1.position = Vector3.Lerp(t3.position, t4.position, t);
                t1.rotation = Quaternion.Slerp(t3.rotation, t4.rotation, t);
            }
        }
        yield return null;
    }
}
