//using System.Collections;
//using System.Collections.Generic;
//using DarkRift;
//using UnityEngine;

//public enum Tags {
//    LoginRequest = 0,
//    LoginRequestAccepted = 1,
//    LoginRequestDenied = 2,

//    LobbyJoinRoomRequest = 100,
//    LobbyJoinRoomDenied = 101,
//    LobbyJoinRoomAccepted = 102,

//    PlayerJoinRequest = 200,
//    PlayerJoinAccepted = 201,
//    PlayerJoinDenied = 202,
//    PlayerInput = 203,
//    PlayerBodyData = 204,
//    PlayerLeaveGame = 205,
//    PlayerJoinedSpawnData = 206,

//    GameUpdate = 300,

//    InventoryEquipItem = 401,
//    InventoryUnequipItem = 402,
//    InventorySpawnItem = 403,

//    ItemTrigger = 500,

//    HealthPlayerHit = 600,
//    HealthPlayedDied = 601,
//    HealthPlayerDowned = 602,
//    HealthRequestGetBackUp = 603,
//    HealthGetBackUp = 604,
//    healthDamageTaken = 605,
//    healthHeal = 606,

//    WeaponFired = 700,
//    WeaponFiredResponse = 701,
//    WeaponGrenadeLit = 702,
//    WeaponGrenadeThrown = 703,
//    WeaponGrenadeExploded = 704,
//    WeaponGrenadeThrownResponse = 705,

//    SpawnRequestSpawn = 800,
//    SpawnSpawnPlayer = 801,
//}

//public class Serializables {

//    public struct IdData : IDarkRiftSerializable {
//        public ushort id;

//        public IdData(ushort id) {
//            this.id = id;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            id = e.Reader.ReadUInt16();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(id);
//        }
//    }

//    public struct LoginRequestData : IDarkRiftSerializable {
//        public string Name;

//        public LoginRequestData(string name) {
//            Name = name;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            Name = e.Reader.ReadString();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(Name);
//        }
//    }

//    public struct LoginInfoData : IDarkRiftSerializable {
//        public ushort Id;
//        public LobbyInfoData Data;

//        public LoginInfoData(ushort id, LobbyInfoData data) {
//            Id = id;
//            Data = data;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            Id = e.Reader.ReadUInt16();
//            Data = e.Reader.ReadSerializable<LobbyInfoData>();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(Id);
//            e.Writer.Write(Data);
//        }
//    }

//    public struct LobbyInfoData : IDarkRiftSerializable {
//        public RoomData[] Rooms;

//        public LobbyInfoData(RoomData[] rooms) {
//            Rooms = rooms;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            Rooms = e.Reader.ReadSerializables<RoomData>();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(Rooms);
//        }
//    }

//    public struct RoomData : IDarkRiftSerializable {
//        public string Name;
//        public byte Slots;
//        public byte MaxSlots;

//        public RoomData(string name, byte slots, byte maxSlots) {
//            Name = name;
//            Slots = slots;
//            MaxSlots = maxSlots;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            Name = e.Reader.ReadString();
//            Slots = e.Reader.ReadByte();
//            MaxSlots = e.Reader.ReadByte();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(Name);
//            e.Writer.Write(Slots);
//            e.Writer.Write(MaxSlots);
//        }
//    }

//    public struct JoinRoomRequest : IDarkRiftSerializable {
//        public string RoomName;

//        public JoinRoomRequest(string name) {
//            RoomName = name;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            RoomName = e.Reader.ReadString();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(RoomName);
//        }
//    }

//    public struct PlayerJoinGameData : IDarkRiftSerializable {

//        //private Dictionary<ClientConnection, ServerPlayer> players;
//        public SpawnPlayerSpawnData[] spawnDatas;

//        public PlayerJoinGameData(Dictionary<ClientConnection, ServerPlayer> players) {

//            //this.players = players;
//            List<SpawnPlayerSpawnData> spawnDatas = new List<SpawnPlayerSpawnData>();
//            foreach (ServerPlayer player in players.Values) {
//                spawnDatas.Add(new SpawnPlayerSpawnData(player.clientConnection.Client.ID, player.clientConnection.Name, player.playerBodyData, player.player.inventory.inventoryData));
//            }
//            this.spawnDatas = spawnDatas.ToArray();
//        }

//        public void Deserialize(DeserializeEvent e) {
//            spawnDatas = e.Reader.ReadSerializables<SpawnPlayerSpawnData>();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(spawnDatas);
//        }
//    }

//    public struct PlayerLeaveGameData : IDarkRiftSerializable {
//        public ushort playerId;

//        public PlayerLeaveGameData(ushort playerId) {
//            this.playerId = playerId;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            playerId = e.Reader.ReadUInt16();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(playerId);
//        }
//    }

//    public struct PlayerInputData : IDarkRiftSerializable {
//        public Dictionary<PlayerInput.InputKeys, Key> keys;
//        public byte hotkey; // Hotkey pressed. 0 = no hotkey pressed.
//        public Quaternion headRotation;

//        public PlayerInputData(Quaternion headRotation) {
//            keys = new Dictionary<PlayerInput.InputKeys, Key>() {
//                [PlayerInput.InputKeys.forward] = new Key(),
//                [PlayerInput.InputKeys.backward] = new Key(),
//                [PlayerInput.InputKeys.left] = new Key(),
//                [PlayerInput.InputKeys.right] = new Key(),
//                [PlayerInput.InputKeys.jump] = new Key(),
//                [PlayerInput.InputKeys.ads] = new Key(),
//                [PlayerInput.InputKeys.fire_1] = new Key(),
//                [PlayerInput.InputKeys.reload] = new Key(),
//                [PlayerInput.InputKeys.sprint] = new Key(),
//            };
//            hotkey = 0;
//            this.headRotation = headRotation;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            keys = new Dictionary<PlayerInput.InputKeys, Key>();

//            for (int i = 0; i < PlayerInput.nrInputKeys; i++) {
//                keys.Add((PlayerInput.InputKeys)i, new Key(e.Reader.ReadBoolean()));
//            }

//            hotkey = e.Reader.ReadByte();
//            headRotation = new Quaternion(e.Reader.ReadSingle(), e.Reader.ReadSingle(), e.Reader.ReadSingle(), e.Reader.ReadSingle());
//        }

//        public void Serialize(SerializeEvent e) {
//            for (int i = 0; i < PlayerInput.nrInputKeys; i++) {
//                e.Writer.Write(keys[(PlayerInput.InputKeys)i].isDown);
//            }

//            e.Writer.Write(hotkey);
//            e.Writer.Write(headRotation.x);
//            e.Writer.Write(headRotation.y);
//            e.Writer.Write(headRotation.z);
//            e.Writer.Write(headRotation.w);
//        }
//    }

//    public struct PlayerBodyData : IDarkRiftSerializable {
//        public ushort id;

//        public Vector3 pos_pelvis;
//        public Vector3 vel_pelvis;
//        public Quaternion[] nodes;

//        public PlayerBodyData(ushort id, Bodypart[] bodyparts) {
//            this.id = id;

//            pos_pelvis = bodyparts[1].physical.position;
//            vel_pelvis = bodyparts[1].rb.velocity;

//            nodes = new Quaternion[16];
//            for (int i = 1; i < bodyparts.Length; i++) {
//                nodes[i - 1] = bodyparts[i].physical.rotation;
//            }
//        }

//        public void Deserialize(DeserializeEvent e) {
//            id = e.Reader.ReadUInt16();

//            // Read pelvis velocity and position.
//            pos_pelvis = e.Reader.ReadVector3();
//            vel_pelvis = e.Reader.ReadVector3();

//            // Read rotation for all nodes.
//            nodes = new Quaternion[16];
//            for (int i = 0; i < 16; i++) {
//                nodes[i] = e.Reader.ReadQuaternion();
//            }
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(id);

//            // Write pelvis velocity and position.
//            e.Writer.WriteVector3(pos_pelvis);
//            e.Writer.WriteVector3(vel_pelvis);

//            // Write rotation for all nodes.
//            for (int i = 0; i < 16; i++) {
//                e.Writer.WriteQuaternion(nodes[i]);
//            }
//        }
//    }

//    public struct GameUpdateData : IDarkRiftSerializable {
//        public PlayerBodyData[] bodyDatas;

//        public GameUpdateData(PlayerBodyData[] bodyDatas) {
//            this.bodyDatas = bodyDatas;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            bodyDatas = e.Reader.ReadSerializables<PlayerBodyData>();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(bodyDatas);
//        }
//    }

//    public struct InventoryEquipItemData : IDarkRiftSerializable {
//        public ushort playerId;
//        public ushort itemId;
//        public ItemSlot.SlotIDs slotId;

//        public InventoryEquipItemData(ushort playerId, ushort itemId, ItemSlot.SlotIDs slotId) {
//            this.playerId = playerId;
//            this.itemId = itemId;
//            this.slotId = slotId;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            playerId = e.Reader.ReadUInt16();
//            itemId = e.Reader.ReadUInt16();
//            slotId = (ItemSlot.SlotIDs)e.Reader.ReadUInt16();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(playerId);
//            e.Writer.Write(itemId);
//            e.Writer.Write((ushort)slotId);
//        }
//    }

//    public struct InventoryUnequipItemData : IDarkRiftSerializable {
//        public ushort playerId;
//        public ItemSlot.SlotIDs slotId;

//        public InventoryUnequipItemData(ushort playerId, ItemSlot.SlotIDs slotId) {
//            this.playerId = playerId;
//            this.slotId = slotId;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            playerId = e.Reader.ReadUInt16();
//            slotId = (ItemSlot.SlotIDs)e.Reader.ReadUInt16();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(playerId);
//            e.Writer.Write((ushort)slotId);
//        }
//    }

//    public struct InventorySpawnItemData : IDarkRiftSerializable {
//        public ushort playerId;
//        public ItemData itemData;

//        public InventorySpawnItemData(ushort playerId, ItemData itemData) {
//            this.playerId = playerId;
//            this.itemData = itemData;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            playerId = e.Reader.ReadUInt16();
//            itemData = e.Reader.ReadSerializable<ItemData>();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(playerId);
//            e.Writer.Write(itemData);
//        }
//    }

//    public struct InventoryData : IDarkRiftSerializable {
//        public ItemData[] itemDatas;
//        public ushort slotHandR_itemId;

//        public InventoryData(ItemData[] itemDatas, ushort slotHandR_itemId) {
//            this.itemDatas = itemDatas;
//            this.slotHandR_itemId = slotHandR_itemId;

//            Debug.Log("TEST:1");
//            Debug.Log("item data count: " + itemDatas.Length);
//        }

//        public void AddItem(Item item) {
//            ItemData[] newItemDatas = new ItemData[itemDatas.Length + 1];
//            for (int i = 0; i < itemDatas.Length; i++) {
//                newItemDatas[i] = itemDatas[i];
//            }
//            newItemDatas[itemDatas.Length] = new ItemData(item.itemData.itemEnum, item.itemData.itemId);
//            itemDatas = newItemDatas;
//            Debug.Log("item data count: " + itemDatas.Length);
//        }

//        public void Deserialize(DeserializeEvent e) {
//            itemDatas = e.Reader.ReadSerializables<ItemData>();
//            slotHandR_itemId = e.Reader.ReadUInt16();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(itemDatas);
//            e.Writer.Write(slotHandR_itemId);
//        }
//    }

//    public struct ItemData : IDarkRiftSerializable {
//        public GameObjects.ItemEnums itemEnum;
//        public ushort itemId;

//        public ItemData(GameObjects.ItemEnums itemEnum, ushort itemId) {
//            this.itemEnum = itemEnum;
//            this.itemId = itemId;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            itemEnum = (GameObjects.ItemEnums)e.Reader.ReadUInt16();
//            itemId = e.Reader.ReadUInt16();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write((ushort)itemEnum);
//            e.Writer.Write(itemId);
//        }
//    }

//    public struct ItemSpawnData : IDarkRiftSerializable {
//        public GameObjects.ItemEnums itemEnum;
//        public ushort itemId;

//        public ItemSpawnData(GameObjects.ItemEnums itemEnum, ushort itemId) {
//            this.itemEnum = itemEnum;
//            this.itemId = itemId;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            itemId = e.Reader.ReadUInt16();
//            itemEnum = (GameObjects.ItemEnums)e.Reader.ReadUInt16();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write((ushort)itemEnum);
//            e.Writer.Write(itemId);
//        }
//    }

//    public struct ItemTrigger : IDarkRiftSerializable {
//        public ushort itemId;

//        public ItemTrigger(ushort itemId) {
//            this.itemId = itemId;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            itemId = e.Reader.ReadUInt16();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(itemId);
//        }
//    }

//    public struct BulletData : IDarkRiftSerializable {
//        public ushort bulletId;
//        public ushort firedById;

//        public BulletData(ushort bulletId, ushort firedById) {
//            this.bulletId = bulletId;
//            this.firedById = firedById;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            bulletId = e.Reader.ReadUInt16();
//            firedById = e.Reader.ReadUInt16();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(bulletId);
//            e.Writer.Write(firedById);
//        }
//    }

//    public struct HealthPlayerDownedData : IDarkRiftSerializable {
//        public ushort playerId;

//        public HealthPlayerDownedData(ushort playerId) {
//            this.playerId = playerId;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            playerId = e.Reader.ReadUInt16();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(playerId);
//        }
//    }

//    public struct HealthPlayerHitData : IDarkRiftSerializable {
//        public ushort playerId;
//        public BulletData bulletData;
//        public float newHp;

//        public HealthPlayerHitData(ushort playerId, BulletData bulletData, float newHp) {
//            this.playerId = playerId;
//            this.bulletData = bulletData;
//            this.newHp = newHp;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            playerId = e.Reader.ReadUInt16();
//            bulletData = e.Reader.ReadSerializable<BulletData>();
//            newHp = e.Reader.ReadSingle();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(playerId);
//            e.Writer.Write(bulletData);
//            e.Writer.Write(newHp);
//        }
//    }

//    public struct HealthDamageTakenData : IDarkRiftSerializable {
//        public ushort playerId;
//        public float damage;

//        public HealthDamageTakenData(ushort playerId, float damage) {
//            this.playerId = playerId;
//            this.damage = damage;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            playerId = e.Reader.ReadUInt16();
//            damage = e.Reader.ReadSingle();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(playerId);
//            e.Writer.Write(damage);
//        }
//    }

//    public struct HealthHealedData : IDarkRiftSerializable {
//        public ushort playerId;
//        public float healAmount;

//        public HealthHealedData(ushort playerId, float healAmount) {
//            this.playerId = playerId;
//            this.healAmount = healAmount;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            playerId = e.Reader.ReadUInt16();
//            healAmount = e.Reader.ReadSingle();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(playerId);
//            e.Writer.Write(healAmount);
//        }
//    }

//    public struct HealthPlayerDiedData : IDarkRiftSerializable {
//        public ushort playerId;

//        public HealthPlayerDiedData(ushort playerId) {
//            this.playerId = playerId;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            playerId = e.Reader.ReadUInt16();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(playerId);
//        }
//    }

//    public struct WeaponFiredData : IDarkRiftSerializable {
//        public BulletData bulletData;
//        public ItemData weaponData;
//        public Vector3 bulletStartPos;
//        public Vector3 bulletDir;

//        public WeaponFiredData(BulletData bulletData, ItemData weaponData, Vector3 bulletStartPos, Vector3 bulletDir) {
//            this.bulletData = bulletData;
//            this.weaponData = weaponData;
//            this.bulletStartPos = bulletStartPos;
//            this.bulletDir = bulletDir;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            bulletData = e.Reader.ReadSerializable<BulletData>();
//            weaponData = e.Reader.ReadSerializable<ItemData>();
//            bulletStartPos = e.Reader.ReadVector3();
//            bulletDir = e.Reader.ReadVector3();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(bulletData);
//            e.Writer.Write(weaponData);
//            e.Writer.WriteVector3(bulletStartPos);
//            e.Writer.WriteVector3(bulletDir);
//        }
//    }

//    public struct WeaponFiredResponse : IDarkRiftSerializable {
//        public ushort weaponId;
//        public ushort bulletTmpId;
//        public ushort bulletNewId;
//        public bool success;

//        public WeaponFiredResponse(ushort weaponId, ushort bulletTmpId, ushort bulletNewId, bool success) {
//            this.weaponId = weaponId;
//            this.bulletTmpId = bulletTmpId;
//            this.bulletNewId = bulletNewId;
//            this.success = success;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            weaponId = e.Reader.ReadUInt16();
//            bulletTmpId = e.Reader.ReadUInt16();
//            bulletNewId = e.Reader.ReadUInt16();
//            success = e.Reader.ReadBoolean();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(weaponId);
//            e.Writer.Write(bulletTmpId);
//            e.Writer.Write(bulletNewId);
//            e.Writer.Write(success);
//        }
//    }
    
//    public struct WeaponGrenadeLitData : IDarkRiftSerializable {
//        public ItemData grenadeData;
    
//        public WeaponGrenadeLitData(ItemData grenadeData) {
//            this.grenadeData = grenadeData;
//        }
    
//        public void Deserialize(DeserializeEvent e) {
//            grenadeData = e.Reader.ReadSerializable<ItemData>();
//        }
    
//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(grenadeData);
//        }
//    }

//    public struct WeaponGrenadeThrownData : IDarkRiftSerializable {
//        public ItemData grenadeData;
//        public Vector3 grenadeStartPos;
//        public Vector3 velocity;

//        public WeaponGrenadeThrownData(ItemData grenadeData, Vector3 grenadeStartPos, Vector3 velocity) {
//            this.grenadeData = grenadeData;
//            this.grenadeStartPos = grenadeStartPos;
//            this.velocity = velocity;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            grenadeData = e.Reader.ReadSerializable<ItemData>();
//            grenadeStartPos = e.Reader.ReadVector3();
//            velocity = e.Reader.ReadVector3();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(grenadeData);
//            e.Writer.WriteVector3(grenadeStartPos);
//            e.Writer.WriteVector3(velocity);
//        }
//    }
    
//    public struct WeaponGrenadeExplodedData : IDarkRiftSerializable {
//        public ItemData grenadeData;
//        public Vector3 position;
//        public float force; 
//        public float radius;
        
//        public WeaponGrenadeExplodedData(ItemData grenadeData, Vector3 position, float force, float radius) {
//            this.grenadeData = grenadeData;
//            this.position = position;
//            this.force = force;
//            this.radius = radius;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            grenadeData = e.Reader.ReadSerializable<ItemData>();
//            position = e.Reader.ReadVector3();
//            force = e.Reader.ReadSingle();
//            radius = e.Reader.ReadSingle();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(grenadeData);
//            e.Writer.WriteVector3(position);
//            e.Writer.Write(force);
//            e.Writer.Write(radius);
//        }
//    }
    
//    public struct SpawnRequestSpawnData : IDarkRiftSerializable {
//        public ushort spawnPointId;

//        public SpawnRequestSpawnData(ushort spawnPointId) {
//            this.spawnPointId = spawnPointId;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            spawnPointId = e.Reader.ReadUInt16();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(spawnPointId);
//        }
//    }

//    public struct SpawnPlayerSpawnData : IDarkRiftSerializable {
//        public ushort playerId;
//        public string name;
//        public PlayerBodyData bodyData;
//        public InventoryData inventoryData;

//        public SpawnPlayerSpawnData(ushort playerId, string name, PlayerBodyData bodyData, InventoryData inventoryData) {
//            this.playerId = playerId;
//            this.name = name;
//            this.bodyData = bodyData;
//            this.inventoryData = inventoryData;
//        }

//        public void Deserialize(DeserializeEvent e) {
//            playerId = e.Reader.ReadUInt16();
//            name = e.Reader.ReadString();
//            bodyData = e.Reader.ReadSerializable<PlayerBodyData>();
//            inventoryData = e.Reader.ReadSerializable<InventoryData>();
//        }

//        public void Serialize(SerializeEvent e) {
//            e.Writer.Write(playerId);
//            e.Writer.Write(name);
//            e.Writer.Write(bodyData);
//            e.Writer.Write(inventoryData);
//        }
//    }
//}

