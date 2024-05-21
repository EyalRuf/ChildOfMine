using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // References to components
    [SerializeField] private Animator animator;  // Commented out usage below for now
    [SerializeField] private Transform groundCheck;  // Transform to check for ground
    [SerializeField] private LayerMask groundLayer;  // Layer of the ground

    private PlayerInputActions playerInputActions;

    // Movement parameters
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float gravity = -9.8f;
    public float groundCheckRadius = 0.2f;

    private Vector2 moveInput;
    private bool isJumping = false;
    private bool isAttacking = false;
    private bool isGrounded = false;
    private float verticalVelocity = 0f;

    // Animation constants
    // private static readonly int AnimMove = Animator.StringToHash("Move");
    // private static readonly int AnimJump = Animator.StringToHash("Jump");
    // private static readonly int AnimAttack = Animator.StringToHash("Attack");

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();

        // Bind input actions
        playerInputActions.Player.Move.performed += OnMove;
        playerInputActions.Player.Move.canceled += OnMove;
        playerInputActions.Player.Jump.performed += OnJump;
        playerInputActions.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();

        // Unbind input actions
        playerInputActions.Player.Move.performed -= OnMove;
        playerInputActions.Player.Move.canceled -= OnMove;
        playerInputActions.Player.Jump.performed -= OnJump;
        playerInputActions.Player.Attack.performed -= OnAttack;
    }

    private void Update()
    {
        HandleGravity();
        HandleMovement();
        HandleGroundDetection();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (!isJumping && !isAttacking)
        {
            moveInput = context.ReadValue<Vector2>();
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!isJumping && isGrounded && !isAttacking)
        {
            isJumping = true;
            verticalVelocity = jumpForce;
            // animator.SetTrigger(AnimJump);
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (!isAttacking && !isJumping)
        {
            isAttacking = true;
            // animator.SetTrigger(AnimAttack);
        }
    }

    private void HandleMovement()
    {
        if (isJumping || isAttacking)
        {
            moveInput = Vector2.zero;  // Restrict movement during jump or attack
        }

        Vector3 move = new Vector3(moveInput.x * moveSpeed, verticalVelocity, 0f) * Time.deltaTime;
        transform.Translate(move);

        // animator.SetFloat(AnimMove, moveInput.magnitude);

        if (isJumping && verticalVelocity <= 0 && isGrounded)
        {
            isJumping = false;
        }

        if (isAttacking)
        {
            // isAttacking = false;
        }
    }

    private void HandleGravity()
    {
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        else if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = 0f;
        }
    }

    private void HandleGroundDetection()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
