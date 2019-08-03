using UnityEngine;

using MyBox;

public class SnapToMousePos : MonoBehaviour {

    [SerializeField, Tooltip("The camera to use"), MustBeAssigned]
    private Camera targetCamera;

    void LateUpdate() {
        transform.position = (Vector2) targetCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
