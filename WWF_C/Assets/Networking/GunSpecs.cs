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

    public Vector3 recoilForce;
    public Vector3 recoilTorque;
    public Vector2 recoilAngleHead;

    public AnimationCurve recoilMultiplierCurve;
    public Vector2 defaultNoiseOffset;
    public float recoilInceasePerBullet;
    public float recoilResetSpeed;

    private void OnEnable() {
        minFireInterval = 1 / (rpm / 60);
    }
}