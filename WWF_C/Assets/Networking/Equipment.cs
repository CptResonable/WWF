using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equipment {
    protected Transform tEquipmentContainer;
    public List<Equipable> equipables;

    public enum Type { none, gun};
    public Type equipedType = Type.none;
    public Equipable equipedItem; 
    public delegate void ItemEquipedDelegate(Type type, Equipable item);
    public event ItemEquipedDelegate itemEquipedEvent;
    public event ItemEquipedDelegate itemUnequipedEvent;

    protected Character character;

    public virtual void Initialize(Character character) {   
        this.character = character;
        tEquipmentContainer = character.transform.Find("EquipmentContainer");
    }

    public void AddItem(Equipable item) {
        Debug.Log("Adding item");
        item.transform.parent = tEquipmentContainer;
        equipables.Add(item);
        item.gameObject.SetActive(false);

        character.GetPlayer().playerData.characterData.equipmentData.AddEquipable(item.equipableData);

        if (character == null)
            Debug.Log("Network item added");
    }

    public virtual void OnUpdate_equipableEquiped(DrDatas.EquipmentDatas.EquipableEquipedData equipableEquipedData) {

    }

    protected virtual void ItemEquiped(Type equipedType, Equipable equipedItem) {
        character.GetPlayer().playerData.characterData.equipmentData.EquipableEquiped(equipedItem.equipableData.equipableId);
        itemEquipedEvent?.Invoke(equipedType, equipedItem);
    }

    protected virtual void ItemUnequiped(Type unequipedType, Equipable unequipedItem) {
        character.GetPlayer().playerData.characterData.equipmentData.EquipableUnequiped();
        itemUnequipedEvent?.Invoke(unequipedType, unequipedItem);
    }
}
