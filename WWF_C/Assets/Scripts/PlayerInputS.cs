using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputS : PlayerInput {
    protected new CharacterS character;

    public PlayerInputS(Character character) : base(character) {
        this.character = (CharacterS)character;
        base.character = this.character;

        toggleAds = new Action();
        attack_1 = new Action();
        equip_s1 = new Action();
        jump = new Action();
        reload = new Action();
        actions = new Action[5] { toggleAds, attack_1, equip_s1, jump, reload };
    }

    public void OnMsg_updateInput(DrDatas.Player.PlayerInputData inputData) {
        vecMoveXZ = inputData.vecMoveXZ;
        headPitchYaw = inputData.headPitchYaw;

        for (int i = 0; i < actions.Length; i++) {
            actions[i].Set(inputData.actionStates[i]);
        }

        //character.eyes.SetPitchYaw(headPitchYaw);
    }
}