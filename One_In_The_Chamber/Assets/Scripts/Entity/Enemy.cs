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
        // TODO: Refactor
		player = GameObject.FindWithTag("Player").transform;
	}

	private void Update() {
		RotateCharacterToPositionOnFrame(player.position, Time.deltaTime);
	}

    private void FixedUpdate() {
        MoveTowardsPlayer(Time.fixedDeltaTime);
    }

	protected void MoveTowardsPlayer(float deltaTime) {
		charRB.velocity = transform.up * movementSpeed * deltaTime;
	}
}
