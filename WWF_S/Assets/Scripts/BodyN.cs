using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BodyN {
    public Transform[] bodyparts;

    public Transform armature;
    public Transform pelvis;
    public Transform leg_1_L;
    public Transform leg_2_L;
    public Transform foot_L;
    public Transform leg_1_R;
    public Transform leg_2_R;
    public Transform foot_R;
    public Transform torso_1;
    public Transform torso_2;
    public Transform arm_1_L;
    public Transform arm_2_L;
    public Transform hand_L;
    public Transform arm_1_R;
    public Transform arm_2_R;
    public Transform hand_R;
    public Transform head;

    private CharacterN character;

    public void Initialize(CharacterN character) {
        this.character = character;

        bodyparts = new Transform[16] {
            pelvis, leg_1_L, leg_2_L, foot_L, leg_1_R, leg_2_R, foot_R,
            torso_1, torso_2, arm_1_L, arm_2_L, hand_L, arm_1_R, arm_2_R, hand_R, head
        };
    }

    public void CopyRigFromData(DrDatas.Player.PlayerBodyData bodyData) {
        for (int i = 0; i < bodyparts.Length; i++) {
            armature.position = bodyData.rootPosition;
            bodyparts[i].rotation = bodyData.rotations[i];
        }
    }
}
