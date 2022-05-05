using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHudElement : MonoBehaviour {
    public enum ID { healthBar }
    public ID id;

    public virtual void Enable() {
        gameObject.SetActive(true);
    }

    public virtual void Disable() {
        gameObject.SetActive(false);
    }
}
