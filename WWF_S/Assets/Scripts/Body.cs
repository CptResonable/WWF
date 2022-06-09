using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Body {
    //public float inertiaTensorMod = 0;

    public Rigidbody rbArmature;

    public Bodypart[] bodyparts;
    public Rigidbody[] rigidbodies;
    //public DrDatas.Player.PlayerBodyData bodyData;

    public Bodypart pelvis;
    public Bodypart leg_1_L;
    public Bodypart leg_2_L;
    public Bodypart foot_L;
    public Bodypart leg_1_R;
    public Bodypart leg_2_R;
    public Bodypart foot_R;
    public Bodypart torso_1;
    public Bodypart torso_2;
    public Bodypart arm_1_L;
    public Bodypart arm_2_L;
    public Bodypart hand_L;
    public Bodypart arm_1_R;
    public Bodypart arm_2_R;
    public Bodypart hand_R;
    public Bodypart head;

    public ArmAimRig armAimRig;

    private CharacterLS character;

    // public void Initialize(Character character) {
    public void Initialize(CharacterLS character) {
        this.character = character;

        bodyparts = new Bodypart[16] {
            pelvis, leg_1_L, leg_2_L, foot_L, leg_1_R, leg_2_R, foot_R,
            torso_1, torso_2, arm_1_L, arm_2_L, hand_L, arm_1_R, arm_2_R, hand_R, head
        };

        List<Rigidbody> rbList = new List<Rigidbody>();

        for (int i = 0; i < bodyparts.Length; i++) {
            bodyparts[i].Initialize(this);

            if (bodyparts[i].rb != null)
                rbList.Add(bodyparts[i].rb);
        }

        rigidbodies = rbList.ToArray();

        pelvis.Deragdollify();
        leg_1_L.Deragdollify();
        leg_2_L.Deragdollify();
        leg_1_R.Deragdollify();
        leg_2_R.Deragdollify();

        character.health.stateChangedEvent += Health_stateChangedEvent;
    }

    private void Health_stateChangedEvent(Character character, Health.State state) {
        if (state == Health.State.knocked) {
            foreach (Bodypart bodypart in bodyparts) {
                bodypart.SetStrengthMod(0);
            }

            RagdollifyLegs();
        }
    }
    
    private void RagdollifyLegs() {
        pelvis.Ragdollify(rbArmature);
        leg_1_L.Ragdollify(pelvis.rb);
        leg_2_L.Ragdollify(leg_1_L.rb);
        leg_1_R.Ragdollify(pelvis.rb);
        leg_2_R.Ragdollify(leg_1_R.rb);

        pelvis.ragdoll.GetComponent<CopyRotation>().enabled = false;
        leg_1_L.ragdoll.GetComponent<LerpRotation>().enabled = false;
        leg_2_L.ragdoll.GetComponent<LerpRotation>().enabled = false;
        leg_1_R.ragdoll.GetComponent<LerpRotation>().enabled = false;
        leg_2_R.ragdoll.GetComponent<LerpRotation>().enabled = false;
    }
}

[Serializable]
public class Bodypart {
    public Transform target;
    public Transform ragdoll;
    public Transform ikTarget;
    public Transform ikPole;

    [SerializeField] private float inertiaTensorMod = 1;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public ConfigurableJoint joint;

    //private Vector3 pid;
    private PhysicallyCopyRotation rotCopy;
    private float strengthMod = 1;

    public void Initialize(Body body) {
        if (ragdoll != null) {
            rb = ragdoll.GetComponent<Rigidbody>();
            joint = ragdoll.GetComponent<ConfigurableJoint>();
            ragdoll.TryGetComponent<PhysicallyCopyRotation>(out rotCopy);

            //if (rb != null)
            //    rb.inertiaTensor = Vector3.Lerp(rb.inertiaTensor, Vector3.one, body.inertiaTensorMod);
            if (rb != null && rb.inertiaTensor.x < 1)
                rb.inertiaTensor *= inertiaTensorMod;

            
            //rotCopy = physical.GetComponent<PhysicallyCopyRotation>();

            //if (joint != null) {
            //    strength = joint.
            //}

            //if (rotCopy != null) {
            //    rotCopy.strengthMod = 1;
            //    pid = rotCopy.p
            //}
        }
    }
    public void Deragdollify() {
        joint.connectedBody = null;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;
        ragdoll.GetComponent<Collider>().enabled = false;
    }

    public void Ragdollify(Rigidbody connectedBody) {
        joint.connectedBody = connectedBody;
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        ragdoll.GetComponent<Collider>().enabled = true;
    }

    public void SetStrengthMod(float value) {
        if (joint != null)
            SetJointStrength(value);

        if (rotCopy == null)
            return;

        strengthMod = value;
        rotCopy.strengthMod = value;
    }

    private void SetJointStrength(float value) {
        JointDrive drive = new JointDrive();
        Debug.Log("SET JOINT STRNGTH!");
        drive.positionSpring = 0;

        if (joint.rotationDriveMode == RotationDriveMode.XYAndZ) {
            joint.angularXDrive = drive;
            joint.angularYZDrive = drive;
        }
        else {
            joint.slerpDrive = drive;
        }      
    }
}

[System.Serializable]
public class ArmAimRig {
    public Transform arm_1_L, arm_2_L, hand_L, arm_1_R, arm_2_R, hand_R;
}

public struct ConfigurableJointValues {

}

