using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HitReceiverCollection", menuName = "ScriptableObjects/HitReceiverCollection")]
public class HitReceiverCollection : ScriptableObject {
    public HitRecieverObject[] hitReceivers;

    public void Initialize(HitRecieverComponent hitReciever) {
        for (int i = 0; i < hitReceivers.Length; i++) {
            hitReceivers[i].Initialize(hitReciever);
        }
    }

    public void Hit(Vector3 position, Quaternion rotation) {
        for (int i = 0; i < hitReceivers.Length; i++) {
            hitReceivers[i].Hit(position, rotation);
        }
    }
}