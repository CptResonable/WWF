using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeanController {
    private CharacterLS character;
    [SerializeField] private Transform tOffset;
    [SerializeField] public Transform tLeanPivot;
    [SerializeField] private float accelerationLeanAmount;
    [SerializeField] private float velocityLeanAmount;
    [SerializeField] private float leanChangeSpeed;

    public Vector3 positionOffset;
    private Vector3 basePosition;

    public void Initialize(CharacterLS character) {
        this.character = character;
        basePosition = tOffset.localPosition;
        character.updateEvent += Update;
    }

    //private void Update() {
    //    //tOffset.localPosition = character.telemetry.xzAccelerationLocal * accelerationLeanAmount;
    //    lean = Quaternion.Slerp(lean, AccelerationLean() * VelocityLean(), leanChangeSpeed * Time.deltaTime);
    //    lean = Quaternion.Inverse(character.rbMain.transform.rotation) * lean;
    //    tOffset.localPosition = basePosition + positionOffset;

    //    Vector3 tiltAxis = Vector3.Cross(character.telemetry.xzVelocity.normalized, Vector3.up);
    //    GizmoManager.i.DrawLine(Time.deltaTime, Color.red, character.rbMain.position, character.rbMain.position + tiltAxis * 2);
    //    Quaternion velocityRotation = Quaternion.AngleAxis(character.telemetry.xzVelocity.magnitude * velocityLeanAmount, tiltAxis);

    //    tLeanPivot.localRotation = Quaternion.identity;
    //    tLeanPivot.Rotate(tiltAxis * character.telemetry.xzVelocity.magnitude, velocityLeanAmount, Space.World);

    //}

    private void Update() {


    }

    public void ApplyVelocityAndAccelerationLean() {
        tOffset.localPosition = basePosition + positionOffset;

        Vector3 lean = VelocityLean() + AccelerationLean();
        tLeanPivot.localRotation = Quaternion.identity;
        tLeanPivot.Rotate(lean, Space.World);
    }


    private Vector3 AccelerationLean() {
        Vector3 tiltAxis = Vector3.Cross(character.telemetry.xzAcceleration.normalized, Vector3.up);
        GizmoManager.i.DrawLine(Time.deltaTime, Color.red, character.rbMain.position, character.rbMain.position + tiltAxis * 2);
        Quaternion accelerationRotation = Quaternion.AngleAxis(-character.telemetry.acceleration.magnitude * accelerationLeanAmount, tiltAxis);
        return -character.telemetry.xzAcceleration.magnitude * accelerationLeanAmount * tiltAxis;
    }

    private Vector3 VelocityLean() {
        Vector3 tiltAxis = Vector3.Cross(character.telemetry.xzVelocity.normalized, Vector3.up);
        //GizmoManager.i.DrawLine(Time.deltaTime, Color.red, character.rbMain.position, character.rbMain.position + tiltAxis * 2);
        //Quaternion velocityRotation = Quaternion.AngleAxis(character.telemetry.xzVelocity.magnitude * velocityLeanAmount, tiltAxis);
        return -character.telemetry.xzVelocity.magnitude * velocityLeanAmount * tiltAxis;
    }


    //private Quaternion AccelerationLean() {
    //    Vector3 tiltAxis = Vector3.Cross(character.tMain.InverseTransformVector(character.telemetry.acceleration.normalized), Vector3.up);
    //    //GizmoManager.i.DrawLine(Time.deltaTime, Color.red, character.body.armature.ragdoll.position, character.body.armature.ragdoll.position + tiltAxis * 2);
    //    // Debug.Log(-character.telemetry.acceleration.magnitude);
    //    Quaternion accelerationRotation = Quaternion.AngleAxis(-character.telemetry.acceleration.magnitude * accelerationLeanAmount, tiltAxis);
    //    return accelerationRotation;
    //}

    //private Quaternion VelocityLean() {
    //    Vector3 tiltAxis = Vector3.Cross(character.tMain.InverseTransformVector(character.telemetry.xzVelocity.normalized), Vector3.up);
    //    //GizmoManager.i.DrawLine(Time.deltaTime, Color.red, character.body.armature.ragdoll.position, character.body.armature.ragdoll.position + tiltAxis * 2);
    //    // Debug.Log(-character.telemetry.acceleration.magnitude);
    //    Quaternion velocityRotation = Quaternion.AngleAxis(-character.telemetry.xzVelocity.magnitude * velocityLeanAmount, tiltAxis);
    //    return velocityRotation;
    //}

}
