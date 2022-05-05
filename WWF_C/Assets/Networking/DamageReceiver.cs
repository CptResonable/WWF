using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour {
    public float damageMultiplier = 1;
    public delegate void DamageReceivedDelegate(float damage);
    public event DamageReceivedDelegate damageReceivedEvent;

    public void ReceiveDamage(float damage) {
        damage *= damageMultiplier;
        damageReceivedEvent?.Invoke(damage);
    }
}
