using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MainBodyController {
    public Rigidbody rb;
    [SerializeField] private float accelerationLeanAmount;
    [HideInInspector] public Quaternion targetRotation;
    [HideInInspector] public Quaternion accelerationLean;

    // private CharacterLS character;

    // public void Initialize(CharacterLS character) {
    //     this.character = character;

    //     character.fixedUpdateEvent += Character_fixedUpdateEvent;
    // }

    // private void Character_fixedUpdateEvent() {
    //     targetRotation = Quaternion.Euler(new Vector3(0, character.eyes.yaw, 0));
    //     accelerationLean = AccelerationLean();
    //     //accelerationLean = Quaternion.identity;
    //     character.body.armature.ikTarget.rotation = targetRotation * accelerationLean;
    // }

    // private Quaternion AccelerationLean() {
    //     Vector3 tiltAxis = Vector3.Cross(character.tMain.InverseTransformVector(character.telemetry.acceleration.normalized), Vector3.up);
    //     GizmoManager.i.DrawLine(Time.deltaTime, Color.red, character.body.armature.ragdoll.position, character.body.armature.ragdoll.position + tiltAxis * 2);
    //     // Debug.Log(-character.telemetry.acceleration.magnitude);
    //     Quaternion accelerationRotation = Quaternion.AngleAxis(-character.telemetry.acceleration.magnitude * accelerationLeanAmount, tiltAxis);
    //     return accelerationRotation;
    // }
}
