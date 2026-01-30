using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerContoller : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce;
    public float jumpAssist;

    public float jumpDuration = 0.35f;

    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference interactAction;
    private bool isGrounded;
    private bool isJumping;
    private float jumpTimeCounter;
    public bool allowUp;

    Rigidbody2D rb;

    Rigidbody2D platformRb;
    [System.NonSerialized]
    public bool isGrabbingRope;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isGrounded = false;
        isJumping = false;
        platformRb = null;
        isGrabbingRope = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
            isJumping = false;
            platformRb = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
            if (isJumping)
            {
                jumpTimeCounter = jumpDuration;
            }
            platformRb = null;
        }
    }

    void FixedUpdate()
    {
        if (!isGrabbingRope) {
            GroundMovement();
        }
        if (allowUp && IsGoUp())
        {
            rb.linearVelocityY = jumpForce;
        }
    }
    void GroundMovement()
    {
        // If no InputAction assigned, fall back to keyboard/gamepad probing
        float baseVelocityX = GetHorizontalDirection() * moveSpeed;
        if (platformRb != null) {
            baseVelocityX += platformRb.linearVelocityX;
        }
        rb.linearVelocityX = baseVelocityX;
        bool isJumpPressed = IsJumpPressed();
        if (isJumpPressed && isGrounded && !isJumping)
        {
            isJumping = true;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        if (jumpTimeCounter > 0 && isJumpPressed)
        {
            rb.AddForce(Vector2.up * jumpAssist * Time.deltaTime, ForceMode2D.Impulse);
            jumpTimeCounter -= Time.deltaTime;
        }
    }

    public bool IsInteracting()
    {
        float interactInput = interactAction.action.ReadValue<float>();
        return interactInput > 0.1f;
    }

    public bool IsJumpPressed()
    {
        float jumpInput = jumpAction.action.ReadValue<float>();
        return jumpInput > 0.1f;
    }

    public float GetHorizontalDirection()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        return moveInput.x;
    }

    public bool IsGoUp()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        return moveInput.y > 0.1f;
    }

}
