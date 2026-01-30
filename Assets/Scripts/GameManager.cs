using NUnit.Framework;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Vector2 lastCheckpointPos;
    void Awake()
    {
        // Singleton Setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
