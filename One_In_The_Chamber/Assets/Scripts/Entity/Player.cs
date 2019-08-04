using UnityEngine;
using UnityEditor.Animations;

using MyBox;

public class Player : Character {

    #region PlayerLoadout_Struct

    [System.Serializable]
    private struct PlayerLoadout {

        [SerializeField, Tooltip("The gun for this loadout"), MustBeAssigned]
        private Gun loadoutGun;

        [SerializeField, Tooltip("The animator controller the player should switch to when using this loadout"), MustBeAssigned]
        private AnimatorController loadoutAnimatorController;

        public GunType LoadoutGunType { get => loadoutGun.GunType; }
        public Gun LoadoutGun { get => loadoutGun; }
        public AnimatorController LoadoutAnimatorController { get => loadoutAnimatorController; }
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

    /// <summary>
    /// The gun this player is currently holding.
    /// </summary>
	private Gun currPlayerGun;

    private Animator playerAnimator;

	private Camera gameCamera;

	private void Awake() {
		gameCamera = Camera.main;
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

    private void Update() {
		RotatePlayerBasedOnMousePosition(Time.deltaTime);

		if(Input.GetKeyDown(fireGunKeyCode) && currPlayerGun.CanFire) {
			currPlayerGun.FireGun();
			HandlePlayerRecoil();
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
}
