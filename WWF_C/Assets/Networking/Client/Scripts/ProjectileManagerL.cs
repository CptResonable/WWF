using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Client;

[System.Serializable]
public class ProjectileManagerL {
    public Dictionary<ushort, Projectile> unverifiedProjectiles = new Dictionary<ushort, Projectile>();
    public Dictionary<ushort, Projectile> projectiles = new Dictionary<ushort, Projectile>();
    [SerializeField] private Transform tProjectileContainer;
    private ushort tmpProjectileIdTicker = 0; // Creates temporary ids for projectiles

    public void Initialize() {
    }
    
    /// <summary> Launch a projectile that is not yet verified by the server, returns a temporary id </summary>
    public ushort LaunchUnverifiedProjectile(ProjectileLaunchParams launchParams, ushort equipableId) {
        GameObject goProjectile = GameObjects.Instantiate(GameObjects.i.projectiles[launchParams.projectileType], tProjectileContainer);
        Projectile projectile = goProjectile.GetComponent<Projectile>();
        projectile.Initialize(launchParams, tmpProjectileIdTicker, equipableId, ClientManagerL.i.localClient.ID, false);
        unverifiedProjectiles.Add(tmpProjectileIdTicker, projectile);
        tmpProjectileIdTicker++;

        return projectile.projectileId;
    }

    /// <summary> Server has verified a projectile launch </summary>
    public void ProjectileLaunchVerified(DrDatas.Guns.GunFiredVerifiedData verifiedFireData) {
        Projectile projectile = unverifiedProjectiles[verifiedFireData.projectileTmpId];
        projectile.OnVerified(verifiedFireData.projectileId);

        // Move projectile to the verified projectile dictionary
        projectiles.Add(projectile.projectileId, projectile);
        unverifiedProjectiles.Remove(verifiedFireData.projectileTmpId);
    }

    /// <summary> Launch projectile fired by networkplayer </summary>
    public void ProjectileLaunched(DrDatas.Guns.GunFiredData firedData) {
        GameObject goProjectile = GameObjects.Instantiate(GameObjects.i.projectiles[firedData.launchParams.projectileType], tProjectileContainer);
        Projectile projectile = goProjectile.GetComponent<Projectile>();
        projectile.Initialize(firedData.launchParams, tmpProjectileIdTicker, firedData.gunId, firedData.clientId, true);
        tmpProjectileIdTicker++;
    }
}

