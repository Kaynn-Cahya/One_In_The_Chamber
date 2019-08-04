using UnityEngine;

using MyBox;

public class Player : Character {

    #region PlayerLoadout_Struct

    [System.Serializable]
    private struct PlayerLoadout {

        [SerializeField, Tooltip("The gun for this loadout"), MustBeAssigned]
        private Gun loadoutGun;

        [SerializeField, Tooltip("The animator controller the player should switch to when using this loadout"), MustBeAssigned]
        private RuntimeAnimatorController loadoutAnimatorController;

        public GunType LoadoutGunType { get => loadoutGun.GunType; }
        public Gun LoadoutGun { get => loadoutGun; }
        public RuntimeAnimatorController LoadoutAnimatorController { get => loadoutAnimatorController; }
    }

    #endregion

    [Separator("Player controls")]
	[SerializeField, Tooltip("The button to press to fire the gun"), SearchableEnum]
	private KeyCode fireGunKeyCode;

    [Separator("Player Weapon")]

    [SerializeField, Tooltip("The type of gun the player should start with"), SearchableEnum]
    private GunType startingGunType;

    [SerializeField, Tooltip("The loadouts this player has"), MustBeAssigned]
    private PlayerLoadout[] playerLoadouts;

    [Separator("Player movement")]
    [SerializeField, Tooltip("How much friction is there for the player when moving around"), PositiveValueOnly]
    private float friction;

    /// <summary>
    /// The gun this player is currently holding.
    /// </summary>
	private Gun currPlayerGun;

    private Animator playerAnimator;

	private Camera gameCamera;

	private void Awake() {
		gameCamera = Camera.main;
        IsActive = true;
    }

	protected override void OnStart() {
        SetupAllLoadoutWeapons();
        playerAnimator = GetComponent<Animator>();
        SwitchToLoadoutOfGunType(startingGunType);

        #region Local_Function

        void SetupAllLoadoutWeapons() {
            foreach (var loadout in playerLoadouts) {
                loadout.LoadoutGun.gameObject.SetActive(false);
                loadout.LoadoutGun.GunOwner = this;
            }
        }

        #endregion
    }

    protected override void OnUpdate() {
        RotatePlayerBasedOnMousePosition(Time.deltaTime);

        if (Input.GetKeyDown(fireGunKeyCode) && currPlayerGun.CanFire) {
            currPlayerGun.FireGun();
            HandlePlayerRecoil();
        }
    }

    private void FixedUpdate() {
        if (charRB.velocity.magnitude != 0f) {
            charRB.velocity -= friction * Time.fixedDeltaTime * (charRB.velocity.normalized);
        }
    }

    private void RotatePlayerBasedOnMousePosition(float deltaTime) {

		Vector3 positionToRotateTowards = gameCamera.ScreenToWorldPoint(Input.mousePosition);

		RotateCharacterToPositionOnFrame(positionToRotateTowards, deltaTime);
	}

	private void HandlePlayerRecoil() {
		charRB.AddForce(-transform.up * currPlayerGun.GetGunRecoil, ForceMode2D.Impulse);
	}

    public void PlayerGunFiredAnimation() {
        playerAnimator.SetTrigger("Fire");
    }

    public void PlayerSetGunAnimationLoadedState(bool isLoaded) {
        playerAnimator.SetBool("Loaded", isLoaded);
    }

    public void SwitchToLoadoutOfGunType(GunType gunType) {

        if (TryGetLoadoutByGunType(out PlayerLoadout loadoutToSwitchTo)) {
            currPlayerGun?.gameObject.SetActive(false);
            currPlayerGun = loadoutToSwitchTo.LoadoutGun;
            currPlayerGun.gameObject.SetActive(true);

            playerAnimator.runtimeAnimatorController = loadoutToSwitchTo.LoadoutAnimatorController;
        }

        #region Local_Function

        bool TryGetLoadoutByGunType(out PlayerLoadout result) {

            foreach (var loadout in playerLoadouts) {
                if (loadout.LoadoutGunType == gunType) {
                    result = loadout;
                    return true;
                }
            }

            result = new PlayerLoadout();
            return false;
        }

        #endregion
    }

    protected override void OnCharacterFallOffArena() {
        IsActive = false;
    }
}
