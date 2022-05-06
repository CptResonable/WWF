using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    protected override void Equipment_itemEquipedEvent(Equipment.Type type, Equipable item) {
        if (character.equipment.equipedType == Equipment.Type.gun) {
            character.StartCoroutine(GrabGripCorutine(item));
        }
    }

    private void WeaponGrip() {
        Gun gun = character.equipment.equipedItem as Gun;
        character.body.hand_L.ikTarget.position = gun.tGrip.position;
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
}
