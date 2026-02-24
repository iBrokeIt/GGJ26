using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialEndZone : MonoBehaviour
{
    [SerializeField] float transitionDelay =1f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke("LoadScene", transitionDelay);
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("Main");
    }
    
}
