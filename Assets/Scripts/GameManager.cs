using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Singleton")]
    public static GameManager Instance;

    [Header("Configuration")]
    public InputActionReference resetAction;
    [SerializeField] GameObject player;

    private Vector2 lastCheckpointPos;
    void Awake()
    {
        // Singleton Setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            lastCheckpointPos = Vector2.zero;
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void UpdateCheckpoint(Vector2 newPos)
    {
        lastCheckpointPos = newPos;
    }

    public void Respawn()
    {
        player.transform.position = lastCheckpointPos;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void Update()
    {
        if (resetAction.action.IsPressed())
        {
            Respawn();
            DimensionSwitcher.Instance.SwitchToDimension(0);
        }
    }
}
