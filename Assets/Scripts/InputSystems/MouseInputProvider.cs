using UnityEngine;

public class MouseInputProvider : IInputProvider {
    public string GetProviderName() {
        return "Mouse Input";
    }

    public Vector3 GetMovementInput() {
        return Vector3.zero;
    }

    public Vector2 GetRotationInput() {
        // Return Delta
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    public bool IsActive() {
        return GetRotationInput().magnitude > 0.01f;
    }
}
