using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InducedBodyForce : MonoBehaviour {
    [SerializeField] ConfigurableJoint joint;
    Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        rb.AddForce(-joint.currentForce);
        rb.AddTorque(-joint.currentTorque);
    }
}
