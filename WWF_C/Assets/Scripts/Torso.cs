using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Torso {
    public enum State { neutral, hipFire, ads }
    public State lastState;
    public State state;

    public ArmLeft armL;
    public ArmRight armR;
    [SerializeField] private Transform pelvisRef;
    private CharacterLS character;
    private Bodypart bpPelvis, bpTorso1, bpTorso2, bpHead;
    [SerializeField] private AnimalAnimator animTest;

    [HideInInspector] public Vector3 aimOffset;
    [SerializeField] private Vector3 hipFireOffset;
    [SerializeField] private Vector3 adsOffset;

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

        character.input.toggleAds.keyDownEvent += ToggleAds_keyDownEvent;
        character.equipment.itemEquipedEvent += Equipment_itemEquipedEvent;
    }

    private void Character_updateEvent() {
        UpperBodyAnimation();
    }

    private void Character_lateUpdateEvent() {
        Quaternion qT1 = QuaternionHelpers.DeltaQuaternion(character.rbMain.rotation, bpTorso1.target.rotation);
        Quaternion qT2 = bpTorso2.target.localRotation;

        bpTorso1.target.rotation = Quaternion.Slerp(character.rbMain.rotation, character.head.iktEyes.rotation, 0.2f);
        bpTorso2.target.rotation = Quaternion.Slerp(character.rbMain.rotation, character.head.iktEyes.rotation, 0.50f);
        character.body.head.target.rotation = character.head.iktEyes.rotation;

        bpTorso1.target.localRotation *= qT1;// * qT1;
        bpTorso2.target.localRotation *= qT2;
    }

    private void Equipment_itemEquipedEvent(Equipment.Type type, Equipable item) {

        if (type == Equipment.Type.gun) {
            SetState(State.hipFire);
        }
    }

    private void ToggleAds_keyDownEvent() {
        if (state == State.hipFire)
            SetState(State.ads);
        else if (state == State.ads)
            SetState(State.hipFire);
    }

    private void SetState(State newState) {
        lastState = state;
        state = newState;

        if (state == State.hipFire)
            aimOffset = hipFireOffset;
        else if (state == State.ads)
            aimOffset = adsOffset;
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