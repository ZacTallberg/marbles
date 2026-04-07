using UnityEngine;

public class TouchInputProvider : IInputProvider {
    public string GetProviderName() {
        return "Touch Input";
    }

    public Vector3 GetMovementInput() {
        // Virtual Joystick logic could go here, but for now let's just use average touch position offset from center?
        // Or leave it zero for now as this is more for rotation in this context.
        return Vector3.zero;
    }

    public Vector2 GetRotationInput() {
        if (Input.touchCount > 0) {
            Vector2 delta = Vector2.zero;
            foreach (Touch t in Input.touches) {
                if (t.phase == TouchPhase.Moved) {
                    delta += t.deltaPosition;
                }
            }
            return delta / Input.touchCount; // Average delta
        }
        return Vector2.zero;
    }

    public bool IsActive() {
        return Input.touchCount > 0;
    }
}
