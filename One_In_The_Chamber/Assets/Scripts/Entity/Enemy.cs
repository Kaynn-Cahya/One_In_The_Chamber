using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class Enemy : Character
{
    [Separator("Enemy Movement Properties")]

    [SerializeField, Tooltip("How fast this character can move around"), PositiveValueOnly]
    private float movementSpeed;

    private Transform player;
    private Rigidbody2D rb2d;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();

        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update() {
        RotateCharacterToPositionOnFrame(player.position, Time.deltaTime);

        MoveTowardsPlayer();
    }

    protected void MoveTowardsPlayer() {
        rb2d.velocity = transform.up * movementSpeed * Time.deltaTime;
    }
}
