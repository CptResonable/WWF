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
    public event EquipablesSpawnedDelegate equipablesSpawnedEvent;
    public event EquipableEquipedDelegate equipableEquipedEvent;

    private List<DrDatas.EquipmentDatas.EquipablesSpawnedDatas> equipablesSpawnedDataList = new List<DrDatas.EquipmentDatas.EquipablesSpawnedDatas>();
    private List<DrDatas.EquipmentDatas.EquipableEquipedData> equipableEquipedDataList = new List<DrDatas.EquipmentDatas.EquipableEquipedData>();

    [SerializeField] private Loadout defaultLoadout;
    public void Initialize() {
        ServerManagerS.i.playerManager.characterSpawnedEvent += OnCharacterSpawned;
        weaponManager.Initialize(this);
    }

    /// <summary> Get all equipment update datas that should be synced to clients </summary>
    public DrDatas.EquipmentDatas.EquipmentUpdateData GetUpdates() {
        DrDatas.EquipmentDatas.EquipmentUpdateData updateData = new DrDatas.EquipmentDatas.EquipmentUpdateData(equipablesSpawnedDataList.ToArray(), equipableEquipedDataList.ToArray());
        equipablesSpawnedDataList.Clear();
        equipableEquipedDataList.Clear();
        return updateData;
    }
    
    private void OnCharacterSpawned(CharacterS character) {
        character.equipment.itemEquipedEvent += OnItemEquiped;
        SpawnEquipment(character, defaultLoadout);
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


    ////TODO: FIX
    //private void SpawnEquipment(CharacterS character) {
    //    DrDatas.EquipmentDatas.EquipableData equipableData = new DrDatas.EquipmentDatas.EquipableData(GameObjects.EquipablesEnums.wep_AK, equipableIdTicker);

    //    GameObject goEquipable = GameObject.Instantiate(GameObjects.i.equipables[equipableData.equipableEnum]); // TODO: make the spawn not hardcoded
    //    Equipable equipable = goEquipable.GetComponent<Equipable>();
    //    equipable.Initialize(equipableData);     
    //    equipableIdTicker++; // Tick up for next id
    //    equipables.Add(equipableData.equipableId, equipable); // Add to dictionary
    //    character.equipment.AddItem(equipable); // Give character item

    //    DrDatas.EquipmentDatas.EquipableSpawnedData equipableSpawnedData = new DrDatas.EquipmentDatas.EquipableSpawnedData(equipableData);
    //    DrDatas.EquipmentDatas.EquipableSpawnedData[] equipablesSpawnedDatasArray = new DrDatas.EquipmentDatas.EquipableSpawnedData[1] {
    //        equipableSpawnedData
    //    };
    //    DrDatas.EquipmentDatas.EquipablesSpawnedDatas equipablesSpawnedDatas = new DrDatas.EquipmentDatas.EquipablesSpawnedDatas(character.GetClientID(), equipablesSpawnedDatasArray);

    //    equipablesSpawnedDataList.Add(equipablesSpawnedDatas);
    //    equipablesSpawnedEvent?.Invoke(equipablesSpawnedDatas);
    //}

    private void OnItemEquiped(Equipment.Type type, Equipable item) {
        DrDatas.EquipmentDatas.EquipableEquipedData equipedData = new DrDatas.EquipmentDatas.EquipableEquipedData(item.character.GetClientID(), item.equipableData);
        equipableEquipedDataList.Add(equipedData);
        equipableEquipedEvent?.Invoke(equipedData);
        //ServerManagerS.i.gameManager.AddItemEquipData(new DarkRiftSerializables.EquipmentSerializables.EquipableEquipedData(item.character.GetClientID(), item.itemId));
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
