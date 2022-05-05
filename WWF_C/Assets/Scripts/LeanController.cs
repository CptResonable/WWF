using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeanController {
    private CharacterLS character;
    [SerializeField] private Transform tOffset;
    [SerializeField] private Transform tLeanPivot;
    [SerializeField] private float accelerationLeanAmount;
    [SerializeField] private float velocityLeanAmount;
    [SerializeField] private float leanChangeSpeed;

    public Vector3 positionOffset;
    private Vector3 basePosition;
    private Quaternion lean;

    public void Initialize(CharacterLS character) {
        this.character = character;
        basePosition = tOffset.localPosition;
        character.updateEvent += Update;
    }

    private void Update() {
        //tOffset.localPosition = character.telemetry.xzAccelerationLocal * accelerationLeanAmount;
        lean = Quaternion.Slerp(lean, AccelerationLean() * VelocityLean(), leanChangeSpeed * Time.deltaTime);
        tOffset.localPosition = basePosition + positionOffset;
        tLeanPivot.localRotation = lean;

    }

    
    private Quaternion AccelerationLean() {
        Vector3 tiltAxis = Vector3.Cross(character.tMain.InverseTransformVector(character.telemetry.acceleration.normalized), Vector3.up);
        //GizmoManager.i.DrawLine(Time.deltaTime, Color.red, character.body.armature.ragdoll.position, character.body.armature.ragdoll.position + tiltAxis * 2);
        // Debug.Log(-character.telemetry.acceleration.magnitude);
        Quaternion accelerationRotation = Quaternion.AngleAxis(-character.telemetry.acceleration.magnitude * accelerationLeanAmount, tiltAxis);
        return accelerationRotation;
    }

    private Quaternion VelocityLean() {
        Vector3 tiltAxis = Vector3.Cross(character.tMain.InverseTransformVector(character.telemetry.xzVelocity.normalized), Vector3.up);
        //GizmoManager.i.DrawLine(Time.deltaTime, Color.red, character.body.armature.ragdoll.position, character.body.armature.ragdoll.position + tiltAxis * 2);
        // Debug.Log(-character.telemetry.acceleration.magnitude);
        Quaternion velocityRotation = Quaternion.AngleAxis(-character.telemetry.xzVelocity.magnitude * velocityLeanAmount, tiltAxis);
        return velocityRotation;
    }

}
