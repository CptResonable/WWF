using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Client;

[System.Serializable]
public class WeaponManagerL {
    private EquipmentManagerL equipmentManager;

    public void Initialize(EquipmentManagerL equipmentManager) {
        this.equipmentManager = equipmentManager;

        GameManagerL.gameManagerLLoadedEvent += GameManagerL_gameManagerLLoadedEvent;
        Gun.GunFiredEvent += Gun_gunFiredEvent;
    }

    private void GameManagerL_gameManagerLLoadedEvent(GameManagerL gameManagerL) {
        ClientManagerL.i.gameManager.gameUpdateReceivedEvent += GameManager_gameUpdateReceivedEvent;
    }
    
    private void GameManager_gameUpdateReceivedEvent(DrDatas.Game.GameUpdateData gameUpdateData) {
        WeaponReloadStartedUpdate(gameUpdateData.weaponUpdateData.reloadStartedDatas);
        WeaponReloadFinishedUpdate(gameUpdateData.weaponUpdateData.reloadFinishedDatas);
    }

    #region Receive messages
    public void MessageRecieved(Message message, MessageReceivedEventArgs e) {
        switch (message.Tag) {
            case Tags.weapons_gunFiredVerified:
                OnMsg_gunFiredVerified(message, e);
                break;
            case Tags.weapons_gunFired:
                OnMsg_gunFired(message, e);
                break;
            default:
                break;
        }
    }

    // Local player gun fired verified by server
    private void OnMsg_gunFiredVerified(Message message, MessageReceivedEventArgs e) {
        DrDatas.Guns.GunFiredVerifiedData verifiedFiredData = message.Deserialize<DrDatas.Guns.GunFiredVerifiedData>();
        ClientManagerL.i.projectileManager.ProjectileLaunchVerified(verifiedFiredData);
    }

    // Network player has fired a gun
    private void OnMsg_gunFired(Message message, MessageReceivedEventArgs e) {
        DrDatas.Guns.GunFiredData firedData = message.Deserialize<DrDatas.Guns.GunFiredData>();
        ClientManagerL.i.projectileManager.ProjectileLaunched(firedData);

        Debug.Log("Network gun fired");
    }

    #endregion

    private void WeaponReloadStartedUpdate(DrDatas.Guns.GunReloadStartedData[] reloadStartedDatas) {
        for (int i = 0; i < reloadStartedDatas.Length; i++) {
            Gun gun = (Gun)equipmentManager.equipables[reloadStartedDatas[i].gunId];
            gun.StartReload();
        }
    }

    private void WeaponReloadFinishedUpdate(DrDatas.Guns.GunReloadFinishedData[] reloadFinishedDatas) {
        for (int i = 0; i < reloadFinishedDatas.Length; i++) {
            Gun gun = equipmentManager.equipables[reloadFinishedDatas[i].gunId] as Gun;
            gun.FinishReload(reloadFinishedDatas[i].bulletCount);
        }
    }
    
    
    #region events
    private void Gun_gunFiredEvent(Gun gun, ProjectileLaunchParams launchParams) {
        ushort tmpProjectileId = ClientManagerL.i.projectileManager.LaunchUnverifiedProjectile(launchParams, gun.equipableData.equipableId); // Launch projectile

        // Create fireed data and send message
        DrDatas.Guns.GunFiredUnverifiedData unverifiedFiredData = new DrDatas.Guns.GunFiredUnverifiedData(gun.equipableData.equipableId, tmpProjectileId, launchParams);
        ClientManagerL.i.localClient.SendMessage(Message.Create(Tags.weapons_gunFiredUnverified, unverifiedFiredData), SendMode.Reliable);

        Debug.Log("GUN FIRED!");
    }
    #endregion
}
