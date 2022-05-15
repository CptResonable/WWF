using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyframedAnimationUpdater {
    private CharacterLS character;
    [SerializeField] private Animator animator;
    private float f;

    public AvatarMask mask;

    public enum MaskTransformIndexes {
        armature, pelvis, leg_1_L, leg_2_L, foot_L, leg_1_R, leg_2_R, foot_R,
        torso_1, torso_2, arm_1_L, arm_2_L, hand_L, handEnd_L, arm_1_R, arm_2_R, hand_R, handEnd_R, head, headEnd
    }

    public void Inititialize(CharacterLS character) {
        this.character = character;
        character.legController.stepStartedEvent += LegController_stepStartedEvent;
    }

    float from, to, t;
    private void LegController_stepStartedEvent() {
        Enums.Side stepSide = character.legController.lastStepSide;
        if (stepSide == Enums.Side.right) {
            from = t;
            to = 0.75f;
        }
        else {
            from = t;
            to = 1.25f;
        }
    }

    private void UpperBodyAnimation() {
        float stepT = character.legController.stepT;

        //Enums.Side stepSide = character.legController.lastStepSide;
        t = Mathf.Lerp(from, to, stepT) % 1;
        f = t;
    }


    public void Update() {
        UpperBodyAnimation();

        animator.SetFloat("VelocityX", character.telemetry.xzVelocityLocal.x);
        animator.SetFloat("VelocityZ", character.telemetry.xzVelocityLocal.z);
        animator.Play("Walk", -1, f);
        animator.speed = 0;
    }
}
