using TMPro;
using UnityEngine;

public class IntroManager : MonoBehaviour
{

    [SerializeField] private Transform storyText;
    public float endYPosition = 5116f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(storyText.position.y >= endYPosition)
            onClickSkip();
    }

    public void onClickSkip()
    {
        if(!GameManager.Instance)
        {
            Debug.LogError("GameManager instance not found!");
            return;
        }
        GameManager.Instance.StartGame();
    }
}
