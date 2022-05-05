using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentLS : Equipment {
    protected new CharacterLS character;
    public override void Initialize(Character character) {
        this.character = (CharacterLS)character;
        base.Initialize(this.character);
        this.character.input.equip_s1.keyDownEvent += Equip_S1_keyDownEvent;
        Debug.Log("INIT!");
    }
    private void Equip_S1_keyDownEvent() {
        equipedType = Type.gun;

        equipedItem = equipables[0];
        equipedItem.EquipL(character);

        ItemEquiped(equipedItem.itemType, equipedItem);
    }
}
