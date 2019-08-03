using UnityEngine;

using MyBox;

public abstract class Gun : MonoBehaviour {

    [SerializeField, Tooltip("What type of gun is this."), SearchableEnum]
    protected GunType gunType;

    [SerializeField, Tooltip("What bullet prefab this gun fires"), MustBeAssigned]
    protected Bullet bulletPrefab;

    [SerializeField, Tooltip("How much recoil this gun has"), PositiveValueOnly]
    protected float gunRecoil;

    public abstract bool CanFire { get; protected set; }

    /// <summary>
    /// Fires this gun.
    /// </summary>
    public abstract void Fire();

    /// <summary>
    /// Loads this gun with ammunition.
    /// </summary>
    public abstract void LoadGun();
}
