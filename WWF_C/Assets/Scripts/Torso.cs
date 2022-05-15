using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Torso {
    public enum State { neutral, hipFire, ads }
    public State lastState;
    public State state;

    public delegate void StateChangedDelegate(State newState);
    public event StateChangedDelegate stateChangedEvent;

    public Head head;
    public KeyframedAnimationUpdater keyframedAnimationUpdater;
    public ArmLeft armL;
    public ArmRight armR;
    [SerializeField] private Transform pelvisRef;
    private CharacterLS character;
    private Bodypart bpPelvis, bpTorso1, bpTorso2, bpHead;

    [HideInInspector] public Vector3 aimOffset;
    [SerializeField] private Vector3 hipFireOffset;
    [SerializeField] private Vector3 adsOffset;

    [HideInInspector] public float runYawOffset;

    private float adsTorsoRotation = 30;

    public void Initialize(CharacterLS character) {
        this.character = character;
        armL.Initialize(character);
        armR.Initialize(character);
        head.Initialize(character);
        keyframedAnimationUpdater.Inititialize(character);
        bpPelvis = character.body.pelvis;
        bpTorso1 = character.body.torso_1;
        bpTorso2 = character.body.torso_2;
        bpHead = character.body.head;

        character.updateEvent += Character_updateEvent;
        character.lateUpdateEvent += Character_lateUpdateEvent;
        //character.legController.stepStartedEvent += LegController_stepStartedEvent;

        character.input.toggleAds.keyDownEvent += ToggleAds_keyDownEvent;
        character.equipment.itemEquipedEvent += Equipment_itemEquipedEvent;
    }

    private void Character_updateEvent() {
    }

    private void Character_lateUpdateEvent() {
        UpdateUpperBody();
    }

    /// <summary> Calculate rotation targets of upper body, all is called from here in order to execute functions in the right order </summary>
    private void UpdateUpperBody() {
        character.LeanController.DoLean();
        keyframedAnimationUpdater.Update();
        head.CalculateHeadTargetRotation();
        UpdateUpperBody_turnTowardsHead();
        head.AddAdsHeadTilt();
        head.CalculateEyePositionAndRotation();
        armR.CalculateHandPosRot();
        armL.CalculateHandPosRot();
    }

    private void UpdateUpperBody_turnTowardsHead() {
        Quaternion qT1 = QuaternionHelpers.DeltaQuaternion(character.LeanController.tLeanPivot.rotation, bpTorso1.target.rotation);
        Quaternion qT2 = bpTorso2.target.localRotation;

        bpTorso1.target.rotation = Quaternion.Slerp(character.LeanController.tLeanPivot.rotation, character.body.head.ikTarget.rotation, 0.2f);
        bpTorso2.target.rotation = Quaternion.Slerp(character.LeanController.tLeanPivot.rotation, character.body.head.ikTarget.rotation, 0.50f);
        character.body.head.target.rotation = character.body.head.ikTarget.rotation;

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

        stateChangedEvent?.Invoke(state);
    }
}