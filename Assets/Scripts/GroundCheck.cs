using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private int groundContactCount;
    private Rigidbody2D currentPlatformRb;

    public bool IsGrounded => groundContactCount > 0;
    public Rigidbody2D PlatformRb => currentPlatformRb;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("GroundCheck trigger entered by: " + other.name);
        if (other.CompareTag("Floor"))
        {
            Debug.Log("GroundCheck detected floor contact with: " + other.name);    
            groundContactCount++;
            currentPlatformRb = other.attachedRigidbody;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("GroundCheck trigger exited by: " + other.name);
        if (other.CompareTag("Floor"))
        {
            groundContactCount--;
            if (groundContactCount <= 0)
            {
                Debug.Log("GroundCheck lost all floor contacts");
                groundContactCount = 0;
                currentPlatformRb = null;
            }
        }
    }
}
