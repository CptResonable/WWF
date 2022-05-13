using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Head {
    private CharacterLS character;
    private Bodypart bpHead;
    private Bodypart bpPelvis;
    private Bodypart bpTorso1;
    public Transform iktEyes;
    [SerializeField] private Transform tTargetYaw;

    //[HideInInspector] public Vector3 xzForwardTarget;
    [SerializeField] private float adsTilt;

    public void Initialize(CharacterLS character) {
        this.character = character;
        bpHead = character.body.head;
        bpPelvis = character.body.pelvis;
        bpTorso1 = character.body.torso_1;

        //character.updateEvent += Character_updateEvent;
        //character.fixedUpdateEvent += Character_fixedUpdateEvent;
    }
    public void CalculateHeadTargetRotation() {
        bpHead.ikTarget.rotation = Quaternion.Euler(new Vector3(character.input.headPitchYaw.x, character.input.headPitchYaw.y, 0));
        tTargetYaw.rotation = Quaternion.Euler(new Vector3(0, character.input.headPitchYaw.y, 0));
    }

    public void AddAdsHeadTilt() {
        bpHead.ikTarget.Rotate(0, 0, adsTilt, Space.Self);
    }

    //private void Character_updateEvent() {
    //    bpHead.ikTarget.rotation = Quaternion.Euler(new Vector3(character.input.headPitchYaw.x, character.input.headPitchYaw.y, 0));
    //    tTargetYaw.rotation = Quaternion.Euler(new Vector3(0, character.input.headPitchYaw.y, 0));
    //}
}
