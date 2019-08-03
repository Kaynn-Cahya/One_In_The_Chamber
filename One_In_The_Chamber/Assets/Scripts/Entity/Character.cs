using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyBox;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Character : MonoBehaviour {

    [Separator("Character Rotation Properties")]

    [SerializeField, Tooltip("How fast this character can rotate around"), PositiveValueOnly]
    private float rotationSpeed;

    [SerializeField, Tooltip("The offset value to use when this character is rotating around"), Range(-360f, 360f)]
    private float rotationOffsetValue;

    protected Rigidbody2D charRB;

    protected abstract void OnStart();
    private void Start() {
        charRB = GetComponent<Rigidbody2D>();
        OnStart();
    }

    /// <summary>
    /// Rotate this character to face the desired position, limited by the deltaTime and the character's rotation speed.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="deltaTime"></param>
    protected void RotateCharacterToPositionOnFrame(Vector3 position, float deltaTime) {
        float angle = GetAngleToRotateTowards() - rotationOffsetValue;

        Quaternion rotateTo = FindRotationValueInQuaternion();

        RotatePlayerToRotationValue();

        #region Local_Function

        float GetAngleToRotateTowards() {
            Vector2 direction = position - transform.position;
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        Quaternion FindRotationValueInQuaternion() {
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }

        void RotatePlayerToRotationValue() {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateTo, rotationSpeed * deltaTime);
        }

        #endregion
    }

    protected void SnapCharacterRotationToFacePosition(Vector3 position) {
        float angle = GetAngleToRotateTowards() - rotationOffsetValue;

        Quaternion rotateTo = FindRotationValueInQuaternion();

        transform.rotation = rotateTo;

        #region Local_Function

        float GetAngleToRotateTowards() {
            Vector2 direction = position - transform.position;
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        Quaternion FindRotationValueInQuaternion() {
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }

        #endregion
    }

    /// <summary>
    /// Trigger that this character got hit.
    /// </summary>
    /// <param name="hitOrigin">Where the character got hit from.</param>
    public void TriggerCharacterHit(Vector2 hitOrigin) {
        // TODO: Knockback this character by on where the character got hit from.
    }
}
