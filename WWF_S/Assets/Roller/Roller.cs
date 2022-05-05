using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Roller : MonoBehaviour {
    [SerializeField] private Vector3 angularVelocity;

    private Rigidbody rb;
    private Vector3 lastAngularVelocity;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        Vector3 error = angularVelocity - rb.angularVelocity;
        rb.angularVelocity = angularVelocity + error;
    }
}
