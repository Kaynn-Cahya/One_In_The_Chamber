[System.Serializable]
public struct BulletProperties {

    public bool AffectEnemyOnHit { get; private set; }
    public float Speed { get; private set; }

    public BulletProperties(float speed, bool affectEnemyOnHit = false) {
        AffectEnemyOnHit = affectEnemyOnHit;
        Speed = speed;
    }
}
