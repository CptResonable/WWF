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
    private float tiltTarget;
    private float tilt;
    private float tiltTransitionSpeed = 9;
    private Coroutine tiltTransitionCorutine;

    public void Initialize(CharacterLS character) {
        this.character = character;
        bpHead = character.body.head;
        bpPelvis = character.body.pelvis;
        bpTorso1 = character.body.torso_1;

        character.torso.stateChangedEvent += Torso_stateChangedEvent;
    }

    private void Torso_stateChangedEvent(Torso.State newState) {
        if (newState == Torso.State.ads) {
            tiltTarget = adsTilt;
        }
        else {
            tiltTarget = 0;
        }

        Debug.Log("TargetSet! " + tiltTarget);

        if (Mathf.Abs(tiltTarget - tilt) > 0.1f) {
            Debug.Log("hello?");

            if (tiltTransitionCorutine == null)
                tiltTransitionCorutine = character.StartCoroutine(TiltTransitionCorutine());
        }
    }

    public void CalculateHeadTargetRotation() {
        bpHead.ikTarget.rotation = Quaternion.Euler(new Vector3(character.input.headPitchYaw.x, character.input.headPitchYaw.y, 0));
        tTargetYaw.rotation = Quaternion.Euler(new Vector3(0, character.input.headPitchYaw.y, 0));
    }

    public void AddAdsHeadTilt() {
        bpHead.ikTarget.Rotate(0, 0, tilt, Space.Self);
    }

    private IEnumerator TiltTransitionCorutine() {
        while (Mathf.Abs(tiltTarget - tilt) > 0.1f) {
            tilt = Mathf.Lerp(tilt, tiltTarget, Time.deltaTime * tiltTransitionSpeed);
            yield return new WaitForEndOfFrame();
        }
        tiltTransitionCorutine = null;
        yield return null;
    }
}
