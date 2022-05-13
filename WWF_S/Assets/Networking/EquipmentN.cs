using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentN : Equipment {
    public override void OnUpdate_equipableEquiped(DrDatas.EquipmentDatas.EquipableEquipedData equipableEquipedData) {

        Equipable item = null;
        for (int i = 0; i < equipables.Count; i++) {
            if (equipables[i].equipableData.equipableId == equipableEquipedData.equipableData.equipableId) {
                item = equipables[i];
                break;
            }
        }

        if (item == null) {
            Debug.LogError("Can't equip item as the player does not have it in it's inventory!");
            return;
        }

        equipedItem = item;
        item.EquipN((CharacterN)character);

        ItemEquiped(item.itemType, item);
    }
}
