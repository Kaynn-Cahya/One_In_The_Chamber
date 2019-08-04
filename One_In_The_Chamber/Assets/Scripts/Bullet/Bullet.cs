using UnityEngine;

using MyBox;

public class Bullet : MonoBehaviour {

    protected delegate void OnBulletContactedEnemy();

    protected OnBulletContactedEnemy onBulletContactedEnemy;

    [SerializeField, Tooltip("True if this bullet leaves a trail behind it")]
    private bool hasBulletTrail;

    [ConditionalField(nameof(hasBulletTrail), false, true), SerializeField, Tooltip("The prefab for the trail renderer")]
    private GameObject bulletTrailRenderer;

    private bool pushEnemyOnHit;

    private float bulletSpeed;

    private float knockback;

    private Vector2 flyDirection;

    private Vector2 flyDestination;

    private bool flyToDestination;

    private GameObject bulletTrail;

    /// <summary>
    /// The gun that shot out this bullet.
    /// </summary>
    private Gun parentGun;

    public float Knockback { get => knockback; }

    private void Update() {
        if (flyToDestination) {
            transform.position = Vector2.MoveTowards(transform.position, flyDestination, Time.deltaTime * bulletSpeed);
        } else {
            transform.position += (Vector3)(flyDirection * bulletSpeed * Time.deltaTime);
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

    public void InitalizeBulletWithDirection(BulletProperties bulletProperties, Vector2 direction, Gun shooter) {
        SetBulletProperties(bulletProperties);
        flyDirection = direction;
        flyToDestination = false;

        parentGun = shooter;

        CreateBulletTrailRendererIfNeeded();
    }

    public void InitalizeBulletWithDestination(BulletProperties bulletProperties, Vector2 destination, Gun shooter) {
        SetBulletProperties(bulletProperties);
        flyDestination = destination;
        flyToDestination = true;

        parentGun = shooter;

        CreateBulletTrailRendererIfNeeded();
    }

    private void SetBulletProperties(BulletProperties bulletProperties) {
        bulletSpeed = bulletProperties.Speed;
        pushEnemyOnHit = bulletProperties.AffectEnemyOnHit;
        knockback = bulletProperties.Knockback;
    }

    private void CreateBulletTrailRendererIfNeeded() {
        if (hasBulletTrail) {
            bulletTrail = Instantiate(bulletTrailRenderer);
            bulletTrail.transform.position = transform.position;
        }
    }

    public void TriggerBulletContactedEnemy() {

        onBulletContactedEnemy?.Invoke();
        parentGun.LoadGun();

        Destroy(gameObject);
    }
}
