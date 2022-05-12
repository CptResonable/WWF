using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loadout", menuName = "ScriptableObjects/Loadout")]
[Serializable]
[SerializeField]
public class Loadout : ScriptableObject {
    public GameObjects.EquipablesEnums slot_1;
    public GameObjects.EquipablesEnums slot_2;
    public List<GameObjects.EquipablesEnums> allItems;

    private void OnEnable() {
        allItems = new List<GameObjects.EquipablesEnums>();
        allItems.Add(slot_1);
        allItems.Add(slot_2);
    }
}