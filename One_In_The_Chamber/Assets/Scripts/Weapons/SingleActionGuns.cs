using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyBox;

public class SingleActionGuns : Gun {
	protected override void OnGunLoaded() {

	}

	protected override void OnGunFired() {
		if(bulletCountPerShot == 1) {
			FireSingleShot();
		} else {
			FireMultipleShots();
		}
	}

	private void FireSingleShot() {
		Vector2 shootDirection = transform.up.normalized;
		var newBullet = CreateNewBullet();

		if(raycastToHitEnemy) {
			InitalizeBulletBasedOnRaycastResult();
		} else {
			newBullet.InitalizeBulletWithDirection(bulletProperties, shootDirection, this);
		}
        GunOwner.PlayerGunFiredAnimation();

        #region Local_Function

        void InitalizeBulletBasedOnRaycastResult() {
			RaycastHit2D hit = Physics2D.Raycast(transform.position, shootDirection, maxRaycastDistance);

			if(hit.collider != null) {

				if(hit.collider.gameObject.CompareTag(enemyTag)) {
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

		#endregion
	}

	private void FireMultipleShots() {
		Vector2 shootDirection = transform.up.normalized;
		Vector2 initalDirection = shootDirection;

		float offSet = shotWideness / 2;

		float angleStep = (shotWideness / (bulletCountPerShot + 1));
		float currFiringAngle = (angleStep - offSet);

		Vector2 bulletMoveDirection;
		FireShots();

		#region Local_Function

		void FireShots() {
			// For each pellet we have to shoot
			for(int i = 0; i < bulletCountPerShot; ++i) {
				// Find out where the bullet have to move to from the current shooting angle.
				bulletMoveDirection = initalDirection.Rotate(currFiringAngle);

				CreateAndInitalizeBulletByBulletMoveDirection();

				currFiringAngle += angleStep;
                GunOwner.PlayerGunFiredAnimation();
            }
		}

		void CreateAndInitalizeBulletByBulletMoveDirection() {
			if(raycastToHitEnemy) {
				InitalizeBulletBasedOnRaycastResult();
			} else {
				var newBullet = CreateNewBullet();
				newBullet.InitalizeBulletWithDirection(bulletProperties, bulletMoveDirection, this);
			}
		}

		void InitalizeBulletBasedOnRaycastResult() {
			var newBullet = CreateNewBullet();
			RaycastHit2D hit = Physics2D.Raycast(transform.position, bulletMoveDirection, maxRaycastDistance);

			if(hit.collider != null) {

				if(hit.collider.gameObject.CompareTag(enemyTag)) {
					newBullet.InitalizeBulletWithDestination(bulletProperties, hit.point, this);
					hit.collider.GetComponent<Character>().TriggerCharacterHit(hit.point, bulletKnockBack);
                    LoadGun();
                } else {
					newBullet.InitalizeBulletWithDirection(bulletProperties, bulletMoveDirection, this);
				}
			} else {
				newBullet.InitalizeBulletWithDirection(bulletProperties, bulletMoveDirection, this);
			}
		}

		#endregion
	}

	protected override void OnAwake() {
	}

	protected override void OnUpdate() {
	}
}
