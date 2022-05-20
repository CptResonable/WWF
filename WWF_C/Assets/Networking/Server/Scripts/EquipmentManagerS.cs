using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Server;

[System.Serializable]
public class EquipmentManagerS {
    public WeaponManagerS weaponManager;
    public Dictionary<ushort, Equipable> equipables = new Dictionary<ushort, Equipable>();

    private ushort equipableIdTicker = 0; // Used to asign ids to equipable items

    public delegate void EquipablesSpawnedDelegate(DrDatas.EquipmentDatas.EquipablesSpawnedDatas equipablesSpawnedDatas);
    public delegate void EquipableEquipedDelegate(DrDatas.EquipmentDatas.EquipableEquipedData equipableEquipedData);
    public delegate void EquipableUnequipedDelegate(DrDatas.EquipmentDatas.EquipableUnequipedData equipableUnequipedData);
    public event EquipablesSpawnedDelegate equipablesSpawnedEvent;
    public event EquipableEquipedDelegate equipableEquipedEvent;
    public event EquipableUnequipedDelegate equipableUnequipedEvent;

    private List<DrDatas.EquipmentDatas.EquipablesSpawnedDatas> equipablesSpawnedDataList = new List<DrDatas.EquipmentDatas.EquipablesSpawnedDatas>();
    private List<DrDatas.EquipmentDatas.EquipableEquipedData> equipableEquipedDataList = new List<DrDatas.EquipmentDatas.EquipableEquipedData>();
    private List<DrDatas.EquipmentDatas.EquipableUnequipedData> equipableUnequipedDataList = new List<DrDatas.EquipmentDatas.EquipableUnequipedData>();

    [SerializeField] private Loadout defaultLoadout;
    public void Initialize() {
        ServerManagerS.i.playerManager.characterSpawnedEvent += OnCharacterSpawned;
        weaponManager.Initialize(this);
    }

    /// <summary> Get all equipment update datas that should be synced to clients </summary>
    public DrDatas.EquipmentDatas.EquipmentUpdateData GetUpdates() {
        DrDatas.EquipmentDatas.EquipmentUpdateData updateData = new DrDatas.EquipmentDatas.EquipmentUpdateData(equipablesSpawnedDataList.ToArray(), 
            equipableEquipedDataList.ToArray(), 
            equipableUnequipedDataList.ToArray());

        equipablesSpawnedDataList.Clear();
        equipableEquipedDataList.Clear();
        equipableUnequipedDataList.Clear();
        return updateData;
    }
    
    private void OnCharacterSpawned(CharacterS character) {
        character.equipment.itemEquipedEvent += Equipment_itemEquipedEvent; ;
        character.equipment.itemUnequipedEvent += Equipment_itemUnequipedEvent;
        SpawnEquipment(character, defaultLoadout);
    }

    private void Equipment_itemEquipedEvent(Equipment.Type type, Equipable item) {
        DrDatas.EquipmentDatas.EquipableEquipedData equipedData = new DrDatas.EquipmentDatas.EquipableEquipedData(item.characterLS.GetClientID(), item.equipableData);
        equipableEquipedDataList.Add(equipedData);
        equipableEquipedEvent?.Invoke(equipedData);
    }

    private void Equipment_itemUnequipedEvent(Equipment.Type type, Equipable item, ushort characterId) {
        DrDatas.EquipmentDatas.EquipableUnequipedData unequipedData = new DrDatas.EquipmentDatas.EquipableUnequipedData(characterId, item.equipableData);
        equipableUnequipedDataList.Add(unequipedData);
        equipableUnequipedEvent?.Invoke(unequipedData);
    }

    private void OnCharacterDespawned() {
    }

    //TODO: FIX
    private void SpawnEquipment(CharacterS character, Loadout loadout) {

        DrDatas.EquipmentDatas.EquipableSpawnedData[] equipablesSpawnedDatasArray = new DrDatas.EquipmentDatas.EquipableSpawnedData[loadout.allItems.Count];
        for (int i = 0; i < loadout.allItems.Count; i++) {
            DrDatas.EquipmentDatas.EquipableData equipableData = new DrDatas.EquipmentDatas.EquipableData(loadout.allItems[i], equipableIdTicker);

            GameObject goEquipable = GameObject.Instantiate(GameObjects.i.equipables[equipableData.equipableEnum]); // TODO: make the spawn not hardcoded
            Equipable equipable = goEquipable.GetComponent<Equipable>();
            equipable.Initialize(equipableData);
            equipableIdTicker++; // Tick up for next id
            equipables.Add(equipableData.equipableId, equipable); // Add to dictionary
            character.equipment.AddItem(equipable); // Give character item

            DrDatas.EquipmentDatas.EquipableSpawnedData equipableSpawnedData = new DrDatas.EquipmentDatas.EquipableSpawnedData(equipableData);
            equipablesSpawnedDatasArray[i] = equipableSpawnedData;
        }

        DrDatas.EquipmentDatas.EquipablesSpawnedDatas equipablesSpawnedDatas = new DrDatas.EquipmentDatas.EquipablesSpawnedDatas(character.GetClientID(), equipablesSpawnedDatasArray);

        equipablesSpawnedDataList.Add(equipablesSpawnedDatas);
        equipablesSpawnedEvent?.Invoke(equipablesSpawnedDatas);
    }
    
    #region Messages recieved
    public void MessageRecieved(Message message, MessageReceivedEventArgs e) {
        // switch (message.Tag) {
        //     case Tags.player_requestSpawn:
        //         OnMsg_spawnRequest(message, e);
        //         break;
        //     default:
        //         break;
        // }
    }

    #endregion

    #region SendMessages
    #endregion
}
