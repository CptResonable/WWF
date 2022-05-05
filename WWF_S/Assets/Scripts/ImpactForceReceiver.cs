using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactForceReceiver : MonoBehaviour {
    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public void ReceiveForce(Vector3 force, Vector3 position) {
        rb.AddForceAtPosition(force, position);
        Debug.Log("HIT!! " + force.magnitude);
    }
}
