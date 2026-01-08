using UnityEngine;
using System.Collections.Generic;

public class AdvancedInputManager : MonoBehaviour {

    public static AdvancedInputManager Instance;

    private List<IInputProvider> providers = new List<IInputProvider>();

    public float sensitivity = 1.0f;
    public float fineControlMultiplier = 0.5f; // Factor to reduce sensitivity per active provider

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
        providers.Add(new GyroInputProvider());
        providers.Add(new TouchInputProvider());
    }

    public Vector2 GetCompositeRotation() {
        Vector2 totalRotation = Vector2.zero;
        int activeCount = 0;

        foreach (var p in providers) {
            if (p.IsActive()) {
                activeCount++;
                totalRotation += p.GetRotationInput();
            }
        }

        if (activeCount == 0) return Vector2.zero;

        // Apply "fine control" logic
        // If 1 input: multiplier = 1
        // If 2 inputs: multiplier = 0.5 (finer control)
        // If 3 inputs: multiplier = 0.33...
        // Formula: 1.0 / (1 + (activeCount - 1) * 2) maybe?
        // Let's stick to simple: 1.0 / activeCount for averaging, OR
        // User request: "cumulative input gets more fine"

        // Interpretation:
        // Input 1 (Mouse) gives delta 10.
        // Input 2 (Gyro) gives delta 5.
        // Sum = 15.
        // If we just sum, it gets FASTER.
        // "More fine" implies it should be SLOWER or more PRECISE.

        // Let's divide the sensitivity by the active count.
        float precisionFactor = 1.0f / activeCount;

        // Also, apply global sensitivity
        return totalRotation * precisionFactor * sensitivity;
    }

    public Vector3 GetCompositeMovement() {
        Vector3 totalMovement = Vector3.zero;
        int activeCount = 0;

        foreach (var p in providers) {
            if (p.IsActive()) {
                activeCount++;
                totalMovement += p.GetMovementInput();
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
