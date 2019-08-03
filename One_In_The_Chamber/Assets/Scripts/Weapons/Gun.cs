using UnityEngine;

using MyBox;

public abstract class Gun : MonoBehaviour {

    [SerializeField, Tooltip("What type of gun is this."), SearchableEnum]
    protected GunType gunType;

    [SerializeField, Tooltip("How much recoil this gun has"), PositiveValueOnly]
    protected float gunRecoil;

    [SerializeField, Tooltip("The firerate of this game"), PositiveValueOnly]
    private float gunFireRate;

    [SerializeField, Tooltip("How much bullet this gun can fire out in 1 shot"), PositiveValueOnly]
    protected int bulletCountPerShot = 1;

    [SerializeField, ConditionalField("bulletCountPerShot", 1, false), Tooltip("How wide is the sprite of each bullet"), Range(0f, 360f)]
    private float spread;

    [Separator("Bullet properties")]

    [SerializeField, Tooltip("What bullet prefab this gun fires"), MustBeAssigned]
    protected Bullet bulletPrefab;

    [SerializeField, Tooltip("True if this gun should use raycasting to see if the shot hit an enemy")]
    private bool raycastToHitEnemy;

    [SerializeField, Tooltip("How fast this bullet should fly"), PositiveValueOnly]
    private float bulletSpeed;

    protected BulletProperties bulletProperties;

    public bool CanFire {
        get => IsLoaded && gunFireRateTimer >= gunFireRate;
    }

    /// <summary>
    /// True if this gun is loaded with an ammunition
    /// </summary>
    public bool IsLoaded { get; protected set; }

    private float gunFireRateTimer;

    protected abstract void OnAwake();

    private void Awake() {
        bulletProperties = new BulletProperties(bulletSpeed, !raycastToHitEnemy);
        gunFireRateTimer = gunFireRate;
        OnAwake();
    }

    protected abstract void OnUpdate();

    private void Update() {
        if (gunFireRateTimer < gunFireRate) {
            gunFireRateTimer += Time.deltaTime;
        }

        OnUpdate();
    }

    protected abstract void OnGunFired();

    /// <summary>
    /// Fires this gun.
    /// </summary>
    public void FireGun() {
        gunFireRateTimer = 0f;
        IsLoaded = false;
        OnGunFired();
    }

    protected abstract void OnGunLoaded();

    /// <summary>
    /// Loads this gun with ammunition.
    /// </summary>
    public void LoadGun() {
        IsLoaded = true;
        OnGunLoaded();
    }


}
