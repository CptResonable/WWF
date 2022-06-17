using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEffectManager : MonoBehaviour {
    private void Awake() {
        Projectile.ProjectileHitEvent += Projectile_ProjectileHitEvent;
    }

    private void Projectile_ProjectileHitEvent(Projectile projectile, RaycastHit hit) {

        SurfaceData surfaceData;    
        if (hit.transform.gameObject.TryGetComponent<SurfaceData>(out surfaceData)) {
            switch (surfaceData.surfaceTag) {
                case SurfaceTag.unspecified:
                    break;
                case SurfaceTag.flesh:
                    VfxManager.i.PlayEffect(VisualEffects.VfxEnum.bloodSplatter, hit.transform, hit.point, hit.normal, true);
                    break;
                case SurfaceTag.metal:
                    break;
                case SurfaceTag.wood:
                    break;
                case SurfaceTag.dirt:
                    break;
                default:
                    break;
            }
        }
    }
}
