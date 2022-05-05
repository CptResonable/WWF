using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerN : Player {
    public new CharacterN character;

    public override void Initialize(DrDatas.Player.PlayerData playerData) {
        base.Initialize(playerData);
    }

    public override void SpawnCharacter(DrDatas.Player.CharacterData characterData) {
        GameObject goNewCharacter = Instantiate(GameObjects.i.characterN, transform);
        character = goNewCharacter.GetComponent<CharacterN>();
        base.goCharacter = goNewCharacter;
        base.character = character;

        base.SpawnCharacter(characterData);
    }
}
