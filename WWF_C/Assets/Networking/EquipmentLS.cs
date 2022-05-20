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
        ItemEquiped(equipables[0].itemType, equipables[0]);
    }

    private void Equip_s2_keyDownEvent() {
        ItemEquiped(equipables[1].itemType, equipables[1]);
    }

    protected override void ItemEquiped(Type equipedType, Equipable equipedItem) {
        if (this.equipedType != Type.none)
            ItemUnequiped(this.equipedType, this.equipedItem);

        this.equipedType = equipedType;
        this.equipedItem = equipedItem;

        equipedItem.EquipL(character);

        base.ItemEquiped(equipedItem.itemType, equipedItem);
    }

    protected override void ItemUnequiped(Type unequipedType, Equipable unequipedItem) {
        equipedItem.UnequipL();
        base.ItemUnequiped(unequipedType, unequipedItem);
    }
}
