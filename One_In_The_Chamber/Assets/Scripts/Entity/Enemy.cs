using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class Enemy : Character {
	[Separator("Enemy Movement Properties")]

	[SerializeField, Tooltip("How fast this character can move around"), PositiveValueOnly]
	private float movementSpeed;

	private Transform player;

	protected override void OnStart() {
		player = GameObject.FindWithTag("Player").transform;
	}

	private void Update() {
		RotateCharacterToPositionOnFrame(player.position, Time.deltaTime);

		MoveTowardsPlayer();
	}

	protected void MoveTowardsPlayer() {
		charRB.velocity = transform.up * movementSpeed * Time.deltaTime;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		// TODO: Make enemy get hit.
		// (For guns that dont depend on raycast.)

		// Call the below function as a callback to the bullet to indiciate that it hit an enemy.
		//collision.gameObject.GetComponent<Bullet>().TriggerBulletContactedEnemy();
	}
}
