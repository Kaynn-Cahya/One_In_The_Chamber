using System.Collections.Generic;
using UnityEngine;

using MyBox;

public class ExplosiveBullet : Bullet {
    [SerializeField, Tooltip("How big is the radius of the explosion"), MustBeAssigned]
    private float explosionRadius;

    [SerializeField, Tooltip("The min and max knockback that can be gotten from the explosion"), MinMaxRange(0f, 100f)]
    private RangedFloat minMaxExplosionKnockback;

    [SerializeField, Tooltip("The explosion effect prefab"), MustBeAssigned]
    private GameObject explosionEffectPrefab;

    [SerializeField, Tooltip("The sound the explosion makes")]
    private SoundType explosionSound;

    private float MinKnockback {
        get => minMaxExplosionKnockback.Min;
    }

    private float MaxKnockback {
        get => minMaxExplosionKnockback.Max;
    }

    private void Awake() {
        onBulletContactedEnemy += Explode;
    }

    private void Explode() {
        SoundManager.Instance.PlayAudioFileBySoundType(explosionSound);

        CreateExplosionEffectAtLocation();

        KnockbackEnemiesInRadius();

        #region Local_Function

        void CreateExplosionEffectAtLocation() {
            var explosionEff = Instantiate(explosionEffectPrefab);
            explosionEff.transform.position = transform.position;
        }

        void KnockbackEnemiesInRadius() {
            HashSet<Enemy> enemiesInRadius = FindAllEnemiesInRadiusOfExplosion();

            foreach (var enemyInRadius in enemiesInRadius) {
                var distanceToEnemy = GetDistanceToEnemy(enemyInRadius);

                // Lerp knockback applied based on the distance from the explosion to the enemy.
                var t = distanceToEnemy / explosionRadius;
                float knockbackToEnemy = Mathf.Lerp(MinKnockback, MaxKnockback, t);
                enemyInRadius.TriggerCharacterHit(transform.position, knockbackToEnemy);
            }
        }

        HashSet<Enemy> FindAllEnemiesInRadiusOfExplosion() {
            return EnemyManager.Instance.FetchEnemiesByCondition<HashSet<Enemy>>(EnemyIsInRadius);
        }

        float GetDistanceToEnemy(Enemy enemy) {
            return Vector2.Distance(enemy.transform.position, transform.position);
        }

        bool EnemyIsInRadius(Enemy enemy) {
            return GetDistanceToEnemy(enemy) <= explosionRadius;
        }

        #endregion
    }
}
