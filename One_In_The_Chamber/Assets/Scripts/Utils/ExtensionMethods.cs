using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods {

    public static float GetNormalizedAngle(this float angle) {
        while (angle < 0f) {
            angle += 360f;
        }

        while (360f < angle) {
            angle -= 360f;
        }

        return angle;
    }

    public static float GetAngleToPosition(this Vector2 fromTrans, Vector2 toTrans) {

        float cos0 = fromTrans.DotProduct(toTrans) / (fromTrans.magnitude * toTrans.magnitude);

        return Mathf.Acos(cos0);
    }

    public static float DotProduct(this Vector2 firstV, Vector2 secondV) {
        float firstDot = firstV.x * secondV.x;
        float secondDot = firstV.y * secondV.y;

        return firstDot + secondDot;
    }

    public static Vector3 DirectionToVector(this Vector3 from, Vector3 to) {
        return (to - from);
    }

    public static SoundType GetCorrespondingSoundType(this GunType gunType) {
        switch (gunType) {
            case GunType.PISTOL:
                return SoundType.SHOOT_PISTOL;
            case GunType.HEAVY_PISTOL:
                return SoundType.SHOOT_HEAVYPISTOL;
            case GunType.RIFLE_SEMIAUTO:
                return SoundType.SHOOT_SEMIAUTO;
            case GunType.ROCKET_LAUNCHER:
                return SoundType.SHOOT_RPG;
            case GunType.SHOTGUN:
                return SoundType.SHOOT_SHOTGUN;
            case GunType.SNIPER_RIFLE:
                return SoundType.SHOOT_SNIPER;
            case GunType.SUBMACHINE_GUN:
                return SoundType.SHOOT_SMG;
            default:
                return SoundType.HIT;
        }
    }

    /// <summary>
    /// Rotates this vector around the given degree.
    /// </summary>
    /// <param name="v"></param>
    /// <param name="degrees"></param>
    /// <returns></returns>
    public static Vector2 Rotate(this Vector2 v, float degrees) {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;

        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);

        return v;
    }
}
