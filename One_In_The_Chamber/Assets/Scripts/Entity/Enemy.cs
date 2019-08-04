using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class Enemy : Character {

    [Separator("Enemy Collision")]
    [SerializeField, Tooltip("The tag for the bullet"), Tag, MustBeAssigned]
    private string bulletTag;

    [Separator("Enemy Movement Properties")]

    [SerializeField, Tooltip("How fast it picks up to speed after being shot at"), PositiveValueOnly]
    private float acceleration;

	[SerializeField, Tooltip("How fast this character can move around"), PositiveValueOnly]
	private float maxMovementSpeed;

    private float currentSpeed;

	private Player playerChar;

	protected override void OnStart() {
        currentSpeed = 0f;
    }

    protected override void OnUpdate() {
        RotateCharacterToPositionOnFrame(playerChar.transform.position, Time.deltaTime);
    }

    private void FixedUpdate() {
        if (!IsActive) { return; }

        MoveTowardsPlayer(Time.fixedDeltaTime);
    }

	protected void MoveTowardsPlayer(float deltaTime) {
        currentSpeed += deltaTime * acceleration;

        if (charRB.velocity.magnitude > maxMovementSpeed) {
            currentSpeed = maxMovementSpeed;
        } else {
            charRB.velocity += (Vector2)(transform.up * acceleration * deltaTime);
        }
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag(bulletTag)) {
            charRB.velocity = Vector2.zero;
            HandleEnemyHitByBullet();

            SoundManager.Instance.PlayAudioFileBySoundType(SoundType.HIT);
        }

        #region Local_Function

        void HandleEnemyHitByBullet() {
            var hitBullet = collision.gameObject.GetComponent<Bullet>();

            TriggerCharacterHit(collision.gameObject.transform.position, hitBullet.Knockback);
            hitBullet.TriggerBulletContactedEnemy();
        }

        #endregion
    }

    public void InitalizeEnemy(Player player) {
        playerChar = player;
    }

    protected override void OnCharacterFallOffArena() {
        GameManager.Instance.TriggerEnemyFellOffArena();
        Destroy(gameObject);
    }
}
