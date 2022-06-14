using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour {
    public float damageMultiplier = 1;
    public delegate void DamageReceivedDelegate(float damage);
    public event DamageReceivedDelegate damageReceivedEvent;

    public Health health;

    /// <summary> Returns true if resulting hp is below 0 </summary>
    public bool ReceiveDamage(float damage) {
        damage *= damageMultiplier;
        damageReceivedEvent?.Invoke(damage);

        if (health != null) {

            if (health.HP < 0)
                return true;
            else
                return false;
        }
        else {
            return false;
        }
    }
}
