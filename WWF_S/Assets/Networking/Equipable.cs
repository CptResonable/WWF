using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipable : MonoBehaviour {
    //public GameObjects.EquipablesEnums equipableEnum;
    public Vector3 hipFirePosition;
    public Vector3 adsPosition;
    public Equipment.Type itemType;
    public DrDatas.EquipmentDatas.EquipableData equipableData;
    //public ushort itemId;
    public Rigidbody rb;
    public bool isEquiped = false;
    public Vector3 positionOffset;
    [HideInInspector] public CharacterLS characterLS;
    [HideInInspector] public CharacterN characterN;

    public virtual void Initialize(DrDatas.EquipmentDatas.EquipableData equipableData) {
        this.equipableData = equipableData;
        rb = GetComponent<Rigidbody>();
    }

    public virtual void EquipL(CharacterLS character) {
        this.characterLS = character;
        gameObject.SetActive(true);
        isEquiped = true;

        character.input.attack_1.keyDownEvent += Attack_1_keyDownEvent;
        character.fixedUpdateEvent += Character_fixedUpdateEvent;
    }

    public virtual void UnequipL() {
        gameObject.SetActive(false);
        isEquiped = false;

        characterLS.input.attack_1.keyDownEvent -= Attack_1_keyDownEvent;
        characterLS.fixedUpdateEvent -= Character_fixedUpdateEvent;

        this.characterLS = null;
        this.characterN = null;
    }

    public virtual void EquipN(CharacterN character) {
        this.characterN = character;
        gameObject.SetActive(true);
        isEquiped = true;
        transform.parent = characterN.bodyN.hand_R;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public virtual void UnequipN() {
        gameObject.SetActive(false);
        isEquiped = false;

        this.characterLS = null;
        this.characterN = null;
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
