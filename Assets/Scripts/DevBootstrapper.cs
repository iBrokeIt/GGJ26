using UnityEngine;

/// <summary>
/// Editor-only: creates missing singletons when hitting Play on any scene.
/// Does nothing if the singletons already exist (e.g., when coming from Menu).
/// </summary>
public static class DevBootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
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

        if (Object.FindAnyObjectByType<AudioManager>() == null)
        {
            var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/MANAGERS.prefab");
            if (prefab != null)
            {
                var go = Object.Instantiate(prefab);
                go.name = "MANAGERS (DevBoot)";
                Object.DontDestroyOnLoad(go);
                Debug.Log("[DevBootstrapper] Loaded missing MANAGERS prefab");
            }
        }
#endif
    }
}
