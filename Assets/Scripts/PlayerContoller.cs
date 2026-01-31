using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerContoller : MonoBehaviour
{
    public AudioClip stepSound; // TODO: remove
    public float moveSpeed = 5f;
    public float jumpForce;
    public float jumpAssist;
    public float jumpDuration = 0.35f;
    public float stepCooldown = 0.35f;
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference interactAction;
    private bool isGrounded;
    private bool isJumping;
    private float jumpTimeCounter;
    private float lastStepTime;
    public bool allowUp;
    public float xOffsetFlip;
    private bool isFlipped;

    Rigidbody2D rb;

    Rigidbody2D platformRb;
    [System.NonSerialized]
    public bool isGrabbingRope;
    Animator animator;
    float meanSpeed;


    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterPlayer(this.gameObject);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        isGrounded = false;
        isJumping = false;
        platformRb = null;
        isGrabbingRope = false;
        isFlipped = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
            isJumping = false;
            platformRb = collision.gameObject.GetComponent<Rigidbody2D>();
            animator.SetTrigger("hitFloor");
            animator.ResetTrigger("jumping");
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
                animator.SetTrigger("jumping");
                animator.ResetTrigger("hitFloor");
            }
            platformRb = null;
        }
    }

    void FixedUpdate()
    {
        float speed = 0;
        if (!isGrabbingRope) {
            GroundMovement();
            if (isGrounded) {
                speed = rb.linearVelocityX;
                if (platformRb != null) {
                    speed = speed - platformRb.linearVelocityX;
                }
                speed = Mathf.Abs(speed);
            }
        }
        // Moving average for smoother animation transitions, prevent idle break when flipping
        meanSpeed = meanSpeed * 0.67f + speed * 0.33f;
        animator.SetFloat("speed", meanSpeed);
        // Flip sprite based on movement direction, but only if there is some movement
        if (rb.linearVelocityX != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(rb.linearVelocityX) * Mathf.Abs(scale.x);
            transform.localScale = scale;
            Vector3 pos = transform.localPosition;
            pos.x += Mathf.Sign(rb.linearVelocityX) * xOffsetFlip;
            transform.localPosition = pos;
        }
        
        animator.SetFloat("verticalSpeed", rb.linearVelocityY);


        if (allowUp && IsGoUp())
        {
            rb.linearVelocityY = jumpForce;
        }
    }
    void GroundMovement()
    {
        // If no InputAction assigned, fall back to keyboard/gamepad probing
        float baseVelocityX = GetHorizontalDirection() * moveSpeed;
        if(Mathf.Abs(baseVelocityX) > 0.1f && isGrounded)
        {
            PlayWalkingSound();
        }

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
        if (moveAction == null || moveAction.action == null)
        {
            Debug.LogWarning("Move Action is missing on " + gameObject.name);
            return 0f;
        }
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        return moveInput.x;
    }

    public bool IsGoUp()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        return moveInput.y > 0.1f;
    }

  

    private void PlayWalkingSound()
    {
        if (Time.time - lastStepTime > stepCooldown)
        {
            AudioManager.Instance.PlayRandomizedSFX(stepSound);
            lastStepTime = Time.time;
        }
    }

}
