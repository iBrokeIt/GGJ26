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

        //if (UnityEngine.EventSystems.EventSystem.current != null)
        //{
        //    UnityEngine.EventSystems.EventSystem.current.enabled = false;
        //    UnityEngine.EventSystems.EventSystem.current.enabled = true;
        //}

        //if (skipButton != null)
        //{
        //    skipButton.onClick.RemoveAllListeners();
        //    skipButton.onClick.AddListener(onClickSkip);
        //    Debug.Log("Button link established in Start");
        //}
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
