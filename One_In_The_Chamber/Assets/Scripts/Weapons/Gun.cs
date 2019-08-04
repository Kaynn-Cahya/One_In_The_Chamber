using UnityEngine;

using MyBox;

public abstract class Gun : MonoBehaviour {

	protected static float maxRaycastDistance;

	[Separator("Base Gun Properties")]

	[SerializeField, Tooltip("What type of gun is this."), SearchableEnum]
	protected GunType gunType;

	[SerializeField, Tooltip("How much recoil this gun has"), PositiveValueOnly]
	protected float gunRecoil;

	[SerializeField, Tooltip("The firerate of this game"), PositiveValueOnly]
	private float gunFireRate;

	[SerializeField, Tooltip("How much bullet this gun can fire out in 1 shot"), PositiveValueOnly]
	protected int bulletCountPerShot = 1;

	[SerializeField, ConditionalField(nameof(bulletCountPerShot), true, 1), Tooltip("How wide is the spread of each bullet"), Range(0f, 360f)]
	protected float shotWideness;

	[Separator("Bullet properties")]

	[SerializeField, Tooltip("What bullet prefab this gun fires"), MustBeAssigned]
	protected Bullet bulletPrefab;

	[SerializeField, Tooltip("True if this gun should use raycasting to see if the shot hit an enemy")]
	protected bool raycastToHitEnemy;

	[SerializeField, Tooltip("How much knockback to inflict."), PositiveValueOnly]
	protected float bulletKnockBack;

	[SerializeField, Tooltip("How fast this bullet should fly"), PositiveValueOnly]
	private float bulletSpeed;

	[SerializeField, Tooltip("How much to rotate the bullet by when we create it."), PositiveValueOnly]
	private float bulletRotationOffset;


	[Separator("Other gun properties")]
	[SerializeField, Tooltip("The tag for the enemy"), Tag]
	protected string enemyTag;

    [SerializeField, Tooltip("True if this gun has a seperate animation when its loaded/unloaded")]
    protected bool hasLoadedAnimation;

    protected BulletProperties bulletProperties;

    /// <summary>
    /// The player who carries this gun.
    /// </summary>
    public Player GunOwner { get; set; }

	public bool CanFire {
		get => IsLoaded && gunFireRateTimer >= gunFireRate;
	}

	public float GetGunRecoil {
		get => gunRecoil;
	}

    /// <summary>
    /// The type of this gun.
    /// </summary>
    public GunType GunType {
        get => gunType;
    }

	/// <summary>
	/// True if this gun is loaded with an ammunition
	/// </summary>
	public bool IsLoaded { get; protected set; }

	private float gunFireRateTimer;

	protected abstract void OnAwake();

	private void Awake() {
		FindMaxRaycastDistance();
		bulletProperties = new BulletProperties(bulletSpeed, bulletKnockBack, !raycastToHitEnemy);
		gunFireRateTimer = gunFireRate;
        IsLoaded = true;
		OnAwake();

		#region Local_Function

		void FindMaxRaycastDistance() {
			maxRaycastDistance = Mathf.Sqrt(Mathf.Pow(Screen.height, 2) + Mathf.Pow(Screen.width, 2));
		}

		#endregion
	}

	protected abstract void OnUpdate();

	private void Update() {
		if(gunFireRateTimer < gunFireRate) {
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

        if (hasLoadedAnimation) {
            GunOwner.PlayerSetGunAnimationLoadedState(false);
        }

        Time.timeScale = 0.05f;
    }

	protected abstract void OnGunLoaded();

	/// <summary>
	/// Loads this gun with ammunition.
	/// </summary>
	public void LoadGun() {
        if (hasLoadedAnimation) {
            GunOwner.PlayerSetGunAnimationLoadedState(true);
        }

        SoundManager.Instance.PlayAudioFileBySoundType(SoundType.RELOAD);

        IsLoaded = true;
		OnGunLoaded();
	}


	protected Bullet CreateNewBullet() {
		var newBullet = Instantiate(bulletPrefab);

		SetNewBulletPosition();
		SetNewBulletRotation();

		return newBullet;

		#region Local_Function

		void SetNewBulletPosition() {
			newBullet.transform.position = transform.position;
		}

		void SetNewBulletRotation() {
			newBullet.transform.rotation = Quaternion.identity;
			newBullet.transform.Rotate(new Vector3(0, 0, bulletRotationOffset));

			Quaternion rotation = Quaternion.LookRotation(transform.forward, transform.TransformDirection(Vector3.up + new Vector3(0, 0, -bulletRotationOffset)));
			newBullet.transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
		}

		#endregion
	}

}
