using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Sounds", menuName = "ScriptableObjects/Sounds", order = 3)]
public class Sounds : ScriptableObject {

    static Sounds _i;
    public static Sounds i {
        get {
            if (_i == null) {
                _i = Resources.Load("Sounds") as Sounds;
            }
            return _i;
        }
    }

    public SFX gs_m1911;
    public SFX gs_dyiAk;
    public SFX footstep_dirt;
    public SFX impact_flesh;
}


[Serializable]
public class SFX {
    public AudioClip[] variations;
    [Range(0, 1)] public float volume;

    public AudioClip GetRandomVariation() {
        return variations[UnityEngine.Random.Range(0, variations.Length - 1)];
    }
}
