using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

[System.Serializable]
public class ArmLeft : Arm {
    public override void Initialize(CharacterLS character) {
        bpArm_1 = character.body.arm_1_L;
        bpArm_2 = character.body.arm_2_L;
        bpHand = character.body.hand_L;

        base.Initialize(character);
    }

    protected override void Character_updateEvent() {
    }
    protected override void Character_lateUpdateEvent() {
    }

    protected override void Equipment_itemEquipedEvent(Equipment.Type type, Equipable item) {
        if (character.equipment.equipedType == Equipment.Type.gun) {
            //character.StartCoroutine(GrabGripCorutine(item));
        }
    }

    protected override void Equipment_itemUnequipedEvent(Equipment.Type type, Equipable item) {
        GameObject.Destroy(handGrip);
    }

    public override void CalculateHandPosRot() {

        if (character.equipment.equipedType == Equipment.Type.gun)
            WeaponGrip();

        bpHand.target.rotation = tHandRotationTarget.rotation;
        bpHand.ikTarget.rotation = tHandRotationTarget.rotation;

        bpArm_1.target.rotation = character.body.armAimRig.arm_1_L.rotation;
        bpArm_2.target.rotation = character.body.armAimRig.arm_2_L.rotation;
        bpHand.target.rotation = character.body.armAimRig.hand_L.rotation;
    }

    private void WeaponGrip() {
        character.body.hand_L.ikTarget.position = tOffHandGripPosition.position;
    }


    public void UpperBodyController_stateTransitionCompleteEvent() {
        Gun gun = (Gun)character.equipment.equipedItem;

        bpHand.ragdoll.position = gun.tGrip.position;
        bpHand.ragdoll.rotation = gun.tGrip.rotation;
        handGrip = bpHand.ragdoll.gameObject.AddComponent<FixedJoint>();
        handGrip.connectedBody = gun.tGrip.GetComponent<Rigidbody>();
    }
}
