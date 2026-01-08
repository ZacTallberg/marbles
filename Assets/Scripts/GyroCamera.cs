using UnityEngine;
using System.Collections;

public class GyroCamera : MonoBehaviour {

    void Start () {
        // Ensure InputManager exists
        if (AdvancedInputManager.Instance == null) {
            GameObject go = new GameObject("AdvancedInputManager");
            go.AddComponent<AdvancedInputManager>();
        }
    }

    void Update() {
        if (AdvancedInputManager.Instance != null) {
            Vector2 rotationInput = AdvancedInputManager.Instance.GetCompositeRotation();

            // X input (Yaw) rotates around Y axis
            // Y input (Pitch) rotates around X axis

            // Note: Mouse X -> Yaw, Mouse Y -> Pitch
            // Rotation is relative (delta).

            // Apply sensitivity and Time.deltaTime if not already handled.
            // AdvancedInputManager sums raw inputs.
            // Mouse is frame-delta. Gyro is rate.

            // We need to apply Time.deltaTime for rate-based inputs if they were raw rates.
            // But GyroInputProvider returned scaled rate.

            // Let's just apply rotation.
            // Using Space.Self to rotate camera locally.
            transform.Rotate(-rotationInput.y, rotationInput.x, 0, Space.Self);

            // Stabilize Z roll?
            Vector3 euler = transform.localEulerAngles;
            euler.z = 0;
            transform.localEulerAngles = euler;
        }
    }
}
