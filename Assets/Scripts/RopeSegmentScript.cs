using System;
using Unity.Mathematics;
using UnityEngine;

public class RopeSegmentScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody2D rb;
    GameObject player;
    bool wasJumpPressedLastFrame;
    float gravityReference;
    public float momentumX;

    
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
        }
    }

    void FixedUpdate()
    {
        if (player != null) {
            PlayerContoller playerContoller = player.GetComponent<PlayerContoller>();
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            float horizontalDirection = playerContoller.GetHorizontalDirection();
            // bool isRopeFalling = rb.linearVelocityY < -1e-4f;
            // bool isRopeMovingInOppositeDirection = Math.Sign(rb.linearVelocityX) != Math.Sign(horizontalSpeed);
            // bool isRopeXStationary = Math.Abs(rb.linearVelocityX) < 1e-4f;
            
            // if (!isRopeFalling && (isRopeMovingInOppositeDirection || isRopeXStationary)) {
            //     Vector2 ropeNorm = rb.linearVelocity.normalized;
            //     if (rb.linearVelocity.sqrMagnitude < 1e-4f) {
            //         ropeNorm = new Vector2(-horizontalSpeed, 0);
            //     }
            Vector2 force = new Vector2(horizontalDirection * momentumX, 0);
            Debug.Log("Applying rope force: " + force);
            rb.AddForce(force, ForceMode2D.Impulse);
            // }
            playerRb.MovePosition(rb.position);
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
