using UnityEngine;

public class KeyboardInputProvider : IInputProvider {
    public string GetProviderName() {
        return "Keyboard Input";
    }

    public Vector3 GetMovementInput() {
        return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    public Vector2 GetRotationInput() {
        return Vector2.zero;
    }

    public bool IsActive() {
        return GetMovementInput().magnitude > 0.01f;
    }
}
