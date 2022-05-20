using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentManagerL {
    public WeaponManagerL weaponManager;
    public Dictionary<ushort, Equipable> equipables = new Dictionary<ushort, Equipable>();

    public void Initialize() {
        GameManagerL.gameManagerLLoadedEvent += GameManagerL_gameManagerLLoadedEvent;
        ClientManagerL.i.playerManager.characterSpawnedEvent += PlayerManagerL_characterSpawnedEvent;

        weaponManager.Initialize(this);
    }

    private void GameManagerL_gameManagerLLoadedEvent(GameManagerL gameManagerL) {
        ClientManagerL.i.gameManager.gameUpdateReceivedEvent += GameManager_gameUpdateReceivedEvent;
    }

    private void GameManager_gameUpdateReceivedEvent(DrDatas.Game.GameUpdateData gameUpdateData) {
        EquipablesSpawnedUpdate(gameUpdateData.equipmentUpdateData);
        EquipablesEquipedUpdate(gameUpdateData.equipmentUpdateData);
    }

    private void PlayerManagerL_characterSpawnedEvent(DrDatas.Player.CharacterData characterData) {
        DrDatas.EquipmentDatas.CharacterEquipmentData equipmentData = characterData.equipmentData;

        // Spawn all items
        for (int i = 0; i < equipmentData.equipables.Length; i++) {
            Debug.Log("SPAWM ALL ITEMS!");
            SpawnEquipable(equipmentData.equipables[i], characterData.clientId);
        }

        // Equip equiped item
        if (equipmentData.hasItemEquiped) {
            Debug.Log("EQUIP EQUIPED ITEM!");
            DrDatas.EquipmentDatas.EquipableEquipedData equipedData = new DrDatas.EquipmentDatas.EquipableEquipedData(characterData.clientId, equipables[equipmentData.equipedEquipableId].equipableData);
            ClientManagerL.i.playerManager.allPlayers[characterData.clientId].character.equipment.OnUpdate_equipableEquiped(equipedData);
        }
    }

    private void EquipablesSpawnedUpdate(DrDatas.EquipmentDatas.EquipmentUpdateData equipmentUpdateData) {
        for (int i = 0; i < equipmentUpdateData.equipablesSpawnedDatas.Length; i++) {
            bool isLocalPlayer = (equipmentUpdateData.equipablesSpawnedDatas[i].clientId == ClientConnectionL.i.client.ID) ? true : false;
            Debug.Log("YOYOYOY ITEM SPAWNED!");

            for (int j = 0; j < equipmentUpdateData.equipablesSpawnedDatas[i].equipablesSpawnedDatas.Length; j++) {
                Debug.Log("YOYOYOY ITEM SPAWNED 2!");
                SpawnEquipable(equipmentUpdateData.equipablesSpawnedDatas[i].equipablesSpawnedDatas[j].equipableData, equipmentUpdateData.equipablesSpawnedDatas[i].clientId);
            }
        }
    }
    
    private void EquipablesEquipedUpdate(DrDatas.EquipmentDatas.EquipmentUpdateData equipmentUpdateData) {
        for (int i = 0; i < equipmentUpdateData.equipableEquipedDatas.Length; i++) {
            Debug.Log("ITEM EQUPED!");
            bool isLocalPlayer = (equipmentUpdateData.equipableEquipedDatas[i].clientId == ClientConnectionL.i.client.ID) ? true : false;
            ClientManagerL.i.playerManager.allPlayers[equipmentUpdateData.equipableEquipedDatas[i].clientId].character.equipment.OnUpdate_equipableEquiped(equipmentUpdateData.equipableEquipedDatas[i]);
            //equipmentUpdateData.equipableEquipedDatas[i]
        }
    }

    private void SpawnEquipable(DrDatas.EquipmentDatas.EquipableData equipableData, ushort clientId) {
        GameObject goEquipable = GameObject.Instantiate(GameObjects.i.equipables[equipableData.equipableEnum]);
        Equipable equipable = goEquipable.GetComponent<Equipable>();
        equipable.Initialize(equipableData);
        equipables.Add(equipableData.equipableId, equipable);

        Debug.Log("YOYOYOY ITEM SPAWNED 3!");

        ClientManagerL.i.playerManager.allPlayers[clientId].character.equipment.AddItem(equipable);
    }
}
