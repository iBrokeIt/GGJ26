using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Singleton")]
    public static GameManager Instance;

    [Header("Configuration")]
    public InputActionReference resetAction;


    private GameObject CurrentPlayer;
    private Vector2 lastCheckpointPos;
    private AudioSource buttonSoundEffect;
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

        buttonSoundEffect = GetComponent<AudioSource>();
        
    }

    public void RegisterPlayer(GameObject player)
    {
        CurrentPlayer = player;
        Debug.Log("Player registered in GameManager!");
    }

    public void UpdateCheckpoint(Vector2 newPos)
    {
        lastCheckpointPos = newPos;
    }

    public void Respawn()
    {
        CurrentPlayer.transform.position = lastCheckpointPos;

        Rigidbody2D rb = CurrentPlayer.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void Update()
    {
        if (CurrentPlayer && resetAction?.action?.IsPressed() == true)
        {
            Respawn();
            DimensionSwitcher.Instance.SwitchToDimension(0);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
        PlayClickSound();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
        PlayClickSound();
    }

    public void GoToHotKeys()
    {
        SceneManager.LoadScene("HotKeys");
        PlayClickSound();
    }

    public void StartTutorial()
    {
        Debug.Log("Starting Tutorial...");
        SceneManager.LoadScene("Tutorial");
        PlayClickSound();
    }

    public void PlayClickSound()
    {
        if (buttonSoundEffect != null)
        {
            buttonSoundEffect.Play();
        }
    }
}
