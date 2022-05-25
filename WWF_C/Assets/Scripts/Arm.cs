using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VacuumBreather;

[System.Serializable]
public class Arm {
    const float RUN_AIM_TRANSITION_SPEED = 5;

    [SerializeField] protected Transform tCompensator;
    [SerializeField] protected Transform tOffHandGripPosition;

    public Enums.Side side;
    public Transform tGripPosition;
    public FixedJoint handGrip;

    public float lerperTest;
    public TWrapper handTargetRunAimInterpolator = new TWrapper(0, 1, 0);

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
    }

    protected virtual void Equipment_itemUnequipedEvent(Equipment.Type type, Equipable item, ushort characterId) {
    }

    protected virtual void Locomotion_sprintChangedEvent(bool isSprinting) {

        // Transition smoothing from run animation to aim ik rig
        float target = isSprinting? 0 : 1;
        character.StartCoroutine(InterpolationUtils.i.SmoothStep(handTargetRunAimInterpolator.t, target, RUN_AIM_TRANSITION_SPEED, handTargetRunAimInterpolator, HandTargetRunAimTransitionComplete));
    }

    protected virtual void HandTargetRunAimTransitionComplete() {
    }

    protected IkTargetStruct InterpolateTargetStruct(IkTargetStruct lastStruct, IkTargetStruct newStruct, float t) {
        IkTargetStruct targetStruct = new IkTargetStruct();
        targetStruct.targetPosition = Vector3.Lerp(lastStruct.targetPosition, newStruct.targetPosition, t);
        targetStruct.targetRotation = Quaternion.Slerp(lastStruct.targetRotation, newStruct.targetRotation, t);
        targetStruct.polePosition = Vector3.Lerp(lastStruct.polePosition, newStruct.polePosition, t);
        return targetStruct;
    }

    public virtual void CalculateArm() {
    }

    public virtual void SetArmRotations() {
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
