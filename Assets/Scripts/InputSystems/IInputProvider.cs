using UnityEngine;

public interface IInputProvider {
    string GetProviderName();
    Vector3 GetMovementInput();
    Vector2 GetRotationInput();
    bool IsActive();
}
