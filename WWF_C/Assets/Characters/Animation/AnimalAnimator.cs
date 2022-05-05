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

    public AvatarMask mask;

    public enum MaskTransformIndexes { armature, pelvis, leg_1_L, leg_2_L, foot_L, leg_1_R, leg_2_R, foot_R, 
        torso_1, torso_2, arm_1_L, arm_2_L, hand_L, handEnd_L, arm_1_R, arm_2_R, hand_R, handEnd_R, head, headEnd
    }

    private void Awake() {
        
        controller = animator.runtimeAnimatorController;
        //for (int i = 0; i < 16; i++) {
        //    mask.SetTransformActive(i, false);
        //    //mask.SetHumanoidBodyPartActive(mask.get, false);
        //}
        //mask.SetTransformActive((int)MaskTransformIndexes.arm_1_R, false);
        //mask.SetTransformActive((int)MaskTransformIndexes.arm_2_R, false);
        //mask.SetTransformActive((int)MaskTransformIndexes.hand_R, false);
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
