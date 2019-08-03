using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyBox;

public class GameBorderSetter : MonoBehaviour {

    [SerializeField, Tooltip("The game camera"), MustBeAssigned]
    private Camera gameCamera;

    [SerializeField, Tooltip("The respective game-wall borders in the game"), MustBeAssigned]
    private Collider2D topWall, bottomWall, rightWall, leftWall;

    [SerializeField, Tooltip("Offset for the heigth and width of the wall respectively.")]
    private float xOffSet, yOffSet;

    private void Start() {
        SetWallPos();

        // This script is no longer needed in the scene.
        Destroy(this);
    }

    private void SetWallPos() {
        Vector2 temp;
        float wallOffsetBySize;

        SetTopWallPosition();
        SetBottomWallPosition();
        SetLeftWallPosition();
        SetRightWallPosition();

        #region Local_Function

        void SetTopWallPosition() {
            // Get the top middle position of the camera.
            temp = gameCamera.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height));
            // Find the offset (wall size) of the wall.
            wallOffsetBySize = topWall.bounds.size.y;
            // Move the top left wall to the top-middle position of the camera, adjusting with the offset.
            topWall.transform.position = (temp + new Vector2(0, wallOffsetBySize / 2f + yOffSet));
        }

        void SetBottomWallPosition() {
            temp = gameCamera.ScreenToWorldPoint(new Vector2(Screen.width / 2f, 0));
            wallOffsetBySize = bottomWall.bounds.size.y;
            bottomWall.transform.position = (temp - new Vector2(0, wallOffsetBySize / 2f + yOffSet));
        }

        void SetRightWallPosition() {
            temp = gameCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height / 2f));
            wallOffsetBySize = rightWall.bounds.size.x;
            rightWall.transform.position = (temp + new Vector2(wallOffsetBySize / 2f + xOffSet, 0));
        }

        void SetLeftWallPosition() {
            temp = gameCamera.ScreenToWorldPoint(new Vector2(0, Screen.height / 2f));
            wallOffsetBySize = leftWall.bounds.size.x;
            leftWall.transform.position = (temp - new Vector2(wallOffsetBySize / 2f + xOffSet, 0));
        }

        #endregion
    }

}
