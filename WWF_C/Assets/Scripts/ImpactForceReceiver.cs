using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactForceReceiver : MonoBehaviour {
    private Rigidbody rb;

    public delegate void ImpactForceReceivedDelegate(ImpactForceReceiver impactForceReceiver, Vector3 force, Vector3 position);
    public event ImpactForceReceivedDelegate impactForceReceivedEvent;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public void ReceiveForce(Vector3 force, Vector3 position) {
        rb.AddForceAtPosition(force, position);
        impactForceReceivedEvent?.Invoke(this, force, position);
    }
}
