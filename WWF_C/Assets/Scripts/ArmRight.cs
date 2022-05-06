using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmRight : Arm {
    public override void Initialize(CharacterLS character) {
        bpArm_1 = character.body.arm_1_R;
        bpArm_2 = character.body.arm_2_R;
        bpHand = character.body.hand_R;

        base.Initialize(character);
    }

    protected override void Character_updateEvent() {
    }
    protected override void Character_lateUpdateEvent() {
        WeaponPoint_hip();

        bpHand.target.rotation = tHandRotationTarget.rotation;
        bpHand.ikTarget.rotation = tHandRotationTarget.rotation;

        bpArm_1.target.rotation = character.body.armAimRig.arm_1_R.rotation;
        bpArm_2.target.rotation = character.body.armAimRig.arm_2_R.rotation;
        bpHand.target.rotation = character.body.armAimRig.hand_R.rotation;
    }

    protected override void Equipment_itemEquipedEvent(Equipment.Type type, Equipable item) {

        // Move item to grip position and rotation
        item.transform.position = tGripPosition.position;
        item.transform.rotation = tGripPosition.rotation;

        // Create and attach fixed joint
        item.rb.velocity = Vector3.zero;
        handGrip = tGripPosition.gameObject.AddComponent<FixedJoint>();
        handGrip.connectedBody = item.rb;

        if (type == Equipment.Type.gun) {
            Gun gun = (Gun)item;
            tOffHandGripPosition.localPosition = gun.tGrip.localPosition;
        }
        
    }

    private void WeaponPoint_hip() {
        Vector3 aimOrigin = character.tCamera.position + character.tCamera.up * character.torso.aimOriginHeightOffset;
        Vector3 rightHandTarget = aimOrigin + character.tCamera.forward * character.torso.aimForwardDistance;
        character.body.hand_R.ikTarget.position = rightHandTarget;
    }

}
