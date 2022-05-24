using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunSpec", menuName = "ScriptableObjects/GunSpec")]
[Serializable][SerializeField]
public class GunSpecs : ScriptableObject {
    enum AmmoTypes { bullet_45ACP }
    public enum FireModes { semiAuto, fullAuto }

    [SerializeField] AmmoTypes _ammoType;
    Type ammoType;
    public FireModes fireMode;
    public float muzzleVelocity;
    public float rpm;
    [HideInInspector] public float minFireInterval { private set; get; }
    public int magSize;
    public float reloadTime;
    public bool hasSlideStop;

    public Vector2 defaultNoiseOffset;
    public float recoilInceasePerBullet;
    public float recoilResetSpeed;

    public AnimationCurve verticalRecoilCurve;
    public AnimationCurve horizontalRecoilCurve;

    public float horizontalRecoilAmount;
    public float verticalRecoilAmount;

    public float noiseScale;
    public float forceScale;
    public float torqueScale;

    public Vector3 baseRecoil;

    public float baseHeadRecoil;
    public float headRecoilScale;
    public float headRecoilSpeed;

    //public Vector3 hipFirePosition;
    //public Vector3 adsPosition;

    private void OnEnable() {
        minFireInterval = 1 / (rpm / 60);
    }
}