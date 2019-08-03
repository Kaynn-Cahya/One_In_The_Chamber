using UnityEngine;

using MyBox;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class GameBorder : MonoBehaviour {

    [SerializeField, Tooltip("Dont destroy any objects with the respective tags"), Tag]
    private string[] dontDestroyTags;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (GameObjectMatchesDontDestroyTags(collision.gameObject)) { return; }

        Destroy(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (GameObjectMatchesDontDestroyTags(collision.gameObject)) { return; }

        Destroy(collision.gameObject);
    }

    private bool GameObjectMatchesDontDestroyTags(GameObject gameObj) {
        foreach (var tag in dontDestroyTags) {
            if (gameObj.CompareTag(tag)) {
                return true;
            }
        }

        return false;
    }
}
