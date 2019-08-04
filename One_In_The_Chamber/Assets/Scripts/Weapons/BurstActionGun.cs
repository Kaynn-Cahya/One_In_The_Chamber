using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyBox;

public class BurstActionGun : Gun
{
    [Separator("BurstActionGun Properties", true)]
    [Range(0, 360), SerializeField, Tooltip("The angle which inaccurate shots will go.")]
    private float inaccuracyAngle;

    [SerializeField, Tooltip("How many rounds it can fire in one burst"), PositiveValueOnly]
    private int roundsPerBurst;

    [SerializeField, Tooltip("How many seconds to wait before the next shot appears"), PositiveValueOnly]
    private float shotIntervalDuration;

    protected override void OnAwake() {
    }

    protected override void OnGunFired() {
        StartCoroutine(FireBurstCoroutine());
    }

    private IEnumerator FireBurstCoroutine() {
        FireFirstShot();

        int shotsFired = 1;

        Vector2 initalDirection = transform.up.normalized;

        Vector2 bulletMoveDirection;
        float halfOfInaccuracyAngle = inaccuracyAngle / 2f;
        while (roundsPerBurst > shotsFired) {
            yield return new WaitForSeconds(shotIntervalDuration);

            float generatedFiringAngle = Random.Range(-halfOfInaccuracyAngle, halfOfInaccuracyAngle);
            bulletMoveDirection = initalDirection.Rotate(generatedFiringAngle);

            if (raycastToHitEnemy) {
                InitalizeBulletBasedOnRaycastResult(bulletMoveDirection);
            } else {
                var newBullet = CreateNewBullet();
                newBullet.InitalizeBulletWithDirection(bulletProperties, bulletMoveDirection, this);
            }
            GunOwner.PlayerGunFiredAnimation();
            ++shotsFired;
        }

        yield return null;

        #region Local_Function

        void FireFirstShot() {
            if (raycastToHitEnemy) {
                InitalizeBulletBasedOnRaycastResult(transform.up.normalized);
            } else {
                InitalizeBulletWithoutRaycast(transform.up.normalized);
            }

            GunOwner.PlayerGunFiredAnimation();
        }

        #endregion
    }

    private void InitalizeBulletWithoutRaycast(Vector2 bulletMoveDirection) {
        var newBullet = CreateNewBullet();
        newBullet.InitalizeBulletWithDirection(bulletProperties, bulletMoveDirection, this);
    }

    private void InitalizeBulletBasedOnRaycastResult(Vector2 shootDirection) {
        var newBullet = CreateNewBullet();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, shootDirection, maxRaycastDistance);

        if (hit.collider != null) {

            if (hit.collider.gameObject.CompareTag(enemyTag)) {
                newBullet.InitalizeBulletWithDestination(bulletProperties, hit.point, this);
                hit.collider.GetComponent<Character>().TriggerCharacterHit(hit.point, bulletKnockBack);
                LoadGun();
            } else {
                newBullet.InitalizeBulletWithDirection(bulletProperties, shootDirection, this);
            }
        } else {
            newBullet.InitalizeBulletWithDirection(bulletProperties, shootDirection, this);
        }
    }

    protected override void OnGunLoaded() {
    }

    protected override void OnUpdate() {
    }
}
