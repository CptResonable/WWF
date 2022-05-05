using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterS : CharacterLS {
    // public new EyesS eyes;
    public new PlayerInputS input;
    private PlayerS player;

    protected override void Awake() {
        player = transform.parent.GetComponent<PlayerS>();

        input = new PlayerInputS(this);
        base.input = input;
        // input = new PlayerInputS(this);
        // eyes.Initialize(this);
        // base.eyes = eyes;

        base.Awake();

        bodyData = new DrDatas.Player.PlayerBodyData(player.client.ID, body);
    }
    public override ushort GetClientID() {
        return player.client.ID;
    }
}
