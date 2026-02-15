using UnityEngine;

/// <summary>
/// Automatically loads missing manager prefabs when entering Play mode from any scene.
/// No setup needed — runs before any scene's Awake() via RuntimeInitializeOnLoadMethod.
/// </summary>
public static class DevBootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Bootstrap()
    {
#if UNITY_EDITOR
        if (Object.FindAnyObjectByType<GameManager>() == null)
        {
            var go = new GameObject("GameManager (DevBoot)");
            go.AddComponent<GameManager>();
            Object.DontDestroyOnLoad(go);
            Debug.Log("[DevBootstrapper] Created missing GameManager");
        }

        LoadPrefabIfMissing<AudioManager>("Assets/Prefabs/MANAGERS.prefab");
#endif
    }

    static void LoadPrefabIfMissing<T>(string prefabPath) where T : Object
    {
        if (Object.FindAnyObjectByType<T>() != null) return;

#if UNITY_EDITOR
        var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefab != null)
        {
            var go = Object.Instantiate(prefab);
            go.name = prefab.name + " (DevBoot)";
            Object.DontDestroyOnLoad(go);
            Debug.Log($"[DevBootstrapper] Loaded missing {typeof(T).Name} from {prefabPath}");
        }
        else
        {
            Debug.LogWarning($"[DevBootstrapper] Prefab not found at {prefabPath}");
        }
#endif
    }
}
