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

    private Vector3 lastPoint;
    public void Initialize(ProjectileLaunchParams launchParams, ushort projectileId, ushort equipableId, ushort clientId, bool isVerified) {
        this.projectileId = projectileId;
        this.equipableId = equipableId;
        this.clientId = clientId;
        this.isVerified = isVerified;

        rb = GetComponent<Rigidbody>();

        transform.position = launchParams.position;
        rb.velocity = launchParams.direction * launchParams.muzzleVelocity;
        lastPoint = transform.position;
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
            DamageReceiver damageReceiver;
            if (hit.transform.gameObject.TryGetComponent<DamageReceiver>(out damageReceiver)) {
                damageReceiver.ReceiveDamage(damage);
            }

            ImpactForceReceiver forceReceiver;
            if (hit.transform.gameObject.TryGetComponent<ImpactForceReceiver>(out forceReceiver)) {
                forceReceiver.ReceiveForce(impactForce * rb.velocity, hit.point);
            }

            rb.velocity = Vector3.zero;
            rb.position = hit.point;
            Destroy(rb);
            transform.position = hit.point;
            //transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            hasHitSomething = true;
        }
        lastPoint = transform.position;
    }
}
