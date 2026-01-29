using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerContoller : MonoBehaviour
{
    public float moveSpeed = 5f;

    public InputActionReference moveAction;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // If no InputAction assigned, fall back to keyboard/gamepad probing
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveInput.x, 0) * moveSpeed;
    }

}
