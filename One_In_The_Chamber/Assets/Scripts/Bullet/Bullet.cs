using UnityEngine;

using MyBox;

public class Bullet : MonoBehaviour {

    [SerializeField, Tooltip("True if this bullet leaves a trail behind it")]
    private bool hasBulletTrail;

    [ConditionalField(nameof(hasBulletTrail), false, true),SerializeField, Tooltip("The prefab for the trail renderer")]
    private GameObject bulletTrailRenderer;

    private bool pushEnemyOnHit;

    private float bulletSpeed;

    private Vector2 flyDirection;

    private Vector2 flyDestination;

    private bool flyToDestination;

    private GameObject bulletTrail;

    private void Update() {
        if (flyToDestination) {
            transform.position = Vector2.MoveTowards(transform.position, flyDestination, Time.deltaTime * bulletSpeed);
        } else {
            transform.position += (Vector3) (flyDirection * bulletSpeed * Time.deltaTime);
        }

        if (flyToDestination) {
            DestroyBulletIfDestinationReached();
        }

        if (hasBulletTrail) {
            UpdateBulletTrailPosition();
        }

        #region Local_Function

        void DestroyBulletIfDestinationReached() {
            if (Vector2.Distance(transform.position, flyDestination) < 1.0f) {
                Destroy(gameObject);
            }
        }

        void UpdateBulletTrailPosition() {
            bulletTrail.transform.position = transform.position;
        }

        #endregion
    }

    public void InitalizeBulletWithDirection(BulletProperties bulletProperties, Vector2 direction) {
        bulletSpeed = bulletProperties.Speed;
        pushEnemyOnHit = bulletProperties.AffectEnemyOnHit;
        flyDirection = direction;
        flyToDestination = false;

        CreateBulletTrailRendererIfNeeded();
    }

    public void InitalizeBulletWithDestination(BulletProperties bulletProperties, Vector2 destination) {
        bulletSpeed = bulletProperties.Speed;
        pushEnemyOnHit = bulletProperties.AffectEnemyOnHit;
        flyDestination = destination;
        flyToDestination = true;

        CreateBulletTrailRendererIfNeeded();
    }

    private void CreateBulletTrailRendererIfNeeded() {
        if (hasBulletTrail) {
            bulletTrail = Instantiate(bulletTrailRenderer);
            bulletTrail.transform.position = transform.position;
        }
    }

    public void TriggerBulletContactedEnemy() {

        Destroy(gameObject);
    }
}
