using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VacuumBreather;

[System.Serializable]
public class Arm {
    const float RUN_AIM_TRANSITION_SPEED = 5f;

    [SerializeField] protected Transform tCompensator;
    [SerializeField] protected Transform tOffHandGripPosition;

    public enum ArmActionState { idle, aim, reload }
    public ArmActionState armActionState = ArmActionState.idle;

    public Enums.Side side;
    public Transform tGripPosition;
    public FixedJoint handGrip;

    public float lerperTest;
    public TWrapper handTargetRunAimInterpolator = new TWrapper(0, 1, 0);
    public Coroutine handTargetRunAimTransitionCorutine;

    protected CharacterLS character;
    protected Torso torso;
    protected Bodypart bpArm_1, bpArm_2, bpHand;
    protected Transform tAimRig_arm1, tAimRig_arm2, tAimRig_hand;

    protected Vector3 error;

    public virtual void Initialize(CharacterLS character) {
        this.character = character;
        this.torso = character.torso;

        character.updateEvent += Character_updateEvent;
        character.lateUpdateEvent += Character_lateUpdateEvent;
        character.equipment.itemEquipedEvent += Equipment_itemEquipedEvent;
        character.equipment.itemUnequipedEvent += Equipment_itemUnequipedEvent;
        character.locomotion.sprintChangedEvent += Locomotion_sprintChangedEvent;
    }

    protected virtual void Character_updateEvent() {
    }

    protected virtual void Character_lateUpdateEvent() {
    }

    protected virtual void Equipment_itemEquipedEvent(Equipment.Type type, Equipable item) {
        DetermineArmActionState();

        if (item.itemType == Equipment.Type.gun) {
            Gun gun = (Gun)item;
            gun.reloadStartedEvent += Gun_reloadStartedEvent;
            gun.reloadFinishedEvent += Gun_reloadFinishedEvent;
        }
    }

    protected virtual void Equipment_itemUnequipedEvent(Equipment.Type type, Equipable item, ushort characterId) {
        DetermineArmActionState();

        if (item.itemType == Equipment.Type.gun) {
            Gun gun = (Gun)item; 
            gun.reloadStartedEvent -= Gun_reloadStartedEvent;
            gun.reloadFinishedEvent -= Gun_reloadFinishedEvent;
        }

        DetermineArmActionState();
    }

    protected virtual void Locomotion_sprintChangedEvent(bool isSprinting) {

        //// Transition smoothing from run animation to aim ik rig
        //float target = isSprinting? 0 : 1;
        //character.StartCoroutine(InterpolationUtils.i.SmoothStep(handTargetRunAimInterpolator.t, target, RUN_AIM_TRANSITION_SPEED, handTargetRunAimInterpolator, HandTargetRunAimTransitionComplete));
        DetermineArmActionState();
    }

    public virtual void DetermineArmActionState() {
        ArmActionState newArmActionState = ArmActionState.idle;

        if (character.locomotion.isSprinting || character.equipment.equipedType == Equipment.Type.none) {
            newArmActionState = ArmActionState.idle;
        }
        else {
            if (character.equipment.equipedType == Equipment.Type.gun)
                newArmActionState = ArmActionState.aim; // TODO, check for reload
            else
                newArmActionState = ArmActionState.idle;
        }

        if (newArmActionState != armActionState) {
            float target = 0;
            if (newArmActionState == ArmActionState.idle)
                target = 0;
            else
                target = 1;

            if (handTargetRunAimTransitionCorutine != null)
                character.StopCoroutine(handTargetRunAimTransitionCorutine);

            if (handTargetRunAimInterpolator.t == target)
                HandTargetRunAimTransitionComplete();
            else
                handTargetRunAimTransitionCorutine = character.StartCoroutine(InterpolationUtils.i.SmoothStep(handTargetRunAimInterpolator.t, target, RUN_AIM_TRANSITION_SPEED, handTargetRunAimInterpolator, HandTargetRunAimTransitionComplete));
        }

        armActionState = newArmActionState;
    }

    protected virtual void HandTargetRunAimTransitionComplete() {
    }

    protected virtual void Gun_reloadStartedEvent(float reloadTime) {
        armActionState = ArmActionState.reload;
    }

    protected virtual void Gun_reloadFinishedEvent() {
        DetermineArmActionState();
    }

    protected IkTargetStruct InterpolateTargetStruct(IkTargetStruct lastStruct, IkTargetStruct newStruct, float t) {
        IkTargetStruct targetStruct = new IkTargetStruct();
        targetStruct.targetPosition = Vector3.Lerp(lastStruct.targetPosition, newStruct.targetPosition, t);
        targetStruct.targetRotation = Quaternion.Slerp(lastStruct.targetRotation, newStruct.targetRotation, t);
        targetStruct.polePosition = Vector3.Lerp(lastStruct.polePosition, newStruct.polePosition, t);
        return targetStruct;
    }

    public void CalculateArm() {
        switch (armActionState) {
            case ArmActionState.idle:
                UpdateState_idle();
                break;
            case ArmActionState.aim:
                UpdateState_aim();
                break;
            case ArmActionState.reload:
                UpdateState_reload();
                break;
        }
    }

    public virtual void UpdateState_idle() {
    }
    public virtual void UpdateState_aim() {
    }
    public virtual void UpdateState_reload() { 
    }

    protected virtual void InterpolateAimAndIdleRotations() {
        bpArm_1.target.rotation = Quaternion.Slerp(bpArm_1.target.rotation, tAimRig_arm1.rotation, handTargetRunAimInterpolator.t);
        bpArm_2.target.rotation = Quaternion.Slerp(bpArm_2.target.rotation, tAimRig_arm2.rotation, handTargetRunAimInterpolator.t);
        bpHand.target.rotation = Quaternion.Slerp(bpHand.target.rotation, tAimRig_hand.rotation, handTargetRunAimInterpolator.t);
    }

    //protected void Compensate() {
    //    Vector3 newError = VectorUtils.FromToVector(bpHand.ragdoll.position, bpHand.ikTarget.position);
    //    Vector3 dError = VectorUtils.FromToVector(error, newError);
    //    error = newError;

    //    PidController pidX = new PidController(pidValues.x, pidValues.y, pidValues.z);
    //    PidController pidY = new PidController(pidValues.x, pidValues.y, pidValues.z);
    //    PidController pidZ = new PidController(pidValues.x, pidValues.y, pidValues.z);

    //    Vector3 output;
    //    output.x = pidX.ComputeOutput(error.x, dError.x, Time.deltaTime);
    //    output.y = pidY.ComputeOutput(error.y, dError.y, Time.deltaTime);
    //    output.z = pidZ.ComputeOutput(error.z, dError.z, Time.deltaTime);

    //    tCompensator.position += output;
    //}
}

public struct IkTargetStruct {
    public Vector3 targetPosition;
    public Quaternion targetRotation;
    public Vector3 polePosition;
}
