using UnityEngine;

public class AccelerometerRotationProvider : IInputProvider {

    private Vector3 lastAccel;

    public AccelerometerRotationProvider() {
        lastAccel = Input.acceleration;
    }

    public string GetProviderName() {
        return "Accelerometer Rotation";
    }

    public Vector3 GetMovementInput() {
        // Accelerometer can be used for movement (tilt), but we are focusing on Rotation per user request.
        // But to be "Full Featured", we might as well return it if we wanted, but let's stick to Rotation focus.
        return Vector3.zero;
    }

    public Vector2 GetRotationInput() {
        Vector3 currentAccel = Input.acceleration;
        Vector3 delta = currentAccel - lastAccel;
        lastAccel = currentAccel;

        // Delta acceleration implies a change in tilt (rotation).
        // Accel X change -> Roll/Yaw change?
        // If phone tilts left/right (X change), we want to Rotate Y (Yaw) or Z (Roll)?
        // For camera control, Yaw (Y axis) is usually desired for "looking around".

        // Accel Y/Z change -> Pitch (X axis) change.

        // Sensitivity
        float sensitivity = 50.0f; // Accel values are small (0-1g).

        return new Vector2(delta.x, delta.y) * sensitivity;
    }

    public bool IsActive() {
        // Always active if moving?
        return GetRotationInput().magnitude > 0.01f;
    }
}
