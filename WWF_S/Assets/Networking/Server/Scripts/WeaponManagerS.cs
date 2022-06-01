using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Server;

[System.Serializable]
public class WeaponManagerS {
    private EquipmentManagerS equipmentManager;
    private List<DrDatas.Guns.GunReloadStartedData> gunReloadStartedDataList = new List<DrDatas.Guns.GunReloadStartedData>();
    private List<DrDatas.Guns.GunReloadFinishedData> gunReloadFinishedDataList = new List<DrDatas.Guns.GunReloadFinishedData>();

    public void Initialize(EquipmentManagerS equipmentManager) {
        this.equipmentManager = equipmentManager;

        Gun.ReloadStartedEvent += Gun_reloadStartedEvent;
        Gun.ReloadFinishedEvent += Gun_reloadFinishedEvent;
    }

    /// <summary> Get all weapon update datas that should be synced to clients </summary>
    public DrDatas.Guns.WeaponUpdateData GetUpdates() {
        DrDatas.Guns.WeaponUpdateData updateData = new DrDatas.Guns.WeaponUpdateData(gunReloadStartedDataList.ToArray(), gunReloadFinishedDataList.ToArray());
        gunReloadStartedDataList.Clear();
        gunReloadFinishedDataList.Clear();
        return updateData;
    }

    #region Receive messages
    public void MessageRecieved(Message message, MessageReceivedEventArgs e) {
        switch (message.Tag) {
            case Tags.weapons_gunFiredUnverified:   
                weapons_gunFiredUnverified(message, e);
                break;
            default:
                break;
        }
    }

    private void weapons_gunFiredUnverified(Message message, MessageReceivedEventArgs e) {
        DrDatas.Guns.GunFiredUnverifiedData unverifiedFiredData = message.Deserialize<DrDatas.Guns.GunFiredUnverifiedData>();
        ushort projectileId = ServerManagerS.i.projectileManager.LaunchProjectile(unverifiedFiredData.launchParams, unverifiedFiredData.gunId, e.Client.ID); // Launch projectile

        // Send verification to shooter
        DrDatas.Guns.GunFiredVerifiedData verifiedFiredData = new DrDatas.Guns.GunFiredVerifiedData(unverifiedFiredData.gunId, unverifiedFiredData.projectileTmpId, projectileId, unverifiedFiredData.launchParams);
        e.Client.SendMessage(Message.Create(Tags.weapons_gunFiredVerified, verifiedFiredData), SendMode.Reliable);

        // Send fired data to other players
        DrDatas.Guns.GunFiredData gunFiredData = new DrDatas.Guns.GunFiredData(e.Client.ID, unverifiedFiredData.gunId, projectileId, unverifiedFiredData.launchParams);
        using (Message msgOut_gunFired = Message.Create(Tags.weapons_gunFired, gunFiredData)) {
            foreach(PlayerS player in ServerManagerS.i.playerManager.players.Values)
                if (player.client.ID != e.Client.ID)
                    player.client.SendMessage(msgOut_gunFired, SendMode.Reliable);
        }
    }

    #endregion
    
    #region events
    private void Gun_reloadStartedEvent(Gun gun) {

        // TODO: make sure its allowed
        DrDatas.Guns.GunReloadStartedData reloadStartedData = new DrDatas.Guns.GunReloadStartedData(gun.equipableData.equipableId);
        gunReloadStartedDataList.Add(reloadStartedData);
    }
    
    private void Gun_reloadFinishedEvent(Gun gun) {
         // TODO: Make sure bullets loaded are not more than the avalible

        DrDatas.Guns.GunReloadFinishedData reloadStartedData = new DrDatas.Guns.GunReloadFinishedData(gun.equipableData.equipableId, (ushort)gun.specs.magSize);
        gunReloadFinishedDataList.Add(reloadStartedData);
    }
    #endregion
}
