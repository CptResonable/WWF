using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlotEnum { back, hipRight, hipLeft }

[System.Serializable]
public class Equipment {
    protected Transform tEquipmentContainer;
    public List<Equipable> equipables;
    public Transform tAmmoPouch;
    public Transform tEquipmentSlot_back, tEquipmentSlot_hipRight, tEquipmentSlot_hipLeft;

    public enum Type { none, gun};
    public Type equipedType = Type.none;
    public Equipable equipedItem; 
    public delegate void ItemEquipedDelegate(Type type, Equipable item);
    public delegate void ItemUnequipedDelegate(Type type, Equipable item, ushort characterId);
    public event ItemEquipedDelegate itemEquipedEvent;
    public event ItemUnequipedDelegate itemUnequipedEvent;

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

    public virtual void OnUpdate_equipableUnequiped(DrDatas.EquipmentDatas.EquipableUnequipedData equipableUnequipedData) {
    }

    protected virtual void ItemEquiped(Type equipedType, Equipable equipedItem) {
        character.GetPlayer().playerData.characterData.equipmentData.EquipableEquiped(equipedItem.equipableData.equipableId);
        itemEquipedEvent?.Invoke(equipedType, equipedItem);
    }

    protected virtual void ItemUnequiped(Type unequipedType, Equipable unequipedItem, ushort characterId) {
        equipedItem = null;
        equipedType = Type.none;

        character.GetPlayer().playerData.characterData.equipmentData.EquipableUnequiped();
        itemUnequipedEvent?.Invoke(unequipedType, unequipedItem, characterId);

        switch (unequipedItem.equipmentSlot) {
            case EquipmentSlotEnum.back:
                unequipedItem.transform.parent = tEquipmentSlot_back;
                break;
            case EquipmentSlotEnum.hipRight:
                unequipedItem.transform.parent = tEquipmentSlot_hipRight;
                break;
            case EquipmentSlotEnum.hipLeft:
                unequipedItem.transform.parent = tEquipmentSlot_hipLeft;
                break;
        }

        unequipedItem.transform.localPosition = unequipedItem.attachmentOffset;
        unequipedItem.transform.localRotation = Quaternion.Euler(unequipedItem.attachmentRotationOffset);
    }
}
