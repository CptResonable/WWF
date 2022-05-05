using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VacuumBreather;

public class Roller2 : MonoBehaviour {
    [SerializeField] private Vector3 angularVelocity;
    [SerializeField] private Vector3 PID;

    private Rigidbody rb;
    //private Vector3 lastAngularVelocity;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    Vector3 lastAngularVelocity = Vector3.zero;
    private void FixedUpdate() {
        Vector3 error = (angularVelocity - rb.angularVelocity) / Time.deltaTime;

        Vector3 angularAccelleration = VectorUtils.FromToVector(lastAngularVelocity, rb.angularVelocity) / Time.deltaTime;
        lastAngularVelocity = rb.angularVelocity;

        PidQuaternionController pidController = new PidQuaternionController(PID.x, PID.y, PID.z);

        //pidController.ComputeRequiredAngularAcceleration(rb.rotation, Quaternion.identity, rb.angularVelocity)
        Vector3 acc = pidController.ComputeRequiredAngularAcceleration(Quaternion.Euler(rb.angularVelocity), Quaternion.identity, -angularAccelleration, Time.deltaTime);

        rb.AddTorque(acc);
    }
}
