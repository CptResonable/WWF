using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterN : Character {
    protected new PlayerN player;
    public BodyN bodyN;
    new public EquipmentN equipment;

    protected override void Awake() {
        equipment.Initialize(this);
        base.Awake();
        base.equipment = equipment;
        player = (PlayerN)base.player;
        bodyN.Initialize(this);
    }
    public override ushort GetClientID() {
        Debug.Log(player);
        return player.playerData.clientId;
    }
}
