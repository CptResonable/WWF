using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VacuumBreather;

public class CopyRotationDynamic : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 PidValues;
    [SerializeField] private float angVelInfluence;
    Quaternion targetRotation = Quaternion.identity;

    private float lastError = 0; // Used to calculate delta error, only used in y and xz mode

    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        Full();
    }

    private void Full() {

        //Quaternion dRot = target.rotation * Quaternion.Inverse(targetRotation);
        Vector3 dDotV = ((target.rotation.eulerAngles - targetRotation.eulerAngles) * Mathf.Deg2Rad) / Time.deltaTime;
        // Quaternion.a
        // Quaternion angVelQ = new Quaternion((dRot.x / Time.deltaTime) * angVelInfluence, (dRot.y / Time.deltaTime) * angVelInfluence, (dRot.z / Time.deltaTime) * angVelInfluence, (dRot.w / Time.deltaTime) * angVelInfluence);

        Quaternion targetRotaion = target.rotation;

        PidQuaternionController pidController = new PidQuaternionController(PidValues.x, PidValues.y, PidValues.z);
        Vector3 output = pidController.ComputeRequiredAngularAcceleration(transform.rotation, target.rotation * Quaternion.Euler(dDotV * angVelInfluence), rb.angularVelocity, Time.deltaTime);
        rb.AddTorque(output, ForceMode.Acceleration);
        //rb.AddTorque(output * 0.2f, ForceMode.Force);
    }
}
