using UnityEngine;
using System.Collections.Generic;

public class AdvancedInputManager : MonoBehaviour {

    public static AdvancedInputManager Instance;

    private List<IInputProvider> providers = new List<IInputProvider>();

    public float sensitivity = 1.0f;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeProviders();
        } else {
            Destroy(gameObject);
        }
    }

    void InitializeProviders() {
        providers.Add(new MouseInputProvider());
        providers.Add(new KeyboardInputProvider());
        providers.Add(new GyroscopeRotationProvider());
        providers.Add(new AccelerometerRotationProvider());
        providers.Add(new CompassRotationProvider());
        providers.Add(new TouchInputProvider());
    }

    public Vector2 GetCompositeRotation() {
        Vector2 totalRotation = Vector2.zero;
        int activeCount = 0;

        foreach (var p in providers) {
            // Only count if it's contributing to rotation
            Vector2 rot = p.GetRotationInput();
            if (p.IsActive() && rot.sqrMagnitude > 0.0001f) {
                activeCount++;
                totalRotation += rot;
            }
        }

        if (activeCount == 0) return Vector2.zero;

        // "Cumulative input gets more fine"
        // Precision Factor = 1 / Active Count
        // 1 provider = 100% sensitivity
        // 2 providers = 50% sensitivity
        // 3 providers = 33% sensitivity

        float precisionFactor = 1.0f / activeCount;

        return totalRotation * precisionFactor * sensitivity;
    }

    public Vector3 GetCompositeMovement() {
        Vector3 totalMovement = Vector3.zero;
        int activeCount = 0;

        // We apply similar logic to movement for consistency,
        // though the user emphasized Rotation.
        foreach (var p in providers) {
            Vector3 mov = p.GetMovementInput();
            if (p.IsActive() && mov.sqrMagnitude > 0.0001f) {
                activeCount++;
                totalMovement += mov;
            }
        }

        if (activeCount == 0) return Vector3.zero;

        float precisionFactor = 1.0f / activeCount;
        return totalMovement * precisionFactor * sensitivity;
    }

    public int GetActiveProviderCount() {
        int count = 0;
        foreach(var p in providers) if(p.IsActive()) count++;
        return count;
    }
}
