using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameObjects", menuName = "ScriptableObjects/GameObjects", order = 2)]
public class GameObjects : ScriptableObject {
    public static GameObjects _i;

    // Players
    public GameObject playerL;
    public GameObject playerS;
    public GameObject playerN;

    // Characters
    public GameObject characterL;
    public GameObject characterS;  
    public GameObject characterN;

    // Sound
    public GameObject SFX_base;

    // Equipables
    [Header("Equipables")]
    public GameObject wep_P25;
    public GameObject wep_AK;
    public enum EquipablesEnums { wep_P25, wep_AK }
    public Dictionary<EquipablesEnums, GameObject> equipables = new Dictionary<EquipablesEnums, GameObject>();

    // Projectiles
    [Header("Equipables")]
    public GameObject bullet_45;
    public enum ProjectileEnums { bullet_45 }
    public Dictionary<ProjectileEnums, GameObject> projectiles = new Dictionary<ProjectileEnums, GameObject>();

    public static GameObjects i {
        get {
            if (_i == null) {
                _i = Resources.Load("GameObjects") as GameObjects;
                i.equipables.Add(EquipablesEnums.wep_P25, i.wep_P25);
                i.equipables.Add(EquipablesEnums.wep_AK, i.wep_AK);

                i.projectiles.Add(ProjectileEnums.bullet_45, i.bullet_45);
            }
            return _i;
        }
    }
}