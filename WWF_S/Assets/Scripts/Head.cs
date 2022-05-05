using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Head {
    private CharacterLS character;
    private Bodypart bpHead;
    private Bodypart bpPelvis;
    private Bodypart bpTorso1;
    [SerializeField] Transform tTargetYaw;

    [HideInInspector] public Vector3 xzForwardTarget;

    public void Initialize(CharacterLS character) {
        this.character = character;
        bpHead = character.body.head;
        bpPelvis = character.body.pelvis;
        bpTorso1 = character.body.torso_1;

        character.updateEvent += Character_updateEvent;
        //character.fixedUpdateEvent += Character_fixedUpdateEvent;
    }

    private void Character_updateEvent() {
        Quaternion targetRotation = Quaternion.Euler(new Vector3(character.input.headPitchYaw.x, character.input.headPitchYaw.y, 0));
        bpHead.ikTarget.rotation = targetRotation;
        tTargetYaw.rotation = Quaternion.Euler(new Vector3(0, character.input.headPitchYaw.y, 0));

        xzForwardTarget = new Vector3(bpHead.ikTarget.forward.x, 0, bpHead.ikTarget.forward.z).normalized;
    }
}
