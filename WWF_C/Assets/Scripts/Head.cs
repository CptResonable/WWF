using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Head {
    public Eyes eyes;
    private CharacterLS character;
    private Bodypart bpHead;
    public Transform iktEyes;
    [SerializeField] private Transform tTargetYaw;
    [SerializeField] public float adsTilt;

    public void Initialize(CharacterLS character) {
        this.character = character;
        eyes.Initialize(character);
        bpHead = character.body.head;
    }

    //public void CalculateHeadTargetRotation() {
    //    if (character.GetPlayer().playerType != Player.PlayerType.local)
    //        return;

    //    pitchYaw += new Vector2(-character.input.mouseDelta.y, character.input.mouseDelta.x);
    //    bpHead.ikTarget.rotation = Quaternion.Euler(new Vector3(pitchYaw.x, pitchYaw.y, 0));
    //    tTargetYaw.rotation = Quaternion.Euler(new Vector3(0, pitchYaw.y, 0));
    //}

    //public void CalculateHeadTargetRotationServer(Vector2 mouseDelta) {
    //    if (character.GetPlayer().playerType != Player.PlayerType.server)
    //        return;

    //    pitchYaw += new Vector2(-mouseDelta.y, mouseDelta.x);
    //    bpHead.ikTarget.rotation = Quaternion.Euler(new Vector3(pitchYaw.x, pitchYaw.y, 0));
    //    tTargetYaw.rotation = Quaternion.Euler(new Vector3(0, pitchYaw.y, 0));


    //    AddAdsHeadTilt();
    //    CalculateEyePositionAndRotation();
    //}

    public void CalculateHeadTargetRotation() {
        bpHead.ikTarget.rotation = Quaternion.Euler(new Vector3(character.input.headPitchYaw.x, character.input.headPitchYaw.y, 0));   
        tTargetYaw.rotation = Quaternion.Euler(new Vector3(0, character.input.headPitchYaw.y, 0));
    }

    public void AddAdsHeadTilt() {
        float tilt = Mathf.Lerp(0, adsTilt, character.torso.adsInterpolator.t);
        bpHead.ikTarget.Rotate(0, 0, tilt, Space.Self);
    }

    public void CalculateEyePositionAndRotation() {
        iktEyes.position = bpHead.ragdoll.position + bpHead.ragdoll.transform.TransformVector(0, 0.18f, 0.072f);
        Vector3 rightDirection = Vector3.Cross(bpHead.ikTarget.forward, Vector3.up);
        Vector3 upDirection = Vector3.Cross(rightDirection, bpHead.ikTarget.forward);
        iktEyes.rotation = Quaternion.LookRotation(bpHead.ikTarget.forward, upDirection);
    }
}
