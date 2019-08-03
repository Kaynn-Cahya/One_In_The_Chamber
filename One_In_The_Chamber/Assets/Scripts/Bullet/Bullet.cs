using UnityEngine;

public class Bullet : MonoBehaviour {
    private bool pushEnemyOnHit;

    private float bulletSpeed;

    private Vector2 flyDirection;

    private Vector2 flyDestination;

    private bool flyToDestination;

    private void Update() {
        if (flyToDestination) {
            transform.position = Vector2.MoveTowards(transform.position, flyDestination, Time.deltaTime * bulletSpeed);
        } else {
            transform.position += (Vector3) (flyDirection * bulletSpeed * Time.deltaTime);
        }

        if (flyToDestination) {
            DestroyBulletIfDestinationReached();
        }

        #region Local_Function

        void DestroyBulletIfDestinationReached() {
            if (Vector2.Distance(transform.position, flyDestination) < 1.0f) {
                Destroy(gameObject);
            }
        }

        #endregion
    }

    public void InitalizeBulletWithDirection(BulletProperties bulletProperties, Vector2 direction) {
        bulletSpeed = bulletProperties.Speed;
        pushEnemyOnHit = bulletProperties.AffectEnemyOnHit;
        flyDirection = direction;
        flyToDestination = false;
    }

    public void InitalizeBulletWithDestination(BulletProperties bulletProperties, Vector2 destination) {
        bulletSpeed = bulletProperties.Speed;
        pushEnemyOnHit = bulletProperties.AffectEnemyOnHit;
        flyDestination = destination;
        flyToDestination = true;
    }

    public void TriggerBulletContactedEnemy() {

        Destroy(gameObject);
    }
}
