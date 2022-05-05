using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;

public class PlayerL : Player {
    public new CharacterL character;

    public override void Initialize(DrDatas.Player.PlayerData playerData) {
        base.Initialize(playerData);

        UiManager.i.OpenWindow(UiWindow.ID.spawnSelection); // Open spawn selection
        UiWindow_spawnSelect.OnClickEvent_btnSpawn += UiWindow_spawnSelect_OnClickEvent_btnSpawn;
    }

    private void UiWindow_spawnSelect_OnClickEvent_btnSpawn(int spawnpointId) {
        ClientManagerL.i.playerManager.SendMsg_requestSpawn((ushort)spawnpointId);
    }

    public override void SpawnCharacter(DrDatas.Player.CharacterData characterData) {
        GameObject goNewCharacter = Instantiate(GameObjects.i.characterL, transform);
        character = goNewCharacter.GetComponent<CharacterL>();
        base.goCharacter = goNewCharacter;
        base.character = character;

        base.SpawnCharacter(characterData);
    }
}
