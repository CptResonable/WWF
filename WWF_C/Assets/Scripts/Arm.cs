using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VacuumBreather;

[System.Serializable]
public class Arm {
    [SerializeField] protected Transform tCompensator;
    [SerializeField] protected Transform tOffHandGripPosition;
    [SerializeField] private Vector3 pidValues;
    [SerializeField] protected Transform tHandRotationTarget;

    public Enums.Side side;
    public Transform tGripPosition;
    public FixedJoint handGrip;

    protected CharacterLS character;
    protected Bodypart bpArm_1, bpArm_2, bpHand;

    protected Vector3 error;

    public virtual void Initialize(CharacterLS character) {
        this.character = character;

        character.updateEvent += Character_updateEvent;
        character.lateUpdateEvent += Character_lateUpdateEvent;
        character.equipment.itemEquipedEvent += Equipment_itemEquipedEvent;
    }

    protected virtual void Character_updateEvent() {
    }

    protected virtual void Character_lateUpdateEvent() {
    }

    protected virtual void Equipment_itemEquipedEvent(Equipment.Type type, Equipable item) {

    }

    protected IkTargetStruct InterpolateTargetStruct(IkTargetStruct lastStruct, IkTargetStruct newStruct, float t) {
        IkTargetStruct targetStruct = new IkTargetStruct();
        targetStruct.targetPosition = Vector3.Lerp(lastStruct.targetPosition, newStruct.targetPosition, t);
        targetStruct.targetRotation = Quaternion.Slerp(lastStruct.targetRotation, newStruct.targetRotation, t);
        targetStruct.polePosition = Vector3.Lerp(lastStruct.polePosition, newStruct.polePosition, t);
        return targetStruct;
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
