using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuaternionHelpers {
    public static Quaternion DeltaQuaternion(Quaternion q1, Quaternion q2) {
        return q2 * Quaternion.Inverse(q1);
    }
}
