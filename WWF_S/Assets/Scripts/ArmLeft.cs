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

        if (character.equipment.equipedType == Equipment.Type.gun)
            WeaponGrip();

        bpHand.target.rotation = tHandRotationTarget.rotation;
        bpHand.ikTarget.rotation = tHandRotationTarget.rotation;

        bpArm_1.target.rotation = character.body.armAimRig.arm_1_L.rotation;
        bpArm_2.target.rotation = character.body.armAimRig.arm_2_L.rotation;
        bpHand.target.rotation = character.body.armAimRig.hand_L.rotation;
    }

    protected override void Equipment_itemEquipedEvent(Equipment.Type type, Equipable item) {
        if (character.equipment.equipedType == Equipment.Type.gun) {
            //character.StartCoroutine(GrabGripCorutine(item));
        }
    }

    protected override void Equipment_itemUnequipedEvent(Equipment.Type type, Equipable item) {
        GameObject.Destroy(handGrip);
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

    //public void UpperBodyController_stateTransitionCompleteEvent() {
    //    gun = (Gun)character.equipment.equipedItem;

    //    //bpHand.ragdoll.position = gun.transform.position + VectorUtils.FromToVector(tGripPosition.position, bpHand.ragdoll.position);
    //    //bpHand.ragdoll.rotation = gun.transform.rotation * Quaternion.Euler(0, -90, 0);

    //    b = true;
    //    //bpHand.ragdoll.position = gun.tGrip.position;// + VectorUtils.FromToVector(tGripPosition.position, bpHand.ragdoll.position);// + VectorUtils.FromToVector(tGripPosition.position, bpHand.ragdoll.position);
    //    ////bpHand.ragdoll.rotation = gun.transform.rotation * Quaternion.Euler(0, -90, 0);
    //    //handGrip = bpHand.ragdoll.gameObject.AddComponent<FixedJoint>();
    //    //handGrip.connectedBody = gun.rb;
    //}
}
