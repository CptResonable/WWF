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

        //if (b) {
        //    Joint.
        //    bpHand.ragdoll.position = gun.tGrip.position;// + VectorUtils.FromToVector(tGripPosition.position, bpHand.ragdoll.position);// + VectorUtils.FromToVector(tGripPosition.position, bpHand.ragdoll.position);
        //                                                 //bpHand.ragdoll.rotation = gun.transform.rotation * Quaternion.Euler(0, -90, 0);
        //    handGrip = bpHand.ragdoll.gameObject.AddComponent<FixedJoint>();
        //    PhysicsJ
        //    Physics.p PhysicsJoint.CreateFixed(jointFrameP, jointFrameC);
        //    Debug.Log("BBBBBBBBBBBBBB");
        //    handGrip.connectedBody = gun.rb;
        //    b = false;
        //    //handGrip.anchor = Vector3.zero;
        //    //handGrip.connectedAnchor = Vector3.zero;
        //}
        ////    handGrip.connectedAnchor = Vector3.zero;
    }

    protected override void Equipment_itemEquipedEvent(Equipment.Type type, Equipable item) {
        if (character.equipment.equipedType == Equipment.Type.gun) {
            //character.StartCoroutine(GrabGripCorutine(item));
        }
    }

    private void WeaponGrip() {
        character.body.hand_L.ikTarget.position = tOffHandGripPosition.position;
        //Gun gun = character.equipment.equipedItem as Gun;
        //character.body.hand_L.ikTarget.position = gun.tGrip.position;
    }

    private IEnumerator GrabGripCorutine(Equipable item) {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForFixedUpdate();
        UpperBodyController_stateTransitionCompleteEvent();

        ////bpArm_1.ragdoll.rotation = bpArm_1.target.rotation;
        ////bpArm_2.ragdoll.rotation = bpArm_2.target.rotation;
        ////bpHand.ragdoll.rotation = character.body.hand_R.target.rotation;

        //bpArm_1.ragdoll.rotation = character.body.armAimRig.arm_1_L.rotation;
        //bpArm_2.ragdoll.rotation = character.body.armAimRig.arm_2_L.rotation;
        //bpHand.ragdoll.rotation = character.body.armAimRig.hand_L.rotation;
        //Gun gun = (Gun)item;

        ////handGrip = tGripPosition.gameObject.AddComponent<FixedJoint>();
        //bpHand.ragdoll.position = gun.tGrip.position;
        //bpHand.rb.velocity = Vector3.zero;
        //bpHand.rb.angularVelocity = Vector3.zero;
        //gun.rb.velocity = Vector3.zero;
        //gun.rb.angularVelocity = Vector3.zero;
        //bpHand.ragdoll.position = gun.tGrip.position;
        //Time.timeScale = 0;
        ////handGrip = bpHand.ragdoll.gameObject.AddComponent<FixedJoint>();
        //handGrip = tGripPosition.gameObject.AddComponent<FixedJoint>();
        //handGrip.autoConfigureConnectedAnchor = true;
        //handGrip.connectedBody = item.rb;

        //Debug.Log("Left con anch: " + handGrip.connectedAnchor * 1000);
        //Debug.Log("Left anch: " + handGrip.anchor * 1000);
        ////handGrip.anchor = Vector3.zero;
        ////handGrip.connectedAnchor = Vector3.zero;

        //Debug.Log("Gripped");
    }

    //private void Connect() {
    //    var commonPivotPointWorld = math.lerp(pointConPWorld, pointPonCWorld, 0.5f);

    //    var pivotP = math.transform(math.inverse(bodyPTransform), commonPivotPointWorld);
    //    var pivotC = math.transform(math.inverse(bodyCTransform), commonPivotPointWorld);

    //    var jointFrameP = new BodyFrame(new RigidTransform(bodyPTransform.rot, pivotP));
    //    var jointFrameC = new BodyFrame(new RigidTransform(bodyCTransform.rot, pivotC));
    //    jointData = PhysicsJoint.CreateFixed(jointFrameP, jointFrameC);
    //}

    bool b;
    Gun gun;


    public void UpperBodyController_stateTransitionCompleteEvent() {
        gun = (Gun)character.equipment.equipedItem;

        bpHand.ragdoll.position = gun.tGrip.position;// + VectorUtils.FromToVector(tGripPosition.position, bpHand.ragdoll.position);// + VectorUtils.FromToVector(tGripPosition.position, bpHand.ragdoll.position);
        bpHand.ragdoll.rotation = gun.tGrip.rotation;
        ////bpHand.ragdoll.rotation = gun.transform.rotation * Quaternion.Euler(0, -90, 0);
        handGrip = tGripPosition.gameObject.AddComponent<FixedJoint>();
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
