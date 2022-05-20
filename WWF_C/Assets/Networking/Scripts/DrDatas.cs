using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;

public class DrDatas {
    public class Login {
        public struct LoginData : IDarkRiftSerializable {
            public string username;

            public LoginData(string username) {
                this.username = username;
            }

            public void Deserialize(DeserializeEvent e) {
                username = e.Reader.ReadString();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(username);
            }
        }

        public struct LoginSuccessData : IDarkRiftSerializable {
            public string sceneName;

            public LoginSuccessData(string sceneName) {
                this.sceneName = sceneName;
            }

            public void Deserialize(DeserializeEvent e) {
                sceneName = e.Reader.ReadString();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(sceneName);
            }
        }

        public struct LoginFailedData : IDarkRiftSerializable {
            public string errorReason;

            public LoginFailedData(string errorReason) {
                this.errorReason = errorReason;
            }

            public void Deserialize(DeserializeEvent e) {
                errorReason = e.Reader.ReadString();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(errorReason);
            }
        }
    }

    public class Player {
        public struct PlayerData : IDarkRiftSerializable {
            public ushort clientId;
            public string username;
            public global::Player.PlayerState state;
            public CharacterData characterData;

            public PlayerData(ushort clientId, string username, global::Player.PlayerState state) {
                this.clientId = clientId;
                this.username = username;
                this.state = state;
                characterData = new CharacterData(clientId, Vector3.zero, Quaternion.identity, new DrDatas.EquipmentDatas.CharacterEquipmentData(new EquipmentDatas.EquipableData[0])); // Dummy character data. Is replaced when character is acually spawned
            }

            public void Deserialize(DeserializeEvent e) {
                clientId = e.Reader.ReadUInt16();
                username = e.Reader.ReadString();
                state = (global::Player.PlayerState)e.Reader.ReadUInt16();

                if (state == global::Player.PlayerState.spawned)
                    characterData = e.Reader.ReadSerializable<CharacterData>();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(clientId);
                e.Writer.Write(username);
                e.Writer.Write((ushort)state);

                if (state == global::Player.PlayerState.spawned)
                    e.Writer.Write(characterData);
            }
        }

        public struct PlayerBodyData : IDarkRiftSerializable {
            public ushort clientId;
            public Vector3 rootPosition;
            public Quaternion[] rotations;

            public PlayerBodyData(ushort clientId, Body body) {
                this.clientId = clientId;

                rootPosition = body.pelvis.ragdoll.position; Debug.Log("NOTE: " + "CHANGED ARMATURE TO PELVIS!");
                rotations = new Quaternion[body.bodyparts.Length];
                for (int i = 0; i < body.bodyparts.Length; i++) {
                    rotations[i] = body.bodyparts[i].ragdoll.rotation;
                }
            }

            public void Update(Body body) {
                rootPosition = body.pelvis.ragdoll.position; Debug.Log("NOTE: " + "CHANGED ARMATURE TO PELVIS!");
                for (int i = 0; i < body.bodyparts.Length; i++) {
                    rotations[i] = body.bodyparts[i].ragdoll.rotation;
                }
            }

            public void Deserialize(DeserializeEvent e) {
                clientId = e.Reader.ReadUInt16();

                rootPosition = e.Reader.ReadVector3(); // Read position

                int rotationCount = e.Reader.ReadByte(); // Read how many rotation nodes there are

                // Read rotations
                rotations = new Quaternion[rotationCount];
                for (int i = 0; i < rotationCount; i++) {
                    rotations[i] = e.Reader.ReadQuaternion();
                }
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(clientId);

                e.Writer.WriteVector3(rootPosition); // Write position
                
                e.Writer.Write((byte)rotations.Length); // Write how many rotation nodes there are

                // Write rotations
                for (int i = 0; i < rotations.Length; i++){
                    e.Writer.WriteQuaternion(rotations[i]);
                }
            }
        }

        public struct PlayerDisconnectedData : IDarkRiftSerializable {
            public ushort clientId;

            public PlayerDisconnectedData(ushort clientId) {
                this.clientId = clientId;
            }

            public void Deserialize(DeserializeEvent e) {
                clientId = e.Reader.ReadUInt16();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(clientId);
            }
        }

        public struct AllPlayerDatas : IDarkRiftSerializable {
            public PlayerData[] playerDatas;

            public AllPlayerDatas(PlayerData[] playerDatas) {
                this.playerDatas = playerDatas;
            }

            public void Deserialize(DeserializeEvent e) {
                playerDatas = e.Reader.ReadSerializables<PlayerData>();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(playerDatas);
            }
        }

        public struct RequestCharacterSpawnData : IDarkRiftSerializable {
            public ushort spawnpointId;

            public RequestCharacterSpawnData(ushort spawnpointId) {
                this.spawnpointId = spawnpointId;
            }

            public void Deserialize(DeserializeEvent e) {
                spawnpointId = e.Reader.ReadUInt16();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(spawnpointId);
            }
        }

        public struct CharacterData : IDarkRiftSerializable {
            public ushort clientId;
            public Vector3 position;
            public Quaternion rotation;
            public EquipmentDatas.CharacterEquipmentData equipmentData;

            public CharacterData(ushort clientId, Vector3 position, Quaternion rotation, EquipmentDatas.CharacterEquipmentData equipmentData) {
                this.clientId = clientId;
                this.position = position;
                this.rotation = rotation;
                this.equipmentData = equipmentData;
            }
            public void Deserialize(DeserializeEvent e) {
                clientId = e.Reader.ReadUInt16();
                position = e.Reader.ReadVector3();
                rotation = e.Reader.ReadQuaternion();
                equipmentData = e.Reader.ReadSerializable<EquipmentDatas.CharacterEquipmentData>();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(clientId);
                e.Writer.WriteVector3(position);
                e.Writer.WriteQuaternion(rotation);
                e.Writer.Write(equipmentData);
            }
        }

        public struct PlayerInputData : IDarkRiftSerializable {
            public Vector2 vecMoveXZ;
            public Vector2 headPitchYaw;
            public bool[] actionStates;

            public PlayerInputData(PlayerInput playerInput) {
                vecMoveXZ = playerInput.vecMoveXZ;
                headPitchYaw = playerInput.headPitchYaw;

                actionStates = new bool[playerInput.actions.Length];
                for (int i = 0; i < actionStates.Length; i++) {
                    actionStates[i] = playerInput.actions[i].isTriggered;
                }
            }

            public void Deserialize(DeserializeEvent e) {
                vecMoveXZ = e.Reader.ReadVector2();
                headPitchYaw = e.Reader.ReadVector2();

                // Read action states
                actionStates = new bool[9];
                for (int i = 0; i < actionStates.Length; i++) {
                    actionStates[i] = e.Reader.ReadBoolean();
                }
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.WriteVector2(vecMoveXZ);
                e.Writer.WriteVector2(headPitchYaw);

                // Write action states
                for (int i = 0; i < actionStates.Length; i++) {
                    e.Writer.Write(actionStates[i]);
                }
            }
        }
    }
    
    public class Game {
        public struct GameUpdateData : IDarkRiftSerializable {
            public DrDatas.Player.PlayerBodyData[] bodyDatas;
            public DrDatas.EquipmentDatas.EquipmentUpdateData equipmentUpdateData;
            public DrDatas.Guns.WeaponUpdateData weaponUpdateData;
            public DrDatas.HealthData.HealthUpdateData healthUpdateData;

            public GameUpdateData(DrDatas.Player.PlayerBodyData[] bodyDatas, DrDatas.EquipmentDatas.EquipmentUpdateData equipmentUpdateData, DrDatas.Guns.WeaponUpdateData weaponUpdateData, DrDatas.HealthData.HealthUpdateData healthUpdateData) {
                this.bodyDatas = bodyDatas;
                this.equipmentUpdateData = equipmentUpdateData;
                this.weaponUpdateData = weaponUpdateData;
                this.healthUpdateData = healthUpdateData;
            }

            public void Deserialize(DeserializeEvent e) {
                bodyDatas = e.Reader.ReadSerializables<DrDatas.Player.PlayerBodyData>();
                equipmentUpdateData = e.Reader.ReadSerializable<DrDatas.EquipmentDatas.EquipmentUpdateData>();
                weaponUpdateData = e.Reader.ReadSerializable<DrDatas.Guns.WeaponUpdateData>();
                healthUpdateData = e.Reader.ReadSerializable<DrDatas.HealthData.HealthUpdateData>();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(bodyDatas);
                e.Writer.Write(equipmentUpdateData);
                e.Writer.Write(weaponUpdateData);
                e.Writer.Write(healthUpdateData);
            }
        }
    }

    public class EquipmentDatas {
        public struct EquipmentUpdateData : IDarkRiftSerializable {
            public EquipablesSpawnedDatas[] equipablesSpawnedDatas;
            public EquipableEquipedData[] equipableEquipedDatas;

            public EquipmentUpdateData(EquipablesSpawnedDatas[] equipablesSpawnedDatas, EquipableEquipedData[] equipableEquipedDatas) {
                this.equipablesSpawnedDatas = equipablesSpawnedDatas;
                this.equipableEquipedDatas = equipableEquipedDatas;
            }

            public void Deserialize(DeserializeEvent e) {
                equipablesSpawnedDatas = e.Reader.ReadSerializables<EquipablesSpawnedDatas>();
                equipableEquipedDatas = e.Reader.ReadSerializables<EquipableEquipedData>();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(equipablesSpawnedDatas);
                e.Writer.Write(equipableEquipedDatas);
            }
        }

        public struct EquipableSpawnedData : IDarkRiftSerializable {
            public EquipableData equipableData;
            public EquipableSpawnedData(EquipableData equipableData) {
                this.equipableData = equipableData;
            }

            public void Deserialize(DeserializeEvent e) {
                equipableData = e.Reader.ReadSerializable<EquipableData>();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(equipableData);
            }
        }

        public struct EquipablesSpawnedDatas : IDarkRiftSerializable {
            public ushort clientId;
            public EquipableSpawnedData[] equipablesSpawnedDatas;

            public EquipablesSpawnedDatas(ushort clientId, EquipableSpawnedData[] equipableSpawnedDatas) {
                this.clientId = clientId;
                this.equipablesSpawnedDatas = equipableSpawnedDatas;
            }

            public void Deserialize(DeserializeEvent e) {
                clientId = e.Reader.ReadUInt16();
                equipablesSpawnedDatas = e.Reader.ReadSerializables<EquipableSpawnedData>();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(clientId);
                e.Writer.Write(equipablesSpawnedDatas);
            }
        }

        public struct EquipableEquipedData : IDarkRiftSerializable {
            public ushort clientId;
            public EquipableData equipableData;

            public EquipableEquipedData(ushort clientId, EquipableData equipableData) {
                this.clientId = clientId;
                this.equipableData = equipableData;
            }
      
            public void Deserialize(DeserializeEvent e) {
                clientId = e.Reader.ReadUInt16();
                equipableData = e.Reader.ReadSerializable<EquipableData>();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(clientId);
                e.Writer.Write(equipableData);
            }
        }

        public struct EquipableData : IDarkRiftSerializable {
            public GameObjects.EquipablesEnums equipableEnum;
            public ushort equipableId;

            public EquipableData(GameObjects.EquipablesEnums equipableEnum, ushort equipableId) {
                this.equipableEnum = equipableEnum;
                this.equipableId = equipableId;
            }

            public void Deserialize(DeserializeEvent e) {
                equipableEnum = (GameObjects.EquipablesEnums)e.Reader.ReadUInt16();
                equipableId = e.Reader.ReadUInt16();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write((ushort)equipableEnum);
                e.Writer.Write(equipableId);
            }
        }

        public struct CharacterEquipmentData : IDarkRiftSerializable {
            public EquipableData[] equipables;
            public bool hasItemEquiped;
            public ushort equipedEquipableId;
            public CharacterEquipmentData(EquipableData[] equipables) {
                this.equipables = equipables;
                hasItemEquiped = false;
                equipedEquipableId = ushort.MaxValue;
            }

            public void EquipableEquiped(ushort equipableId) {
                hasItemEquiped = true;
                equipedEquipableId = equipableId;
            }

            public void EquipableUnequiped() {
                hasItemEquiped = false;
                equipedEquipableId = ushort.MaxValue;
            }

            public void AddEquipable(EquipableData equipable) {
                EquipableData[] newArray = new EquipableData[equipables.Length + 1];

                for (int i = 0; i < equipables.Length; i++) {
                    newArray[i] = equipables[i];
                }

                newArray[equipables.Length] = equipable;
                equipables = newArray;
            }

            public void Deserialize(DeserializeEvent e) {
                equipables = e.Reader.ReadSerializables<EquipableData>();
                hasItemEquiped = e.Reader.ReadBoolean();
                equipedEquipableId = e.Reader.ReadUInt16();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(equipables);
                e.Writer.Write(hasItemEquiped);
                e.Writer.Write(equipedEquipableId);
            }
        }
    }

    public class Projectile {
        public struct ProjectileLauncedData : IDarkRiftSerializable {
            public ushort clientId;
            public ushort itemId;
            public ProjectileLaunchParams launchParams;
            public ProjectileLauncedData(ushort clientId, ushort itemId, ProjectileLaunchParams launchParams) {
                this.clientId = clientId;
                this.itemId = itemId;
                this.launchParams = launchParams;
            }

            public void Deserialize(DeserializeEvent e) {
                clientId = e.Reader.ReadUInt16();
                itemId = e.Reader.ReadUInt16();

                launchParams.projectileType = (GameObjects.ProjectileEnums)e.Reader.ReadByte();
                launchParams.position = e.Reader.ReadVector3();
                launchParams.direction = e.Reader.ReadVector3();
                launchParams.muzzleVelocity = e.Reader.ReadSingle();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(clientId);
                e.Writer.Write(itemId);

                e.Writer.Write((byte)launchParams.projectileType);
                e.Writer.WriteVector3(launchParams.position);
                e.Writer.WriteVector3(launchParams.direction);
                e.Writer.Write(launchParams.muzzleVelocity);
            }
        }
    }

    public class Guns { // TODO: Rename this "Weapons"
        public struct WeaponUpdateData : IDarkRiftSerializable {
            public GunReloadStartedData[] reloadStartedDatas;
            public GunReloadFinishedData[] reloadFinishedDatas;

            public WeaponUpdateData(GunReloadStartedData[] equipablesSpawnedDatas, GunReloadFinishedData[] reloadFinishedDatas) {
                this.reloadStartedDatas = equipablesSpawnedDatas;
                this.reloadFinishedDatas = reloadFinishedDatas;
            }

            public void Deserialize(DeserializeEvent e) {
                reloadStartedDatas = e.Reader.ReadSerializables<GunReloadStartedData>();
                reloadFinishedDatas = e.Reader.ReadSerializables<GunReloadFinishedData>();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(reloadStartedDatas);
                e.Writer.Write(reloadFinishedDatas);
            }
        }

        public struct GunFiredData : IDarkRiftSerializable {
            public ushort clientId;
            public ushort gunId;
            public ushort projectileId;
            public ProjectileLaunchParams launchParams;

            public GunFiredData( ushort clientId, ushort gunId, ushort projectileId, ProjectileLaunchParams launchParams) {
                this.clientId = clientId;
                this.gunId = gunId;
                this.projectileId = projectileId;
                this.launchParams = launchParams;
            }
            public void Deserialize(DeserializeEvent e) {
                clientId = e.Reader.ReadUInt16();
                gunId = e.Reader.ReadUInt16();
                projectileId = e.Reader.ReadUInt16();
                launchParams = e.Reader.ReadProjectileLaunchParams();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(clientId);
                e.Writer.Write(gunId);
                e.Writer.Write(projectileId);
                e.Writer.WriteProjectileLaunchParams(launchParams);
            }
        }

        public struct GunFiredUnverifiedData : IDarkRiftSerializable {
            public ushort gunId;
            public ushort projectileTmpId;
            public ProjectileLaunchParams launchParams;

            public GunFiredUnverifiedData(ushort gunId, ushort projectileTmpId, ProjectileLaunchParams launchParams) {
                this.gunId = gunId;
                this.projectileTmpId = projectileTmpId;
                this.launchParams = launchParams;
            }
            public void Deserialize(DeserializeEvent e) {
                gunId = e.Reader.ReadUInt16();
                projectileTmpId = e.Reader.ReadUInt16();
                launchParams = e.Reader.ReadProjectileLaunchParams();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(gunId);
                e.Writer.Write(projectileTmpId);
                e.Writer.WriteProjectileLaunchParams(launchParams);
            }
        }

        public struct GunFiredVerifiedData : IDarkRiftSerializable {
            public ushort gunId;
            public ushort projectileTmpId;
            public ushort projectileId;
            public ProjectileLaunchParams launchParams;

            public GunFiredVerifiedData(ushort gunId, ushort projectileTmpId, ushort projectileId, ProjectileLaunchParams launchParams) {
                this.gunId = gunId;
                this.projectileTmpId = projectileTmpId;
                this.projectileId = projectileId;
                this.launchParams = launchParams;
            }

            public void Deserialize(DeserializeEvent e) {
                gunId = e.Reader.ReadUInt16();
                projectileTmpId = e.Reader.ReadUInt16();
                projectileId = e.Reader.ReadUInt16();
                launchParams = e.Reader.ReadProjectileLaunchParams();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(gunId);
                e.Writer.Write(projectileTmpId);
                e.Writer.Write(projectileId);
                e.Writer.WriteProjectileLaunchParams(launchParams);
            }
        }

        public struct GunReloadStartedData : IDarkRiftSerializable {
            public ushort gunId;

            public GunReloadStartedData(ushort gunId) {
                this.gunId = gunId;
            }

            public void Deserialize(DeserializeEvent e) {
                gunId = e.Reader.ReadUInt16();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(gunId);
            }
        }

        public struct GunReloadFinishedData : IDarkRiftSerializable {
            public ushort gunId;
            public ushort bulletCount;

            public GunReloadFinishedData(ushort gunId, ushort bulletCount) {
                this.gunId = gunId;
                this.bulletCount = bulletCount;
            }

            public void Deserialize(DeserializeEvent e) {
                gunId = e.Reader.ReadUInt16();
                bulletCount = e.Reader.ReadUInt16();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(gunId);
                e.Writer.Write(bulletCount);
            }
        }
    }

    public class HealthData {
        public struct HealthUpdateData : IDarkRiftSerializable {
            public HealthHpChangeData[] healthHpChangeDatas;
            public HealthStateChangeData[] healthStateChangeDatas;

            public HealthUpdateData(HealthHpChangeData[] healthHpChangeDatas, HealthStateChangeData[] healthStateChangeDatas) {
                this.healthHpChangeDatas = healthHpChangeDatas;
                this.healthStateChangeDatas = healthStateChangeDatas;
            }

            public void Deserialize(DeserializeEvent e) {
                healthHpChangeDatas = e.Reader.ReadSerializables<HealthHpChangeData>();
                healthStateChangeDatas = e.Reader.ReadSerializables<HealthStateChangeData>();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(healthHpChangeDatas);
                e.Writer.Write(healthStateChangeDatas);
            }
        }

        public struct HealthStateChangeData : IDarkRiftSerializable {
            public ushort clientId;
            public Health.State newState;

            public HealthStateChangeData( ushort clientId, Health.State newState) {
                this.clientId = clientId;
                this.newState =  newState;
            }
            public void Deserialize(DeserializeEvent e) {
                clientId = e.Reader.ReadUInt16();
                newState = (Health.State)e.Reader.ReadUInt16();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(clientId);
                e.Writer.Write((ushort)newState);
            }
        }

        public struct HealthHpChangeData : IDarkRiftSerializable {
            public ushort clientId;
            public float newHP;
            // TODO: Have damage/health dealer id

            public HealthHpChangeData( ushort clientId, float newHP) {
                this.clientId = clientId;
                this.newHP = newHP;
            }
            public void Deserialize(DeserializeEvent e) {
                clientId = e.Reader.ReadUInt16();
                newHP = e.Reader.ReadSingle();
            }

            public void Serialize(SerializeEvent e) {
                e.Writer.Write(clientId);
                e.Writer.Write(newHP);
            }
        }
    }
}
