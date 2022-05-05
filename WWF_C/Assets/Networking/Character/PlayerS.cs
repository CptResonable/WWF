using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Server;

public class PlayerS : Player {
    public IClient client;
    public new CharacterS character;

    public override void Initialize(DrDatas.Player.PlayerData playerData) {
        base.Initialize(playerData);
        client = ServerManagerS.i.clientConnections[playerData.clientId].client;
    }

    public override void SpawnCharacter(DrDatas.Player.CharacterData characterData) {
        GameObject goNewCharacter = Instantiate(GameObjects.i.characterS, transform);
        character = goNewCharacter.GetComponent<CharacterS>();
        base.goCharacter = goNewCharacter;
        base.character = character;

        base.SpawnCharacter(characterData);
    }
}
