using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyBox;

public class Player : Character {

    [SerializeField, Tooltip("The camera used for the player to aim with the mouse in the game"), MustBeAssigned]
    private Camera gameCamera;

    private void Update() {
        RotatePlayerBasedOnMousePosition(Time.deltaTime);
    }

    private void RotatePlayerBasedOnMousePosition(float deltaTime) {

        Vector3 positionToRotateTowards = gameCamera.ScreenToWorldPoint(Input.mousePosition);

        RotateCharacterToPositionOnFrame(positionToRotateTowards, deltaTime);
    }
}
