using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZ_Pooling;

public class VfxManager : MonoBehaviour {
    public static VfxManager i;
    void Awake() {
        if (i != null) {
            Destroy(gameObject);
            return;
        }

        i = this;
        //DontDestroyOnLoad(this);
    }

    public void PlayEffect(VisualEffects.VfxEnum vfxEnum, Transform origin) {
        //GameObject vfxGameObject = Instantiate(VisualEffects.i.effects[vfxEnum], transform);
        GameObject vfxGameObject = EZ_PoolManager.Spawn(VisualEffects.i.effects[vfxEnum].transform, origin.position, Quaternion.identity).gameObject;

        vfxGameObject.transform.position = origin.position;
        vfxGameObject.transform.forward = origin.forward;

        vfxGameObject.GetComponent<VFX>().Initiate(origin);
    }

    public void PlayEffect(VisualEffects.VfxEnum vfxEnum, Transform origin, Vector3 position, Vector3 dirForward, bool attachedToOrigin) {
        //GameObject vfxGameObject = Instantiate(VisualEffects.i.effects[vfxEnum], transform);
        GameObject vfxGameObject = EZ_PoolManager.Spawn(VisualEffects.i.effects[vfxEnum].transform, position, Quaternion.identity).gameObject;

        if (attachedToOrigin)
            vfxGameObject.transform.parent = origin;

        //vfxGameObject.transform.position = position;
        vfxGameObject.transform.forward = dirForward;

        VFX vfx = vfxGameObject.GetComponent<VFX>();
        vfx.Initiate(origin);
    }

    public void PlayEffect(VisualEffects.VfxEnum vfxEnum, Transform origin, Vector3 position, Quaternion rotation, bool attachedToOrigin) {
        //GameObject vfxGameObject = Instantiate(VisualEffects.i.effects[vfxEnum], transform);
        GameObject vfxGameObject = EZ_PoolManager.Spawn(VisualEffects.i.effects[vfxEnum].transform, position, Quaternion.identity).gameObject;

        if (attachedToOrigin)
            vfxGameObject.transform.parent = origin;

        //vfxGameObject.transform.position = position;
        vfxGameObject.transform.rotation = rotation;

        VFX vfx = vfxGameObject.GetComponent<VFX>();
        vfx.Initiate(origin);
    }
}