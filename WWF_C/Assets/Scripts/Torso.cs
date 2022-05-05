using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Torso {
    public Arm armL;
    public Arm armR;
    [SerializeField] private Transform pelvisRef;
    private CharacterLS character;
    private Bodypart bpPelvis, bpTorso1, bpTorso2, bpHead;
    [SerializeField] private AnimalAnimator animTest;

    [SerializeField] public float aimOriginHeightOffset;
    [SerializeField] public float aimForwardDistance;

    [HideInInspector] public float runYawOffset;

    public void Initialize(CharacterLS character) {
        this.character = character;
        armL.Initialize(character);
        armR.Initialize(character);
        bpPelvis = character.body.pelvis;
        bpTorso1 = character.body.torso_1;
        bpTorso2 = character.body.torso_2;
        bpHead = character.body.head;

        character.updateEvent += Character_updateEvent;
        character.lateUpdateEvent += Character_lateUpdateEvent;
        character.legController.stepStartedEvent += LegController_stepStartedEvent;
    }

    private void Character_updateEvent() {
        // Quaternion adjustedPelvisRotation = bpPelvis.ikTarget.rotation * Quaternion.Euler(bpPelvis.ikTarget.InverseTransformVector(new Vector3(-180, 0, 0)));
        // bpTorso1.target.rotation = Quaternion.Lerp(pelvisRef.rotation, bpHead.target.rotation, 0.25f) * Quaternion.Euler(0, runYawOffset / 2, 0);
        // bpTorso2.target.rotation = Quaternion.Lerp(pelvisRef.rotation, bpHead.target.rotation, 0.55f)  * Quaternion.Euler(0, runYawOffset, 0);
        // // bpTorso1.target.rotation = Quaternion.Lerp(pelvisRef.rotation, bpHead.target.rotation, 0.25f) * Quaternion.Euler(0, 30 / 2, 0);
        // // bpTorso2.target.rotation = Quaternion.Lerp(pelvisRef.rotation, bpHead.target.rotation, 0.55f)  * Quaternion.Euler(0, 60, 0);

        UpperBodyAnimation();
        //WeaponPoint_hip();
    }

    private void Character_lateUpdateEvent() {
        // Quaternion adjustedPelvisRotation = bpPelvis.ikTarget.rotation * Quaternion.Euler(bpPelvis.ikTarget.InverseTransformVector(new Vector3(-180, 0, 0)));
        // bpTorso1.target.rotation = Quaternion.Lerp(pelvisRef.rotation, bpHead.target.rotation, 0.25f) * Quaternion.Euler(0, runYawOffset / 2, 0);
        // bpTorso2.target.rotation = Quaternion.Lerp(pelvisRef.rotation, bpHead.target.rotation, 0.55f)  * Quaternion.Euler(0, runYawOffset, 0);
        // // bpTorso1.target.rotation = Quaternion.Lerp(pelvisRef.rotation, bpHead.target.rotation, 0.25f) * Quaternion.Euler(0, 30 / 2, 0);
        // // bpTorso2.target.rotation = Quaternion.Lerp(pelvisRef.rotation, bpHead.target.rotation, 0.55f)  * Quaternion.Euler(0, 60, 0);

        //UpperBodyAnimation();
        WeaponPoint_hip();
    }

    float from;
    float to;
    float t;
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
        Enums.Side stepSide = character.legController.lastStepSide;

        t = Mathf.Lerp(from, to, stepT) % 1;
        animTest.f = t;
    }

    private void WeaponPoint_hip() {
        Vector3 aimOrigin = character.tCamera.position + character.tCamera.up * aimOriginHeightOffset;
        Vector3 rightHandTarget = aimOrigin + character.tCamera.forward * aimForwardDistance;
        GizmoManager.i.DrawSphere(Time.deltaTime, Color.red, rightHandTarget, 0.15f);
        character.body.hand_R.ikTarget.position = rightHandTarget;
    }


    // private void UpperBodyAnimation() {
    //     float stepT = character.legController.stepT;
    //     Enums.Side stepSide = character.legController.lastStepSide;

    //     if (stepSide == Enums.Side.left) {
    //         t = Mathf.Lerp(0.25f, 0.75f, stepT);
    //     }
    //     else {
    //         t = Mathf.Lerp(0.75f, 1.25f, stepT) % 1;
    //     }
    //     animTest.f = t;
    // }

    //public void SetArmSwingValue(float t) {
    //    runYawOffset = t * 45;
    //    armL.SetArmSwing(t);
    //    armR.SetArmSwing(t);
    //}
}