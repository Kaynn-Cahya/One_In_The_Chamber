[System.Serializable]
public struct BulletProperties {

    public bool AffectEnemyOnHit { get; private set; }
    public float Speed { get; private set; }

    public float Knockback { get; private set; }

    public BulletProperties(float speed, float knockback, bool affectEnemyOnHit = false) {
        AffectEnemyOnHit = affectEnemyOnHit;
        Speed = speed;
        Knockback = knockback;
    }
}
