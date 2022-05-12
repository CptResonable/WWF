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
        this.character.input.equip_s2.keyDownEvent += Equip_s2_keyDownEvent;
    }

    private void Equip_S1_keyDownEvent() {
        equipedType = Type.gun;

        equipedItem = equipables[0];
        equipedItem.EquipL(character);

        ItemEquiped(equipedItem.itemType, equipedItem);
    }

    private void Equip_s2_keyDownEvent() {
        equipedType = Type.gun;

        equipedItem = equipables[1];
        equipedItem.EquipL(character);

        ItemEquiped(equipedItem.itemType, equipedItem);
    }
}
