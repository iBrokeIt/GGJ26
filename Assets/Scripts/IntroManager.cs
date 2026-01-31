using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{

    [SerializeField] private Transform storyText;
    public float endYPosition = 5116f;
    public Button skipButton;
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
        Debug.Log("Skip Intro");
        if (!GameManager.Instance)
        {
            Debug.LogError("GameManager instance not found!");
            return;
        }
        GameManager.Instance.StartGame();
    }
}
