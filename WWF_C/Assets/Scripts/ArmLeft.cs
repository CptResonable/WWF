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
        tAimRig_arm1 = character.body.armAimRig.arm_1_L;
        tAimRig_arm2 = character.body.armAimRig.arm_2_L;
        tAimRig_hand = character.body.armAimRig.hand_L;

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
        base.Equipment_itemEquipedEvent(type, item);
    }

    protected override void Equipment_itemUnequipedEvent(Equipment.Type type, Equipable item, ushort characterId) {
        LetGoGrip();
        base.Equipment_itemUnequipedEvent(type, item, characterId);
    }

    protected override void Locomotion_sprintChangedEvent(bool isSprinting) {
        if (isSprinting) {
            LetGoGrip();
        }

        base.Locomotion_sprintChangedEvent(isSprinting);
    }

    protected override void HandTargetRunAimTransitionComplete() {
        if (!character.locomotion.isSprinting && character.equipment.equipedType == Equipment.Type.gun) {
            GrabGrip();
        }
    }

    public override void UpdateState_idle() {

        // Set hand target and ik target rotation
        bpHand.ikTarget.rotation = character.body.hand_R.ikTarget.rotation;

        // Set ik target position
        if (character.equipment.equipedType == Equipment.Type.gun)
            bpHand.ikTarget.position = tOffHandGripPosition.position;

        InterpolateAimAndIdleRotations();

        base.UpdateState_idle();
    }
    public override void UpdateState_aim() {

        // Set hand target and ik target rotation
        bpHand.ikTarget.rotation = character.body.hand_R.ikTarget.rotation;

        // Set ik target position
        if (character.equipment.equipedType == Equipment.Type.gun)
            bpHand.ikTarget.position = tOffHandGripPosition.position;

        InterpolateAimAndIdleRotations();

        base.UpdateState_aim();
    }
    public override void UpdateState_reload() {

        // Set hand target and ik target rotation
        bpHand.ikTarget.rotation = character.body.hand_R.ikTarget.rotation;

        Gun gun = (Gun)character.equipment.equipedItem;
        // Set ik target position

        float t = 1 - Mathf.Abs((gun.reloadProgress - 0.5f) * 2);
        t = InterpolationUtils.LinearToSmoothStep(t);
        bpHand.ikTarget.position = Vector3.Lerp(tOffHandGripPosition.position, character.equipment.tAmmoPouch.position, t);

        InterpolateAimAndIdleRotations();

        base.UpdateState_reload();
    }

    protected override void InterpolateAimAndIdleRotations() {
        base.InterpolateAimAndIdleRotations();
    }

    protected override void Gun_reloadStartedEvent(float reloadTime) {
        base.Gun_reloadStartedEvent(reloadTime);

        LetGoGrip();
    }

    protected override void Gun_reloadFinishedEvent() {
        base.Gun_reloadFinishedEvent();
    }

    public void GrabGrip() {
        Debug.Log("GRAB GRIP!");
        if (handGrip != null)
            LetGoGrip();

        Gun gun = (Gun)character.equipment.equipedItem;

        bpHand.ragdoll.position = gun.tGrip.position;
        bpHand.ragdoll.rotation = gun.tGrip.rotation;
        handGrip = bpHand.ragdoll.gameObject.AddComponent<FixedJoint>();
        handGrip.connectedBody = gun.tGrip.GetComponent<Rigidbody>();
    }

    public void LetGoGrip() {
        GameObject.Destroy(handGrip);
    }
}
