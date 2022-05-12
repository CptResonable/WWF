using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipable : MonoBehaviour {
    //public GameObjects.EquipablesEnums equipableEnum;
    public Equipment.Type itemType;
    public DrDatas.EquipmentDatas.EquipableData equipableData;
    //public ushort itemId;
    public Rigidbody rb;
    public bool isEquiped = false;
    public Vector3 positionOffset;
    [HideInInspector] public CharacterLS character;

    // public void Initialize(GameObjects.EquipablesEnums equipableEnum, ushort itemId) {
    //     this.equipableEnum = equipableEnum;
    //     this.itemId = itemId;
    //     rb = GetComponent<Rigidbody>();
    // }

    public virtual void Initialize(DrDatas.EquipmentDatas.EquipableData equipableData) {
        this.equipableData = equipableData;
        rb = GetComponent<Rigidbody>();
    }

    public virtual void EquipL(CharacterLS character) {
        this.character = character;
        gameObject.SetActive(true);
        isEquiped = true;

        character.input.attack_1.keyDownEvent += Attack_1_keyDownEvent;
        character.fixedUpdateEvent += Character_fixedUpdateEvent;
    }

    public virtual void EquipN(CharacterN characterN) {
        gameObject.SetActive(true);
        isEquiped = true;
        //characterN.fixedUpdateEvent += Character_fixedUpdateEvent;
    }

    public virtual void UnequipL() {
        gameObject.SetActive(false);
        isEquiped = false;

        character.input.attack_1.keyDownEvent -= Attack_1_keyDownEvent;
        character.fixedUpdateEvent -= Character_fixedUpdateEvent;

        this.character = null;
    }

    public virtual void UnequipN() {
        gameObject.SetActive(false);
        isEquiped = false;
    }

    protected virtual void Character_fixedUpdateEvent() {
    }

    protected virtual void Attack_1_keyDownEvent() {
    }
    public virtual void Attack() {        
    }
    public virtual void StartReload() {
    }
}
