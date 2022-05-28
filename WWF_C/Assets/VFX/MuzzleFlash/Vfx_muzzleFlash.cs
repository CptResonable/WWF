using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vfx_muzzleFlash : VFX {
    private MeshRenderer meshRenderer;
    public override void Initiate(Transform origin) {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        base.Initiate(origin);
    }
}
