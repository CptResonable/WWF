using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmRight : Arm {
    [SerializeField] private Transform tAimOriginBase;
    [SerializeField] private Transform tAimOrigin;

    public override void Initialize(CharacterLS character) {
        bpArm_1 = character.body.arm_1_R;
        bpArm_2 = character.body.arm_2_R;
        bpHand = character.body.hand_R;

        base.Initialize(character);
    }

    protected override void Character_updateEvent() {
    }

    protected override void Character_lateUpdateEvent() {
    }

    protected override void Equipment_itemEquipedEvent(Equipment.Type type, Equipable item) {

        // Move item to grip position and rotation
        item.transform.position = tGripPosition.position;
        item.transform.rotation = tGripPosition.rotation;

        // Create and attach fixed joint
        item.rb.velocity = Vector3.zero;
        handGrip = bpHand.ragdoll.gameObject.AddComponent<FixedJoint>();
        handGrip.connectedBody = item.rb;

        if (type == Equipment.Type.gun) {
            Gun gun = (Gun)item;
            tOffHandGripPosition.localPosition = gun.tGrip.localPosition;
            character.torso.armL.UpperBodyController_stateTransitionCompleteEvent();
        }   
    }

    protected override void Equipment_itemUnequipedEvent(Equipment.Type type, Equipable item, ushort characterId) {
        GameObject.Destroy(handGrip);
    }

    public override void CalculateArm() {

        // Set hand target and ik target rotation
        bpHand.ikTarget.rotation = character.tCamera.rotation * Quaternion.Euler(-90, 0, -180);
        bpHand.ikTarget.Rotate(new Vector3(0, character.telemetry.xzVelocityLocal.x * -5, 0), Space.Self);
        bpHand.target.rotation = bpHand.ikTarget.rotation;

        // Copy rotation from aim rig to target rig
        bpArm_1.target.rotation = character.body.armAimRig.arm_1_R.rotation;
        bpArm_2.target.rotation = character.body.armAimRig.arm_2_R.rotation;

        CalculateAimOrgin();

        // Set ik target position
        character.body.hand_R.ikTarget.position = tAimOrigin.position + bpHand.ikTarget.TransformVector(character.torso.aimOffset);
        //character.body.hand_R.ikTarget.position = tAimOrigin.position + character.tCamera.TransformVector(character.torso.aimOffset);
    }

    // Calculates position and rotation of aim origin
    private void CalculateAimOrgin() {
        tAimOriginBase.position = character.body.head.ragdoll.position;

        float targetHeadTilt;
        if (character.torso.state == Torso.State.hipFire)
            targetHeadTilt = 0;
        else
            targetHeadTilt = torso.head.adsTilt;

        tAimOriginBase.rotation = torso.head.iktEyes.rotation;
        tAimOriginBase.Rotate(0, 0, targetHeadTilt, Space.Self);
    }

    //private void WeaponPoint_hip() {
    //    character.body.hand_R.ikTarget.position = character.tCamera.position + character.tCamera.TransformVector(character.torso.aimOffset);
    //}


    //private void WeaponPoint_hip() {
    //    Vector3 aimOrigin = character.tCamera.position + character.tCamera.up * character.torso.aimOriginHeightOffset;
    //    Vector3 rightHandTarget = aimOrigin + character.tCamera.forward * character.torso.aimForwardDistance;
    //    character.body.hand_R.ikTarget.position = rightHandTarget;
    //}

}
