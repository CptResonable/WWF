using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VisualEffects", menuName = "ScriptableObjects/VisualEffects")]
public class VisualEffects : ScriptableObject {
    public enum VfxEnum { muzzleFlash, impact_dirt, bloodSplatter, grenadeExplosion };

    [SerializeField] private Combo[] _effects;
    public readonly Dictionary<VfxEnum, GameObject> effects = new Dictionary<VfxEnum, GameObject>();

    private static VisualEffects _i;
    public static VisualEffects i {
        get {
            if (_i == null) {
                _i = Resources.Load("VisualEffects") as VisualEffects;

                // Compile combo list into a dictionary.
                for (int j = 0; j < i._effects.Length; j++)
                    i.effects.Add(i._effects[j].vfxEnum, i._effects[j].vfx);
            }
            return _i;
        }
    }

    [Serializable]
    public struct Combo {
        public VfxEnum vfxEnum;
        public GameObject vfx;
    }
}
