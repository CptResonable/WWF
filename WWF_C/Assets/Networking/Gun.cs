using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Equipable {
    public Transform tSight_back;
    public Transform tSight_front;
    public Transform tMuzzle;
    public Transform tGrip;
    public Transform tChaimber;
    public GunSpecs specs;

    public int bulletsInMagCount;
    [HideInInspector] public bool isReloading = false;
    public float reloadProgress;

    [SerializeField] private ProjectileLauncher projectileLauncher;
    private float noiseOffset = 0;
    private float recoilMultiplyerIndex = 0;
    private float timeSinceLastShot;
    private bool fireOnCooldown = false;
    private bool recoilIsReseting = true;
    private Coroutine reloadCorutine;

    public delegate void GunFiredDelegate(Gun gun, ProjectileLaunchParams lauchParams);
    public static event GunFiredDelegate GunFiredEvent;
    public event Delegates.EmptyDelegate gunFiredEvent;

    public delegate void ReloadDelegate(Gun gun);
    public static event ReloadDelegate ReloadStartedEvent;
    public static event ReloadDelegate ReloadFinishedEvent;
    public event Delegates.EmptyDelegate reloadFinishedEvent;

    private Rigidbody rbGrip;

    public override void Initialize(DrDatas.EquipmentDatas.EquipableData equipableData) {
        base.Initialize(equipableData);

        rbGrip = tGrip.GetComponent<Rigidbody>();
        bulletsInMagCount = specs.magSize;
    }

    protected override void Character_fixedUpdateEvent() {
        base.Character_fixedUpdateEvent();

        timeSinceLastShot += Time.deltaTime;

        if (recoilIsReseting) {
            recoilMultiplyerIndex -= specs.recoilResetSpeed * timeSinceLastShot * timeSinceLastShot * Time.deltaTime;
            if (recoilMultiplyerIndex < 0)
                recoilMultiplyerIndex = 0;
        }

        if (isReloading) {
            reloadProgress += Time.deltaTime / specs.reloadTime;
            if (reloadProgress > 1)
                reloadProgress = 1;
        }
        //noiseOffset -= specs.recoilResetSpeed * 2 * timeSinceLastShot * timeSinceLastShot * Time.deltaTime;
        //if (noiseOffset < 0)
        //    noiseOffset = 0;

    }

    public override void EquipL(CharacterLS character) {
        base.EquipL(character);
        character.updateEvent += Character_updateEvent;
        character.input.reload.keyDownEvent += Reload_keyDownEvent; ;
    }

    public override void UnequipL() {
        characterLS.updateEvent -= Character_updateEvent;
        characterLS.input.reload.keyDownEvent -= Reload_keyDownEvent; ;
        base.UnequipL();
    }

    public override void EquipN(CharacterN character) {
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;
        rbGrip.isKinematic = true;
        rbGrip.interpolation = RigidbodyInterpolation.None;

        base.EquipN(character);
    }

    public override void UnequipN() {
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rbGrip.isKinematic = false;
        rbGrip.interpolation = RigidbodyInterpolation.Interpolate;

        base.UnequipN();
    }

    private void Character_updateEvent() {
        if (characterLS.input.attack_1.isTriggered) {
            if (!fireOnCooldown && bulletsInMagCount > 0)
                Fire();
        }

        if (isReloading) {
            reloadProgress += Time.deltaTime / specs.reloadTime;
            if (reloadProgress > 1)
                reloadProgress = 1;
        }
    }

    private void Reload_keyDownEvent() {
        StartReload();
    }

    protected override void Attack_1_keyDownEvent() {
        base.Attack_1_keyDownEvent();

        //if (specs.fireMode == GunSpecs.FireModes.semiAuto)
        //    Fire();
        //else if (specs.fireMode == GunSpecs.FireModes.fullAuto)
        //    StartCoroutine(AutoFireCorutine());
    }

    public override void Attack() {
        //base.Attack();

        //Debug.Log("FIRE!");

        //if (bulletsInMagCount > 0) {
        //    ProjectileLaunchParams launchParams = projectileLauncher.Launch(tMuzzle.position, tMuzzle.forward, equipableData.equipableId);
        //    gunFiredEvent?.Invoke(this, launchParams);
        //    Recoil();
        //    bulletsInMagCount--;
        //}
    }

    private void Fire() {
        Debug.Log("FIRE!");
        ProjectileLaunchParams launchParams = projectileLauncher.Launch(specs.muzzleVelocity, tMuzzle.position, tMuzzle.forward, equipableData.equipableId);
        GunFiredEvent?.Invoke(this, launchParams);
        gunFiredEvent?.Invoke();
        Recoil();
        //VfxManager.i.PlayEffect(VisualEffects.VfxEnum.muzzleFlash, tMuzzle);
        VfxManager.i.PlayEffect(VisualEffects.VfxEnum.muzzleFlash, tMuzzle, tMuzzle.position, tMuzzle.forward, true);
        bulletsInMagCount--;
        StartCoroutine(FireCooldownCorutine());

        if (bulletsInMagCount > 0 && !fireOnCooldown) {

        }
    }

    private void Recoil() {

        // Calculate force vectors.
        Vector2 recoil = Vector2.zero;
        noiseOffset = recoilMultiplyerIndex * specs.noiseScale;
        recoil.x = Mathf.PerlinNoise(specs.defaultNoiseOffset.x + noiseOffset, specs.defaultNoiseOffset.y + noiseOffset) * specs.verticalRecoilCurve.Evaluate(recoilMultiplyerIndex);
        recoil.y = (Mathf.PerlinNoise(specs.defaultNoiseOffset.x * 5 + noiseOffset, specs.defaultNoiseOffset.x * 5 + noiseOffset) - 0.4f) * specs.horizontalRecoilCurve.Evaluate(recoilMultiplyerIndex);

        // Apply force.
        Vector3 recoilForce = specs.baseRecoil;
        rb.AddForceAtPosition(transform.TransformVector(recoilForce), tChaimber.position);

        // Apply torque
        Vector3 recoilTorque = new Vector3(recoil.x * specs.verticalRecoilAmount, 0, recoil.y * specs.horizontalRecoilAmount);
        rb.AddRelativeTorque(recoilTorque);
        if (characterLS.GetPlayer().playerType == Player.PlayerType.local)
            StartCoroutine(HeadRecoilCorutine(0.1f, new Vector2(-recoil.x - specs.baseHeadRecoil, recoil.y) * specs.headRecoilScale));

        Debug.Log("Noise offset: " + recoilMultiplyerIndex);

        // Recoil scaling stuff.
        recoilMultiplyerIndex += specs.recoilInceasePerBullet;
        if (recoilMultiplyerIndex > 1)
            recoilMultiplyerIndex = 1;

        timeSinceLastShot = 0;

        StartCoroutine(RecoilResetDelayCorutine());
    }

    public override void StartReload() {
        base.StartReload();

        isReloading = true;
        reloadProgress = 0;
        ReloadStartedEvent?.Invoke(this);
        characterLS.torso.armL.ReloadStarted(specs.reloadTime);
        reloadCorutine = StartCoroutine(ReloadCorutine()); // Start reload      
    }

    public void FinishReload(int bulletsInMagCount) {
        isReloading = false;

        this.bulletsInMagCount = bulletsInMagCount;
        reloadFinishedEvent?.Invoke();
    }

    private IEnumerator FireCooldownCorutine() {
        fireOnCooldown = true;
        yield return new WaitForSeconds(specs.minFireInterval);
        fireOnCooldown = false;
    }

    private IEnumerator HeadRecoilCorutine(float time, Vector2 amount) {
        float t = 0;
        while (t < 1) {

            float newT = Mathf.Lerp(t, 1, Time.deltaTime * specs.headRecoilSpeed);
            float d = newT - t;
            //float d = Time.deltaTime / time;
            t += d;
            characterLS.input.headPitchYaw += amount * d;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    private IEnumerator RecoilResetDelayCorutine() {
        recoilIsReseting = false;
        yield return new WaitForSeconds(specs.minFireInterval);
        recoilIsReseting = true;
    }

    private IEnumerator AutoFireCorutine() {
        bool interupted = false;
        while (!interupted && characterLS.input.attack_1.isTriggered) {
            if (bulletsInMagCount <= 0) {
                interupted = true;
                continue;
            }
            Fire();
            yield return new WaitForSeconds(specs.minFireInterval);
        }
    }

    //private IEnumerator AutoFireCorutine() {
    //    bool interupted = false;
    //    while (!interupted && characterLS.input.attack_1.isTriggered) {
    //        if (bulletsInMagCount <= 0) {
    //            interupted = true;
    //            continue;
    //        }
    //        Fire();
    //        yield return new WaitForSeconds(specs.minFireInterval);
    //    }
    //}

    private IEnumerator ReloadCorutine() {
        yield return new WaitForSeconds(specs.reloadTime);
        ReloadFinishedEvent?.Invoke(this);
    }
}
