using UnityEngine;

public class GameBootstrapper : MonoBehaviour {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init() {
        if (AdvancedInputManager.Instance == null) {
            GameObject go = new GameObject("AdvancedInputManager");
            go.AddComponent<AdvancedInputManager>();
            DontDestroyOnLoad(go);
        }
    }
}
