using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class CharacterL : CharacterLS {
    public new PlayerInput input;
    protected override void Awake() {
        input = new PlayerInputL(this);
        base.input = input;
        base.Awake();
    }

    public override ushort GetClientID() {
        return ClientConnectionL.i.client.ID;
    }
}
