using UnityEngine;

public class CloudSounds : MonoBehaviour
{
   public AudioClip cloudJumpSound;
   public BoxCollider2D soundTrigger;

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlayRandomizedSFX(cloudJumpSound);
        }
    }
}
