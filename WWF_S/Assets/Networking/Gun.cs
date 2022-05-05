using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Equipable {
    public Transform tSight;
    public Transform tMuzzle;
    public Transform tGrip;
    public Transform tChaimber;
    public Vector3 hipFirePosition;
    public Vector3 adsPosition;
    public GunSpecs specs;

    public int bulletsInMagCount;

    [SerializeField] private ProjectileLauncher projectileLauncher;
    private float noiseOffset = 0;
    private float recoilMultiplyerIndex = 0;
    private float timeSinceLastShot;
    private Coroutine reloadCorutine;

    public delegate void GunFiredDelegate(Gun gun, ProjectileLaunchParams lauchParams);
    public static event GunFiredDelegate gunFiredEvent;

    public delegate void ReloadDelegate(Gun gun);
    public static event ReloadDelegate reloadStartedEvent;
    public static event ReloadDelegate gunReloadFinishedEvent;

    public override void Initialize(DrDatas.EquipmentDatas.EquipableData equipableData) {
        base.Initialize(equipableData);

        bulletsInMagCount = specs.magSize;
    }

    public override void EquipL(CharacterLS character) {
        base.EquipL(character);
    }

    protected override void Character_fixedUpdateEvent() {
        base.Character_fixedUpdateEvent();

        timeSinceLastShot += Time.deltaTime;

        recoilMultiplyerIndex -= specs.recoilResetSpeed * timeSinceLastShot * timeSinceLastShot * Time.deltaTime;
        if (recoilMultiplyerIndex < 0)
            recoilMultiplyerIndex = 0;

        noiseOffset -= specs.recoilResetSpeed * 2 * timeSinceLastShot * timeSinceLastShot * Time.deltaTime;
        if (noiseOffset < 0)
            noiseOffset = 0;

    }

    public override void Attack() {
        base.Attack();

        if (bulletsInMagCount > 0) {
            ProjectileLaunchParams launchParams = projectileLauncher.Launch(tMuzzle.position, tMuzzle.forward, equipableData.equipableId);
            gunFiredEvent?.Invoke(this, launchParams);
            Recoil();
            bulletsInMagCount--;
        }
    }

    public override void StartReload() {
        base.StartReload();

        reloadStartedEvent?.Invoke(this);
        reloadCorutine = StartCoroutine(ReloadCorutine()); // Start reload      
    }

    private void Recoil() {
        // Calculate force vectors.
        float recoilMulitplier = specs.recoilMultiplierCurve.Evaluate(recoilMultiplyerIndex);

        Vector2 noiseTorque = Vector2.zero;
        noiseTorque.x = Mathf.PerlinNoise(specs.defaultNoiseOffset.x + noiseOffset, specs.defaultNoiseOffset.y + noiseOffset);
        noiseTorque.y = Mathf.PerlinNoise(specs.defaultNoiseOffset.x * 5 + noiseOffset, specs.defaultNoiseOffset.x * 5 + noiseOffset);

        noiseTorque = new Vector2(noiseTorque.x - 0.5f, noiseTorque.y - 0.5f);
        //noiseTorque *= 500;

        Vector3 noiseForce = new Vector3(noiseTorque.x, 0, noiseTorque.y);
        //Debug.Log(noiseTorque);

        // Apply recoil.
        rb.AddForceAtPosition(transform.TransformVector((specs.recoilForce + noiseForce) * recoilMulitplier * 0.75f), tChaimber.position);
        rb.AddRelativeTorque(new Vector3(specs.recoilTorque.x + noiseTorque.x, specs.recoilTorque.y, specs.recoilTorque.z + noiseTorque.y) * recoilMulitplier * 0.75f);

        Debug.Log("f: " + transform.TransformVector((specs.recoilForce + noiseForce) * recoilMulitplier * 0.75f));
        Debug.Log("t: " + new Vector3(specs.recoilTorque.x + noiseTorque.x, specs.recoilTorque.y, specs.recoilTorque.z + noiseTorque.y) * recoilMulitplier * 0.75f);

        //character.eyes.OnGunRecoil(specs.recoilAngleHead * recoilMulitplier);
        //// Apply recoil to head.
        //LPlayer lPlayer = player as LPlayer;
        //lPlayer.head.Recoil(specs.recoilAngleHead * (recoilMulitplier * 0.5f));

        // Recoil scaling stuff.
        noiseOffset += specs.recoilInceasePerBullet * 2f;
        recoilMultiplyerIndex += specs.recoilInceasePerBullet;
        if (recoilMultiplyerIndex > 1)
            recoilMultiplyerIndex = 1;

        timeSinceLastShot = 0;
    }

    public void FinishReload(int bulletsInMagCount) {
        Debug.Log("Reloaded!");
        this.bulletsInMagCount = bulletsInMagCount;
    }

    private IEnumerator ReloadCorutine() {
        yield return new WaitForSeconds(specs.reloadTime);
        gunReloadFinishedEvent?.Invoke(this);
    }
}