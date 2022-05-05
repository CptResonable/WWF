using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileLauncher {
    public GameObjects.ProjectileEnums projectileType;
    public float muzzleVelocity;
    public delegate void ProjectileLaunchedDelegate(ProjectileLaunchParams launchParams, ushort launchedFromId);
    public static event ProjectileLaunchedDelegate projectileLaunchedEvent;
    
    public ProjectileLaunchParams Launch(Vector3 position, Vector3 direction, ushort launchedFromId) {
        ProjectileLaunchParams launchParams = new ProjectileLaunchParams(projectileType, muzzleVelocity, position, direction);
        projectileLaunchedEvent?.Invoke(launchParams, launchedFromId);
        return launchParams;
    }
}

public struct ProjectileLaunchParams {
    public ProjectileLaunchParams(GameObjects.ProjectileEnums projectileType, float muzzleVelocity, Vector3 position, Vector3 direction) {
        this.projectileType = projectileType;
        this.muzzleVelocity = muzzleVelocity;
        this.position = position;
        this.direction = direction;
    }
    public GameObjects.ProjectileEnums projectileType;
    public Vector3 position;
    public Vector3 direction;
    public float muzzleVelocity;
    
}
