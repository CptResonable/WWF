using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VacuumBreather;

[System.Serializable]
public class Arm {
    [SerializeField] protected Transform tCompensator;
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

        //if (side == Enums.Side.left) {
        //    bpArm_1 = character.body.arm_1_L;
        //    bpArm_2 = character.body.arm_2_L;
        //    bpHand = character.body.hand_L;
        //}
        //else {
        //    bpArm_1 = character.body.arm_1_R;
        //    bpArm_2 = character.body.arm_2_R;
        //    bpHand = character.body.hand_R;
        //}

        character.updateEvent += Character_updateEvent;
        character.lateUpdateEvent += Character_lateUpdateEvent;
        character.equipment.itemEquipedEvent += Equipment_itemEquipedEvent;
    }

    protected virtual void Character_updateEvent() {
        
        //bpHand.target.rotation = tHandRotationTarget.rotation;
        //bpHand.ikTarget.rotation = tHandRotationTarget.rotation;

        //if (side == Enums.Side.left)
        //    return;

        //bpArm_1.target.rotation = character.body.armAimRig.arm_1_R.rotation;
        //bpArm_2.target.rotation = character.body.armAimRig.arm_2_R.rotation;
        //bpHand.target.rotation = character.body.armAimRig.hand_R.rotation;
    }
    protected virtual void Character_lateUpdateEvent() {
       
        //bpHand.target.rotation = tHandRotationTarget.rotation;
        //bpHand.ikTarget.rotation = tHandRotationTarget.rotation;

        //if (side == Enums.Side.right) {
        //    bpArm_1.target.rotation = character.body.armAimRig.arm_1_R.rotation;
        //    bpArm_2.target.rotation = character.body.armAimRig.arm_2_R.rotation;
        //    bpHand.target.rotation = character.body.armAimRig.hand_R.rotation;

        //    WeaponPoint_hip();
        //}
        //else {
        //    bpArm_1.target.rotation = character.body.armAimRig.arm_1_L.rotation;
        //    bpArm_2.target.rotation = character.body.armAimRig.arm_2_L.rotation;
        //    bpHand.target.rotation = character.body.armAimRig.hand_L.rotation;

        //    if (character.equipment.equipedType == Equipment.Type.gun)
        //        WeaponGrip();
        //}
    }

    protected virtual void Equipment_itemEquipedEvent(Equipment.Type type, Equipable item) {

        //if (side == Enums.Side.right) {

        //    // Move item to grip position and rotation
        //    item.transform.position = tGripPosition.position;
        //    item.transform.rotation = tGripPosition.rotation;

        //    // Create and attach fixed joint
        //    item.rb.velocity = Vector3.zero;
        //    handGrip = tGripPosition.gameObject.AddComponent<FixedJoint>();
        //    handGrip.connectedBody = item.rb;
        //}
        //else {
        //    if (character.equipment.equipedType == Equipment.Type.gun) {
        //        character.StartCoroutine(GrabGripCorutine(item)); 
        //    }
        //}
    }

    protected IkTargetStruct InterpolateTargetStruct(IkTargetStruct lastStruct, IkTargetStruct newStruct, float t) {
        IkTargetStruct targetStruct = new IkTargetStruct();
        targetStruct.targetPosition = Vector3.Lerp(lastStruct.targetPosition, newStruct.targetPosition, t);
        targetStruct.targetRotation = Quaternion.Slerp(lastStruct.targetRotation, newStruct.targetRotation, t);
        targetStruct.polePosition = Vector3.Lerp(lastStruct.polePosition, newStruct.polePosition, t);
        return targetStruct;
    }

    private void WeaponPoint_hip() {
        Vector3 aimOrigin = character.tCamera.position + character.tCamera.up * character.torso.aimOriginHeightOffset;
        Vector3 rightHandTarget = aimOrigin + character.tCamera.forward * character.torso.aimForwardDistance;
        character.body.hand_R.ikTarget.position = rightHandTarget;
    }

    private IEnumerator GrabGripCorutine(Equipable item) {
        yield return new WaitForSeconds(0.5f);
        bpArm_1.ragdoll.rotation = bpArm_1.target.rotation;
        bpArm_2.ragdoll.rotation = bpArm_2.target.rotation;
        bpHand.ragdoll.rotation = character.body.hand_R.target.rotation;

        handGrip = tGripPosition.gameObject.AddComponent<FixedJoint>();
        handGrip.connectedBody = item.rb;

        Debug.Log("Gripped");
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
