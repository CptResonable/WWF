using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Eyes {
    public Camera eyeCamera;

    private CharacterLS character;
    private float targetFOV;
    private float FOV;
    private float fovTransitionSpeed = 16;
    private Coroutine fovTransitionCorutine;

    public void Initialize(CharacterLS character) {
        this.character = character;
        FOV = Settings.FOV;

        character.torso.stateChangedEvent += Torso_stateChangedEvent;
    }

    private void Torso_stateChangedEvent(Torso.State newState) {
        if (newState == Torso.State.ads) {
            targetFOV = Settings.FOV * 0.75f;
        }
        else {
            targetFOV = Settings.FOV;
        }

        if (Mathf.Abs(targetFOV - FOV) > 0.1f) {

            if (fovTransitionCorutine == null)
                fovTransitionCorutine = character.StartCoroutine(FovTransitionCorutine());
        }
    }

    private IEnumerator FovTransitionCorutine() {
        while (Mathf.Abs(targetFOV - FOV) > 0.1f) {
            FOV = Mathf.Lerp(FOV, targetFOV, Time.deltaTime * fovTransitionSpeed);
            eyeCamera.fieldOfView = FOV;
            yield return new WaitForEndOfFrame();
        }

        fovTransitionCorutine = null;
        yield return null;
    }
}
