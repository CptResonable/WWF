using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiWindow_spawnSelect : UiWindow {
    public delegate void SpawnpointBtnDelegate(int spawnpointId);
    public static event SpawnpointBtnDelegate OnClickEvent_btnSpawn;

    public void OnBtnSpawnpointSelect(int spawnpointId) {
        OnClickEvent_btnSpawn?.Invoke(spawnpointId);
        UiManager.i.CloseActiveWindow();
    }
}
