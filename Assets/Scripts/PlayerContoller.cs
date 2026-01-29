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
    private bool isGrounded;
    private bool isJumping;
    private float jumpTimeCounter;

    Rigidbody2D rb;

    Rigidbody2D platformRb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isGrounded = false;
        isJumping = false;
        platformRb = null;
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
        // If no InputAction assigned, fall back to keyboard/gamepad probing
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        float baseVelocityX = moveInput.x * moveSpeed;
        if (platformRb != null) {
            baseVelocityX += platformRb.linearVelocityX;
        }
        rb.linearVelocityX = baseVelocityX;
        float jumpInput = jumpAction.action.ReadValue<float>();
        if (jumpInput > 0.1f && isGrounded && !isJumping)
        {
            isJumping = true;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        if (jumpTimeCounter > 0 && jumpInput > 0.1f)
        {
            rb.AddForce(Vector2.up * jumpAssist * Time.deltaTime, ForceMode2D.Impulse);
            jumpTimeCounter -= Time.deltaTime;
        }
    }

}
