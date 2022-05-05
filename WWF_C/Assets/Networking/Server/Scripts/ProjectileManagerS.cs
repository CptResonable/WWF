using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Client;

[System.Serializable]
public class ProjectileManagerS {
    public Dictionary<ushort, Projectile> projectiles = new Dictionary<ushort, Projectile>();
    [SerializeField] private Transform tProjectileContainer;
    private ushort tmpProjectileIdTicker = 0; // Creates ids for projectiles

    public void Initialize() {
    }
    
    /// <summary> Launch a projectile </summary>
    public ushort LaunchProjectile(ProjectileLaunchParams launchParams, ushort equipableId, ushort clientId) {
        GameObject goProjectile = GameObjects.Instantiate(GameObjects.i.projectiles[launchParams.projectileType], tProjectileContainer);
        Projectile projectile = goProjectile.GetComponent<Projectile>();
        projectile.Initialize(launchParams, tmpProjectileIdTicker, equipableId, clientId, true);
        tmpProjectileIdTicker++;

        projectiles.Add(projectile.projectileId, projectile);
        return projectile.projectileId;
    }
}

