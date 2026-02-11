using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerContoller : MonoBehaviour
{
    public AudioClip stepSound; // TODO: remove
    public float moveSpeed = 5f;
    public float jumpForce;
    public float coyoteTime = 0.08f;
    public float jumpBufferTime = 0.1f;
    public float jumpCutMultiplier = 0.4f;
    public float stepCooldown = 0.35f;
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference interactAction;
    public GroundCheck groundCheck;
    private bool isJumping;
    private bool wasGrounded;
    private bool wasJumpPressed;
    private float coyoteTimer;
    private float jumpBufferTimer;
    private float lastStepTime;
    public bool allowUp;
    public float xOffsetFlip;
    private float direction;

    Rigidbody2D rb;
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
        isJumping = false;
        wasGrounded = false;
        isGrabbingRope = false;
        direction = 1f;
    }

    void FixedUpdate()
    {
        bool isGrounded = groundCheck.IsGrounded;
        Rigidbody2D platformRb = groundCheck.PlatformRb;

        // Detect landing for animations
        if (!wasGrounded && isGrounded)
        {
            isJumping = false;
            animator.SetTrigger("hitFloor");
            animator.ResetTrigger("jumping");
        }
        wasGrounded = isGrounded;

        float speed = 0;
        if (!isGrabbingRope) {
            GroundMovement(isGrounded, platformRb);
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
        float currDirection = Mathf.Sign(rb.linearVelocityX);
        if (Mathf.Abs(rb.linearVelocityX) > 0.1f && currDirection != direction)
        {
            direction = currDirection;
            Vector3 scale = transform.localScale;
            scale.x = direction * Mathf.Abs(scale.x);
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
    void GroundMovement(bool isGrounded, Rigidbody2D platformRb)
    {
        float baseVelocityX = GetHorizontalDirection() * moveSpeed;
        if(Mathf.Abs(baseVelocityX) > 0.1f && isGrounded)
        {
            PlayWalkingSound();
        }

        if (platformRb != null) {
            baseVelocityX += platformRb.linearVelocityX;
        }
        rb.linearVelocityX = baseVelocityX;

        // --- Jump ---
        bool jumpPressed = IsJumpPressed();
        bool jumpPressedThisFrame = jumpPressed && !wasJumpPressed;
        wasJumpPressed = jumpPressed;

        // Coyote time: refresh while grounded, tick down while airborne
        if (isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.fixedDeltaTime;

        // Jump buffer: set on press, tick down each frame
        if (jumpPressedThisFrame)
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.fixedDeltaTime;

        // Initiate jump when buffer and coyote are both active
        if (jumpBufferTimer > 0 && coyoteTimer > 0 && !isJumping)
        {
            isJumping = true;
            rb.linearVelocityY = jumpForce;
            jumpBufferTimer = 0;
            coyoteTimer = 0;
            animator.SetTrigger("jumping");
            animator.ResetTrigger("hitFloor");
        }

        // Variable jump height: cut upward velocity on early release
        if (!jumpPressed && isJumping && rb.linearVelocityY > 0)
        {
            rb.linearVelocityY *= jumpCutMultiplier;
            isJumping = false;
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

    public void DisableInput()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        interactAction.action.Disable();
    }

}
