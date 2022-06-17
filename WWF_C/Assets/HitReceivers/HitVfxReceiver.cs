using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HitVfxReciever", menuName = "ScriptableObjects/HitVfxReciever")]
public class HitVfxReceiver : HitRecieverObject {
    [SerializeField] private Transform tTest;
    [SerializeField] private VisualEffects.VfxEnum vfxEnum;
    public override void Hit(Vector3 position, Quaternion rotation) {
        VfxManager.i.PlayEffect(vfxEnum, hitReciever.transform, position, rotation, true);
    }
}
