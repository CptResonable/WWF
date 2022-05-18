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
        equip_s2 = new Action();
        equip_s3 = new Action();
        equip_s4 = new Action();
        jump = new Action();
        reload = new Action();
        sprint = new Action();
        actions = new Action[9] { toggleAds, attack_1, equip_s1, equip_s2, equip_s3, equip_s4, jump, reload, sprint };
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
