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

                _i.gunshotSfxs.Add(GunshotSfxEnums.m1911, _i.gs_m1911);
                _i.gunshotSfxs.Add(GunshotSfxEnums.dyiAk, _i.gs_dyiAk);
            }
            return _i;
        }
    }

    public SFX footstep_dirt;
    public SFX impact_flesh;
    public SFX impact_dirt;

    // Gun shots
    [Header("Gun shots")]
    public SFX gs_m1911;
    public SFX gs_dyiAk;
    public enum GunshotSfxEnums { m1911, dyiAk }
    public Dictionary<GunshotSfxEnums, SFX> gunshotSfxs = new Dictionary<GunshotSfxEnums, SFX>();
}


[Serializable]
public class SFX {
    public AudioClip[] variations;
    [Range(0, 1)] public float volume;
    [Range(0, 2)] public float pitchMod;
    [Range(0, 0.5f)] public float pitchVariance;

    public AudioClip GetRandomVariation() {
        return variations[UnityEngine.Random.Range(0, variations.Length - 1)];
    }
}
