using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealthManagerS {
    private List<DrDatas.HealthData.HealthHpChangeData> healthChangeDataList = new List<DrDatas.HealthData.HealthHpChangeData>();
    private List<DrDatas.HealthData.HealthStateChangeData> healthStateChangeDataList = new List<DrDatas.HealthData.HealthStateChangeData>();

    public void Initialize() {
        Health.hpSetStaticEvent += Health_hpChangedEvent;
        Health.stateChangedEvent += Health_stateChangedEvent;
    }

    #region Events
    private void Health_hpChangedEvent(Character character, float change, float newHP) {
        healthChangeDataList.Add(new DrDatas.HealthData.HealthHpChangeData(character.GetClientID(), newHP));
    }

    private void Health_stateChangedEvent(Character character, Health.State state) {
        healthStateChangeDataList.Add(new DrDatas.HealthData.HealthStateChangeData(character.GetClientID(), state));
    }
    #endregion

    /// <summary> Get all health update datas that should be synced to clients </summary>
    public DrDatas.HealthData.HealthUpdateData GetUpdates() {
        DrDatas.HealthData.HealthUpdateData updateData = new DrDatas.HealthData.HealthUpdateData(healthChangeDataList.ToArray(), healthStateChangeDataList.ToArray());
        healthChangeDataList.Clear();
        healthStateChangeDataList.Clear();
        return updateData;
    }   
}
