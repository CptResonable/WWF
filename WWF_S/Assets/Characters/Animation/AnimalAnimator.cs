using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAnimator : MonoBehaviour {
    [SerializeField] private CharacterLS character;
    [SerializeField] private Animator animator;
    [SerializeField] public float f;

    public AvatarMask mask;

    public enum MaskTransformIndexes { armature, pelvis, leg_1_L, leg_2_L, foot_L, leg_1_R, leg_2_R, foot_R, 
        torso_1, torso_2, arm_1_L, arm_2_L, hand_L, handEnd_L, arm_1_R, arm_2_R, hand_R, handEnd_R, head, headEnd
    }
    
    private void Update() {
        animator.SetFloat("VelocityX", character.telemetry.xzVelocityLocal.x);
        animator.SetFloat("VelocityZ", character.telemetry.xzVelocityLocal.z);
        animator.Play("Walk", -1, f);
        animator.speed = 0;
    }
}
