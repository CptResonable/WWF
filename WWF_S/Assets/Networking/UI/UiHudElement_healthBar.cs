using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UiHudElement_healthBar : UiHudElement {
    private Slider slider_healthBar;
    private Health health;

    public void Initialize(Health health) {
        this.health = health;
        slider_healthBar = GetComponent<Slider>();

        health.hpSetEvent += Health_hpSetEvent;
    }

    private void Health_hpSetEvent(float change, float newHP) {
        slider_healthBar.value = newHP / health.maxHP;
    }
}
