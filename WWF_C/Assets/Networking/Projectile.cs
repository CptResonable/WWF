using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] float damage;
    [SerializeField] float impactForce;
    public ushort projectileId;
    public ushort equipableId; // Id of the item this is launched from
    public ushort clientId;
    public bool isVerified; // Has this projectile been authenticated by the server?

    public bool hasHitSomething;
    private RaycastHit hit;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private Vector3 lastPoint;
    private Coroutine autoDespawnCorutine;

    public delegate void ProjectileHitDelegate(Projectile projectile, RaycastHit hit);
    public static event ProjectileHitDelegate ProjectileHitEvent;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(ProjectileLaunchParams launchParams, ushort projectileId, ushort equipableId, ushort clientId, bool isVerified) {
        this.projectileId = projectileId;
        this.equipableId = equipableId;
        this.clientId = clientId;
        this.isVerified = isVerified;

        meshRenderer.enabled = true;
        hasHitSomething = false;
        transform.position = launchParams.position;
        rb.velocity = launchParams.direction * launchParams.muzzleVelocity;
        lastPoint = transform.position;

        autoDespawnCorutine = StartCoroutine(AutoDespawnCorutine());
    }

    private void FixedUpdate() {
        if (!hasHitSomething)
            HitDetection();
    }

    public void OnVerified(ushort newId) {
        projectileId = newId;
        isVerified = true;
    }

    private void HitDetection() {
        if (Physics.Linecast(lastPoint, transform.position, out hit)) {

            bool targetDestroyed = false;

            DamageReceiver damageReceiver;
            if (hit.transform.gameObject.TryGetComponent<DamageReceiver>(out damageReceiver)) {
                targetDestroyed = damageReceiver.ReceiveDamage(damage);
            }

            ImpactForceReceiver forceReceiver;
            if (hit.transform.gameObject.TryGetComponent<ImpactForceReceiver>(out forceReceiver)) {

                Vector3 force = impactForce * rb.velocity;
                if (targetDestroyed)
                    force *= 10;

                forceReceiver.ReceiveForce(force, hit.point);
            }

            ProjectileHitEvent?.Invoke(this, hit);

            rb.velocity = Vector3.zero;
            rb.position = hit.point;
            transform.position = hit.point;
            meshRenderer.enabled = false;
            hasHitSomething = true;

            StopCoroutine(autoDespawnCorutine);
            StartCoroutine(DespawnDelayCorutine());
        }
        lastPoint = transform.position;
    }

    private IEnumerator DespawnDelayCorutine() {
        yield return new WaitForSeconds(0.5f);
        EZ_Pooling.EZ_PoolManager.Despawn(transform);
    }

    private IEnumerator AutoDespawnCorutine() {
        yield return new WaitForSeconds(60);
        EZ_Pooling.EZ_PoolManager.Despawn(transform);
    }
}
