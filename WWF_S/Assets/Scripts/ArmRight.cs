using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmRight : Arm {
    [SerializeField] private Transform tAimOriginBase;
    [SerializeField] private Transform tAimOrigin;
    [SerializeField] private Transform tAimOrigin2;

    public override void Initialize(CharacterLS character) {
        bpArm_1 = character.body.arm_1_R;
        bpArm_2 = character.body.arm_2_R;
        bpHand = character.body.hand_R;
        tAimRig_arm1 = character.body.armAimRig.arm_1_R;
        tAimRig_arm2 = character.body.armAimRig.arm_2_R;
        tAimRig_hand = character.body.armAimRig.hand_R;

        base.Initialize(character);

        character.fixedUpdateEvent += Character_fixedUpdateEvent;
    }

    private void Character_fixedUpdateEvent() {
        //AimAccuracyCorrection();
    }

    protected override void Character_updateEvent() {
        //AimAccuracyCorrection();
    }

    protected override void Character_lateUpdateEvent() {
        //AimAccuracyCorrection();
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
            //character.torso.armL.GrabGrip();
        }
        
        base.Equipment_itemEquipedEvent(type, item);
    }

    protected override void Equipment_itemUnequipedEvent(Equipment.Type type, Equipable item, ushort characterId) {
        GameObject.Destroy(handGrip);
       
        base.Equipment_itemUnequipedEvent(type, item, characterId);
    }

    public override void CalculateArm() {

        // Set hand target and ik target rotation
        bpHand.ikTarget.rotation = character.tCamera.rotation * Quaternion.Euler(-90, 0, -180);

        // Tilt gun when turning and strafing
        float tilt = character.rbMain.angularVelocity.y * -2; // Turn
        tilt += character.telemetry.xzVelocityLocal.x * -5; // Strafe
        bpHand.ikTarget.Rotate(new Vector3(0, tilt, 0), Space.Self);

        CalculateAimOrgin();

        // Set ik target position
        character.body.hand_R.ikTarget.position = tAimOrigin2.position + bpHand.ikTarget.TransformVector(character.torso.aimOffset);

        InterpolateAimAndIdleRotations();

        base.CalculateArm();

        //AimAccuracyCorrection();
    }

    public override void ReloadStarted(float reloadTime) {
        base.ReloadStarted(reloadTime);
    }

    protected override void InterpolateAimAndIdleRotations() {
        base.InterpolateAimAndIdleRotations();
    }

    //// Calculates position and rotation of aim origin
    //private void CalculateAimOrgin() {
    //    tAimOriginBase.position = character.body.head.ragdoll.position;

    //    float targetHeadTilt;
    //    if (character.torso.state == Torso.State.hipFire)
    //        targetHeadTilt = 0;
    //    else
    //        targetHeadTilt = torso.head.adsTilt;

    //    tAimOriginBase.rotation = torso.head.iktEyes.rotation;
    //    tAimOriginBase.Rotate(0, 0, targetHeadTilt, Space.Self);

    //    tAimOrigin.rotation = torso.head.iktEyes.rotation;
    //}

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

        tAimOrigin.rotation = torso.head.iktEyes.rotation;

        //Gun gun = (Gun)character.equipment.equipedItem;
        //Vector3 pBack = Vector3.ProjectOnPlane(gun.tSight_back.position, torso.head.iktEyes.forward);
        //Vector3 pFront = Vector3.ProjectOnPlane(gun.tSight_front.position, torso.head.iktEyes.forward);

        //Vector3 localOffset = torso.head.iktEyes.InverseTransformPoint(pBack);
        //tAimOrigin.position = pBack
    }

    private Quaternion accuracyCorrectionRotation;
    [SerializeField] private Vector3 accuracyCorrectionPosition;
    private void AimAccuracyCorrection() {
        if (character.torso.state != Torso.State.ads) {
            tAimOrigin2.localPosition = Vector3.zero;
            return;
        }

        Gun gun = (Gun)character.equipment.equipedItem;
        Vector3 camToSight = (gun.tSight_back.position - character.tCamera.position).normalized;
        Debug.DrawLine(character.tCamera.position, gun.tSight_back.position + camToSight * 12, Color.cyan);
        float yawError = Vector3.SignedAngle(character.tCamera.forward, camToSight, tAimOrigin.up);
        float pitchError = Vector3.SignedAngle(character.tCamera.forward, camToSight, tAimOrigin.right);
        tAimOrigin2.localPosition += new Vector3(-yawError * Time.deltaTime * 0.02f, pitchError * 0.02f * Time.deltaTime, 0) * character.torso.adsInterpolator.t / (1 + character.rbMain.angularVelocity.y);

        tAimOrigin2.localPosition = Vector3.Lerp(tAimOrigin2.localPosition, Vector3.zero, character.rbMain.angularVelocity.y * Time.deltaTime);
    }
    //private void AimAccuracyCorrection() {
    //    Gun gun = (Gun)character.equipment.equipedItem;
    //    Vector3 pBack = Vector3.ProjectOnPlane(gun.tSight_back.position, torso.head.iktEyes.forward);
    //    Vector3 pFront = Vector3.ProjectOnPlane(gun.tSight_front.position, torso.head.iktEyes.forward);

    //    Vector3 localOffset = torso.head.iktEyes.InverseTransformPoint(pBack);
    //    bpHand.ikTarget.position += torso.head.iktEyes.TransformVector(localOffset);
    //}
    //private void AimAccuracyCorrection() {
    //    Gun gun = (Gun)character.equipment.equipedItem;
    //    Vector3 pBack = Vector3.ProjectOnPlane(gun.tSight_back.position, torso.head.iktEyes.forward);
    //    Vector3 pFront = Vector3.ProjectOnPlane(gun.tSight_front.position, torso.head.iktEyes.forward);

    //    Vector3 error = VectorUtils.FromToVector(pBack, pFront);
    //    accuracyCorrectionPosition += error;

    //    bpHand.ikTarget.position -= bpHand.ikTarget.TransformVector(accuracyCorrectionPosition);
    //}
}
