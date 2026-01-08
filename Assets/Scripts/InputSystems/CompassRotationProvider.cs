using UnityEngine;

public class CompassRotationProvider : IInputProvider {

    private float lastHeading;
    private bool initialized = false;

    public CompassRotationProvider() {
        // Start location service to enable compass? Not always strictly necessary but often good practice.
        // Input.compass.enabled = true;
        Input.compass.enabled = true;
        initialized = true;
        lastHeading = Input.compass.magneticHeading;
    }

    public string GetProviderName() {
        return "Compass Rotation";
    }

    public Vector3 GetMovementInput() {
        return Vector3.zero;
    }

    public Vector2 GetRotationInput() {
        if (!initialized) return Vector2.zero;

        float currentHeading = Input.compass.magneticHeading;
        float delta = currentHeading - lastHeading;

        // Handle wrapping (0 <-> 360)
        if (delta > 180) delta -= 360;
        if (delta < -180) delta += 360;

        lastHeading = currentHeading;

        // Heading changes Yaw (Y axis rotation).
        // Vector2 X output -> Yaw.

        // Sensitivity
        float sensitivity = 1.0f; // Degrees are degrees.

        return new Vector2(delta * sensitivity, 0);
    }

    public bool IsActive() {
        return initialized && Mathf.Abs(GetRotationInput().x) > 0.1f;
    }
}
