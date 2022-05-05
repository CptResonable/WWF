using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Client;

public class GameManagerL : MonoBehaviour {
    public delegate void GameUpdateReceivedDelegate(DrDatas.Game.GameUpdateData gameUpdateData);
    public event GameUpdateReceivedDelegate gameUpdateReceivedEvent;
    public delegate void GameManagerLLoadedDelegate(GameManagerL gameManagerL);
    public static event GameManagerLLoadedDelegate gameManagerLLoadedEvent;

    private void Awake() {
        if (ClientManagerL.i == null) {
            Destroy(gameObject);
        }
        else {
            gameManagerLLoadedEvent?.Invoke(this);
        }
    }

    // private void FixedUpdate() {
    //     Debug.Log(ClientManagerL.i.playerManager.allPlayers.Count);

    //     foreach (Player player in ClientManagerL.i.playerManager.allPlayers.Values){
            
    //     }
        
    //             Debug.Log("p: " + ClientManagerL.i.playerManager.allPlayers[equipmentUpdateData.equipablesSpawnedDatas[i].clientId]);
    //             Debug.Log("c: " + ClientManagerL.i.playerManager.allPlayers[equipmentUpdateData.equipablesSpawnedDatas[i].clientId].character);
    //             Debug.Log("e: " + ClientManagerL.i.playerManager.allPlayers[equipmentUpdateData.equipablesSpawnedDatas[i].clientId].character.equipment);
    // }

    #region Messages recieved
    public void MessageRecieved(Message message, MessageReceivedEventArgs e) {
        switch (message.Tag) {
            case Tags.game_updateGame:
                OnMsg_gameUpdate(message, e);
                break;
            default:
                break;
        }
    }

    private void OnMsg_gameUpdate(Message message, MessageReceivedEventArgs e) {
        DrDatas.Game.GameUpdateData updateData = message.Deserialize<DrDatas.Game.GameUpdateData>();
        gameUpdateReceivedEvent?.Invoke(updateData);
    }
    #endregion
}
