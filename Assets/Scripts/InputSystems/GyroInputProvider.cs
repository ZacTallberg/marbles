using UnityEngine;

public class GyroInputProvider : IInputProvider {

    private bool initialized = false;

    public GyroInputProvider() {
        if (SystemInfo.supportsGyroscope) {
            Input.gyro.enabled = true;
            initialized = true;
        }
    }

    public string GetProviderName() {
        return "Gyroscope Input";
    }

    public Vector3 GetMovementInput() {
        if (!initialized) return Vector3.zero;
        Vector3 accel = Input.acceleration;
        return new Vector3(accel.x, 0, accel.y);
    }

    public Vector2 GetRotationInput() {
        if (!initialized) return Vector2.zero;

        // Input.gyro.rotationRateUnbiased is in radians per second.
        // We need to convert this to "degrees per frame" to match how we use it (transform.Rotate).

        // 1. Convert to degrees/sec: * Mathf.Rad2Deg
        // 2. Convert to degrees/frame: * Time.deltaTime

        float rateX = Input.gyro.rotationRateUnbiased.x * Mathf.Rad2Deg * Time.deltaTime;
        float rateY = Input.gyro.rotationRateUnbiased.y * Mathf.Rad2Deg * Time.deltaTime;

        // Apply a sensitivity scaling to match Mouse feel.
        // Mouse "pixels" are arbitrary, but usually range 0.1 to 10 per frame.
        // Gyro degrees/frame at normal rotation speed (e.g. 90 deg/sec) is 90 * 0.016 = 1.5 deg/frame.
        // They are roughly comparable in magnitude (1-5 range).

        float sensitivity = 2.0f;

        // Mapping:
        // Device Y rotation (twist) -> Camera Yaw (X output)
        // Device X rotation (tilt) -> Camera Pitch (Y output)
        // Signs need to be checked. Usually tilting forward (positive X rate) should look down (Positive pitch? Or negative?).
        // If I look down, Camera X angle increases (0 -> 90).
        // Device tilt forward -> Positive X rate.
        // So +RateX -> +OutputY (Pitch).
        // Let's assume standard mapping:

        return new Vector2(rateY, -rateX) * sensitivity;
    }

    public bool IsActive() {
        // Threshold check
        return initialized && (GetMovementInput().magnitude > 0.05f || GetRotationInput().magnitude > 0.1f);
    }
}
