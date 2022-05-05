using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointMotor : MonoBehaviour {
    public Quaternion deltaRot;
    [SerializeField] JointMotor jmParent;
    [SerializeField] private Transform target;
    [SerializeField] private bool configuredInWorldSpace;

    private ConfigurableJoint joint;
    private Quaternion startLocalRot;
    private Quaternion startWorldRot;

    private void Awake() {
        joint = GetComponent<ConfigurableJoint>();
        startLocalRot = transform.localRotation;
        startWorldRot = transform.rotation;
    }

    private void FixedUpdate() {


        Quaternion t = target.rotation;
        // if (jmParent != null)
        //     t *= Quaternion.Inverse(jmParent.deltaRot);

        if (configuredInWorldSpace)
            joint.SetTargetRotation(t, startWorldRot);
        else
            joint.SetTargetRotationLocal(target.localRotation, startLocalRot);

        //deltaRot = target.rotation * Quaternion.Inverse(transform.rotation);
    }
}
