using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealthManagerL {
    public void Initialize() {
        GameManagerL.gameManagerLLoadedEvent += GameManagerL_gameManagerLLoadedEvent;
    }

    private void GameManagerL_gameManagerLLoadedEvent(GameManagerL gameManagerL) {
        ClientManagerL.i.gameManager.gameUpdateReceivedEvent += GameManager_gameUpdateReceivedEvent;
    }

    private void GameManager_gameUpdateReceivedEvent(DrDatas.Game.GameUpdateData gameUpdateData) {
        HpChangeUpdate(gameUpdateData.healthUpdateData.healthHpChangeDatas);
        HealthStateChangeUpdate(gameUpdateData.healthUpdateData.healthStateChangeDatas);
    }

    private void HpChangeUpdate(DrDatas.HealthData.HealthHpChangeData[] hpChangeDatas) {

        // Set HPs
        if (hpChangeDatas.Length > 0) {
            for (int i = 0; i < hpChangeDatas.Length; i++){
                ClientManagerL.i.playerManager.allPlayers[hpChangeDatas[i].clientId].character.health.SetHP(hpChangeDatas[i].newHP);
            }
        }
    }

    private void HealthStateChangeUpdate(DrDatas.HealthData.HealthStateChangeData[] stateChangeDatas) {

        // Set HPs
        if (stateChangeDatas.Length > 0) {
            Debug.Log("!!!!!!!!!!!     2");
            for (int i = 0; i < stateChangeDatas.Length; i++){
                Debug.Log("!!!!!!!!!!!      3");
                ClientManagerL.i.playerManager.allPlayers[stateChangeDatas[i].clientId].character.health.SetState(stateChangeDatas[i].newState);
            }
        }
    }
}
