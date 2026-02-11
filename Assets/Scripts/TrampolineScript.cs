using UnityEngine;

public class TrampolineScript : MonoBehaviour
{
    public float bounceForce = 10f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player_Feet"))
        {
            Rigidbody2D rb = other.attachedRigidbody;
            rb.linearVelocityY = bounceForce;
        }
    }
}
