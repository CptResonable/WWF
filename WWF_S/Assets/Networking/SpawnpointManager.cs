using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnpointManager : MonoBehaviour {
    public static SpawnpointManager i;

    [SerializeField] private List<Transform> tSpawnpoints;

    private void Awake() {
        i = this;
    }

    public bool GetSpawnpointData(ushort spawnpointId, out SpawnpointData spawnpointData) {

        // Make sure spawnpoint exist
        if (spawnpointId >= tSpawnpoints.Count) {
            Debug.LogError("Spawnpoint with the ID: " + spawnpointId + " does not exist!");
            spawnpointData = new SpawnpointData(transform);
            return false;
        }

        spawnpointData = new SpawnpointData(tSpawnpoints[spawnpointId]);
        return true;
    }

    public struct SpawnpointData {
        public Vector3 position;
        public Quaternion rotation;

        public SpawnpointData (Transform tSpawnpoint) {
            position = tSpawnpoint.position;
            rotation = tSpawnpoint.rotation;
        }
    }
}
