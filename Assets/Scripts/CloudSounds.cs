using UnityEngine;

public class CloudSounds : MonoBehaviour
{
   public AudioClip cloudJumpSound;
   public BoxCollider2D soundTrigger;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player_Feet"))
        {
            AudioManager.Instance.PlayRandomizedSFX(cloudJumpSound);
        }
    }
}
