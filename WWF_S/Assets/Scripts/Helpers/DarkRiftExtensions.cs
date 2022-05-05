using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DarkRiftExtensions {
    public static void WriteVector3(this DarkRift.DarkRiftWriter writer, Vector3 vector3) {
        writer.Write(vector3.x);
        writer.Write(vector3.y);
        writer.Write(vector3.z);
    }

    public static Vector3 ReadVector3(this DarkRift.DarkRiftReader reader) {
        Vector3 vector3;
        vector3.x = reader.ReadSingle();
        vector3.y = reader.ReadSingle();
        vector3.z = reader.ReadSingle();
        return vector3;
    }

    public static void WriteVector2(this DarkRift.DarkRiftWriter writer, Vector2 vector2) {
        writer.Write(vector2.x);
        writer.Write(vector2.y);
    }

    public static Vector2 ReadVector2(this DarkRift.DarkRiftReader reader) {
        Vector2 vector2;
        vector2.x = reader.ReadSingle();
        vector2.y = reader.ReadSingle();
        return vector2;
    }

    public static void WriteQuaternion(this DarkRift.DarkRiftWriter writer, Quaternion quaternion) {
        writer.Write(quaternion.x);
        writer.Write(quaternion.y);
        writer.Write(quaternion.z);
        writer.Write(quaternion.w);
    }

    public static Quaternion ReadQuaternion(this DarkRift.DarkRiftReader reader) {
        Quaternion quaternion;
        quaternion.x = reader.ReadSingle();
        quaternion.y = reader.ReadSingle();
        quaternion.z = reader.ReadSingle();
        quaternion.w = reader.ReadSingle();
        return quaternion;
    }

    public static void WriteProjectileLaunchParams(this DarkRift.DarkRiftWriter writer, ProjectileLaunchParams launchParams) {
        writer.Write((byte)launchParams.projectileType);
        writer.WriteVector3(launchParams.position);
        writer.WriteVector3(launchParams.direction);
        writer.Write(launchParams.muzzleVelocity);
    }

    public static ProjectileLaunchParams ReadProjectileLaunchParams(this DarkRift.DarkRiftReader reader) {
        ProjectileLaunchParams launchParams;
        launchParams.projectileType = (GameObjects.ProjectileEnums)reader.ReadByte();
        launchParams.position = reader.ReadVector3();
        launchParams.direction = reader.ReadVector3();
        launchParams.muzzleVelocity = reader.ReadSingle();
        return launchParams;
    }
}
