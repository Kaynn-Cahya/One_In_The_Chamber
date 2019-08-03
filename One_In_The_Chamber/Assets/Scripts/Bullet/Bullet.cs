using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour {
    private bool pushEnemyOnHit;

    private float bulletSpeed;

    private Vector2 flyDirection;

    public void InitalizeBullet(BulletProperties bulletProperties, Vector2 direction) {
        bulletSpeed = bulletProperties.Speed;
        pushEnemyOnHit = bulletProperties.AffectEnemyOnHit;
        flyDirection = direction;
    }
}
