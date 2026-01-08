using UnityEngine;

public class GyroscopeRotationProvider : IInputProvider {

    private bool initialized = false;

    public GyroscopeRotationProvider() {
        if (SystemInfo.supportsGyroscope) {
            Input.gyro.enabled = true;
            initialized = true;
        }
    }

    public string GetProviderName() {
        return "Gyroscope Rotation";
    }

    public Vector3 GetMovementInput() {
        return Vector3.zero;
    }

    public Vector2 GetRotationInput() {
        if (!initialized) return Vector2.zero;

        // Rate in rad/s -> deg/frame
        float rateX = Input.gyro.rotationRateUnbiased.x * Mathf.Rad2Deg * Time.deltaTime;
        float rateY = Input.gyro.rotationRateUnbiased.y * Mathf.Rad2Deg * Time.deltaTime;

        // Sensitivity matching
        float sensitivity = 2.0f;

        // Map Device X (Pitch) -> Camera Pitch (Y output of Vector2, which rotates X axis)
        // Map Device Y (Twist/Yaw) -> Camera Yaw (X output of Vector2, which rotates Y axis)
        // Note: Vector2(x,y) usually means (Horizontal, Vertical) inputs.
        // Horizontal Input -> Rotates Y axis (Yaw).
        // Vertical Input -> Rotates X axis (Pitch).

        // Gyro Y is Yaw. Gyro X is Pitch.
        // gyro.y -> Vector2.x
        // gyro.x -> Vector2.y

        return new Vector2(-rateY, -rateX) * sensitivity;
    }

    public bool IsActive() {
        return initialized && GetRotationInput().magnitude > 0.01f;
    }
}
