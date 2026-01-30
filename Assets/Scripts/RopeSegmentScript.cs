using UnityEngine;

public class RopeSegmentScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody2D rb;
    GameObject player;
    bool wasJumpPressedLastFrame;
    float gravityReference;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        wasJumpPressedLastFrame = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            PlayerContoller playerContoller = player.GetComponent<PlayerContoller>();
            bool isJumpPressed = playerContoller.IsJumpPressed();
            wasJumpPressedLastFrame = isJumpPressed && wasJumpPressedLastFrame;
            if (!wasJumpPressedLastFrame && isJumpPressed)
            {
                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                playerRb.gravityScale = gravityReference;
                playerRb.linearVelocity = rb.linearVelocity;
                player.GetComponent<PlayerContoller>().isGrabbingRope = false;
                player = null;
            }
            if (player != null) {
                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                float horizontalSpeed = playerContoller.GetHorizontalSpeed();
                rb.AddForce(new Vector2(horizontalSpeed, 0), ForceMode2D.Impulse);
                playerRb.MovePosition(rb.position);
            }

        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") 
        && other.GetComponent<PlayerContoller>().jumpAction.action.IsPressed()) {
            player = other.gameObject;
            Debug.Log("Player grabbed rope");
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            gravityReference = playerRb.gravityScale;
            playerRb.gravityScale = 0;
            playerRb.linearVelocityY = 0;
            wasJumpPressedLastFrame = true;
            other.GetComponent<PlayerContoller>().isGrabbingRope = true;
        }
    }
}
