using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputL : PlayerInput {
    protected new CharacterL character;
    public PlayerControls playerControls;

    public PlayerInputL(CharacterL character) : base(character) {
        this.character = (CharacterL)character;
        base.character = this.character;
            
        playerControls = new PlayerControls();
        playerControls.Enable();

        toggleAds = new Action(playerControls.Land.ToggleAds);
        attack_1 = new Action(playerControls.Land.Attack_1);
        equip_s1 = new Action(playerControls.Land.Equip_S1);
        equip_s2 = new Action(playerControls.Land.Equip_S2);
        equip_s3 = new Action(playerControls.Land.Equip_S3);
        equip_s4 = new Action(playerControls.Land.Equip_S4);
        jump = new Action(playerControls.Land.Jump);
        crouch = new Action(playerControls.Land.Crouch);
        reload = new Action(playerControls.Land.Reload);
        sprint = new Action(playerControls.Land.Sprint);
        actions = new Action[9] { toggleAds, attack_1, equip_s1, equip_s2, equip_s3, equip_s4, jump, reload, sprint };

        character.updateEvent += Character_updateEvent;
        character.fixedUpdateEvent += Character_fixedUpdateEvent;
    }

    private void Character_updateEvent() {
        vecMoveXZ = playerControls.Land.Move.ReadValue<Vector2>();
        v3vecMoveXZ = new Vector3(vecMoveXZ.x, 0, vecMoveXZ.y);
        Vector2 mouseDelta = playerControls.Land.MouseDelta.ReadValue<Vector2>() * Settings.mouseSensitivity;
        headPitchYaw += new Vector2(-mouseDelta.y, mouseDelta.x);
    }

    private void Character_fixedUpdateEvent() {

        // Send input data to server
        ClientManagerL.i.playerManager.SendMsg_updateInput(new DrDatas.Player.PlayerInputData(this));
    }

    public void UpdateEyePitchYaw(float pitch, float yaw) {
        headPitchYaw = new Vector2(pitch, yaw);
    }
}
