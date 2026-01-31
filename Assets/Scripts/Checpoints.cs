using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public GameObject flame;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Checkpoint trigger entered");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Checkpoint reached at position: " + transform.position);
            GameManager.Instance.UpdateCheckpoint(transform.position);
            flame.SetActive(true);
        }
    }
}
