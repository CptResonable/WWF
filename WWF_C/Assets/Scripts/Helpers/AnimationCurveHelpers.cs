using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationCurveHelpers {
    public static AnimationCurve InterpolateCurve(AnimationCurve curveA, AnimationCurve curveB, float t) {
        if (curveA.keys.Length != curveB.keys.Length) {
            Debug.LogError("Curves do not have same number of keys!");
            return curveA;
        }

        AnimationCurve newCurve = curveA;
        for (int i = 0; i < curveA.keys.Length; i++) {
            Keyframe newKey = curveA.keys[i];
            newKey.value = Mathf.Lerp(newKey.value, curveB.keys[i].value, t);
            newKey.time = Mathf.Lerp(newKey.time, curveB.keys[i].time, t);
            newKey.inTangent = Mathf.Lerp(newKey.inTangent, curveB.keys[i].inTangent, t);
            newKey.outTangent = Mathf.Lerp(newKey.outTangent, curveB.keys[i].outTangent, t);
            newKey.inWeight = Mathf.Lerp(newKey.inWeight, curveB.keys[i].inWeight, t);
            newKey.outWeight = Mathf.Lerp(newKey.outWeight, curveB.keys[i].outWeight, t);
            newCurve.MoveKey(i, newKey);
        }

        return newCurve;
    }
}
