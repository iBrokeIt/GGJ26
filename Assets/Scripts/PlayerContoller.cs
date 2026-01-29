using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerContoller : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    private bool isGrounded = false;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    void Update()
    {
        // If no InputAction assigned, fall back to keyboard/gamepad probing
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveInput.x, 0) * moveSpeed;
        float jumpInput = jumpAction.action.ReadValue<float>();
        if (jumpInput > 0.1f && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

}
