using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private float scale;
    [SerializeField] private float offset;
    public float b;

    private void Update() {
        //GetComponent<ConfigurableJoint>().anchor = new Vector3(0, b, 0);
        // float f = (Mathf.Sin(Time.time * speed) + 1) * scale;
        // GetComponent<ConfigurableJoint>().anchor = new Vector3(0, offset + f, 0);
        // Debug.Log(f);
    }
}
