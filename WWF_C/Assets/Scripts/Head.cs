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
    [SerializeField] private float adsTilt;
    //private float tiltTarget;
    //private float tilt;
    //private float tiltTransitionSpeed = 9;
    //private Coroutine tiltTransitionCorutine;

    public void Initialize(CharacterLS character) {
        this.character = character;
        eyes.Initialize(character);
        bpHead = character.body.head;

        character.torso.stateChangedEvent += Torso_stateChangedEvent;
    }

    private void Torso_stateChangedEvent(Torso.State newState) {
        //if (newState == Torso.State.ads) {
        //    tiltTarget = adsTilt;
        //}
        //else {
        //    tiltTarget = 0;
        //}

        //if (Mathf.Abs(tiltTarget - tilt) > 0.1f) {

        //    if (tiltTransitionCorutine == null)
        //        tiltTransitionCorutine = character.StartCoroutine(TiltTransitionCorutine());
        //}
    }

    public void CalculateHeadTargetRotation() {
        bpHead.ikTarget.rotation = Quaternion.Euler(new Vector3(character.input.headPitchYaw.x, character.input.headPitchYaw.y, 0));   
        tTargetYaw.rotation = Quaternion.Euler(new Vector3(0, character.input.headPitchYaw.y, 0));
    }

    public void AddAdsHeadTilt() {
        float tilt = Mathf.Lerp(0, adsTilt, character.torso.adsInterpolator.t);
        //if (character.torso.state == Torso.State.hipFire)
        //    tilt = 0;
        //else
        //    tilt = 30;
        bpHead.ikTarget.Rotate(0, 0, tilt, Space.Self);
    }

    public void CalculateEyePositionAndRotation() {
        iktEyes.position = bpHead.target.position + bpHead.target.transform.TransformVector(0, 0.18f, 0.072f);
        Vector3 rightDirection = Vector3.Cross(bpHead.ikTarget.forward, Vector3.up);
        Vector3 upDirection = Vector3.Cross(rightDirection, bpHead.ikTarget.forward);
        iktEyes.rotation = Quaternion.LookRotation(bpHead.ikTarget.forward, upDirection);
    }

    //private IEnumerator TiltTransitionCorutine() {
    //    while (Mathf.Abs(tiltTarget - tilt) > 0.1f) {
    //        tilt = Mathf.Lerp(tilt, tiltTarget, Time.deltaTime * tiltTransitionSpeed);
    //        yield return new WaitForEndOfFrame();
    //    }

    //    tiltTransitionCorutine = null;
    //    yield return null;
    //}
}
