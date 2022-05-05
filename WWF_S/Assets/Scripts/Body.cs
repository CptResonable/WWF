using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Body {
    //public float inertiaTensorMod = 0;

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

    //private Character character;

    // public void Initialize(Character character) {
    public void Initialize() {
        //this.character = character;

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
        //CharacterColliderSetup.SetupIgnores(this);
    }

    // public void CopyRigFromData(DrDatas.Player.PlayerBodyData bodyData) {
    //     this.bodyData = bodyData;

    //     for (int i = 0; i < bodyparts.Length; i++) {
    //         armature.ragdoll.position = bodyData.rootPosition;
    //         bodyparts[i].ragdoll.rotation = bodyData.rotations[i];
    //     }
    // }

    // public void SetAllStrengthMods(float value) {
    //     //value = Mathf.Clamp01(value);

    //     for (int i = 0; i < bodyparts.Length; i++) {
    //         bodyparts[i].SetStrengthMod(value);
    //     }
    // }
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

    public void SetStrengthMod(float value) {
        if (rotCopy == null)
            return;

        strengthMod = value;
        rotCopy.strengthMod = value;
    }
}