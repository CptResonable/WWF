using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAnimator : MonoBehaviour {
    [SerializeField] private Transform tCamera;
    [SerializeField] private Transform tCharacter;
    [SerializeField] private CharacterLS character;
    [SerializeField] private Transform torso1, torso2;
    [SerializeField] private Animator animator;
    [SerializeField] public float f;
    RuntimeAnimatorController controller;

    private void Awake() {
        controller = animator.runtimeAnimatorController;
    }
    
    private void Update() {
        animator.SetFloat("VelocityX", character.telemetry.xzVelocityLocal.x);
        animator.SetFloat("VelocityZ", character.telemetry.xzVelocityLocal.z);
        animator.Play("Walk", -1, f);
        animator.speed = 0;
    }

    private void LateUpdate() {
        Quaternion qT1 = QuaternionHelpers.DeltaQuaternion(tCharacter.rotation, torso1.rotation);
        Quaternion qT2 = torso2.localRotation;

        Quaternion headRot = character.body.head.target.rotation;
        torso1.rotation = Quaternion.Slerp(tCharacter.rotation, tCamera.rotation, 0.2f);
        torso2.rotation = Quaternion.Slerp(tCharacter.rotation, tCamera.rotation, 0.50f);
        character.body.head.target.rotation = tCamera.rotation;

        torso1.localRotation *= qT1;// * qT1;
        torso2.localRotation *= qT2;
    }
}
