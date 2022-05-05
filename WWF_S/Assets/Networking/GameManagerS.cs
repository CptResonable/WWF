using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Server;

[System.Serializable]
public class GameManagerS : MonoBehaviour {
    private List<IDarkRiftSerializable> updates = new List<IDarkRiftSerializable>();
    //private DarkRiftSerializables.GameSerializables.GameUpdateData gameUpdateData = new DarkRiftSerializables.GameSerializables.GameUpdateData(false);
    private void Awake() {
        if (ServerManagerS.i == null) {
            Destroy(gameObject);
        }
        else {
            ServerManagerS.i.GameSceneLoaded(this);
        }
    }
    
    private void FixedUpdate() {
        DrDatas.Game.GameUpdateData gameUpdateData = new DrDatas.Game.GameUpdateData(
            ServerManagerS.i.playerManager.GatherBodyDatas(),
            ServerManagerS.i.equipmentManager.GetUpdates(),
            ServerManagerS.i.equipmentManager.weaponManager.GetUpdates(),
            ServerManagerS.i.playerManager.healthManager.GetUpdates()
            );

        // Send update to all players
        using (Message msgOut = Message.Create(Tags.game_updateGame, gameUpdateData)) {
            foreach (PlayerS player in ServerManagerS.i.playerManager.players.Values)
                player.client.SendMessage(msgOut, SendMode.Reliable);
        }
    }

    public void AddUpdateData(IDarkRiftSerializable data) {
        updates.Add(data);
    }
}
