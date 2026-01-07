using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// First-person controller for player movement and camera look
/// Handles WASD movement, mouse look, jumping, and sprinting
/// Uses Unity's new Input System
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    
    [Header("Look Settings")]
    public float mouseSensitivity = 2f;
    public float lookXLimit = 90f;
    
    [Header("References")]
    public Camera playerCamera;
    
    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private float rotationX = 0;
    
    // Input System
    private GameInputActions inputActions;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isSprinting;
    private bool jumpPressed;
    
    private void Awake()
    {
        inputActions = new GameInputActions();
    }
    
    private void OnEnable()
    {
        inputActions.Player.Enable();
        
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Look.canceled += OnLook;
        inputActions.Player.Sprint.performed += OnSprint;
        inputActions.Player.Sprint.canceled += OnSprint;
        inputActions.Player.Jump.performed += OnJump;
    }
    
    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Look.performed -= OnLook;
        inputActions.Player.Look.canceled -= OnLook;
        inputActions.Player.Sprint.performed -= OnSprint;
        inputActions.Player.Sprint.canceled -= OnSprint;
        inputActions.Player.Jump.performed -= OnJump;
        
        inputActions.Player.Disable();
    }
    
    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    
    private void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    
    private void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.performed;
    }
    
    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
        }
    }
    
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        
        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // If no camera is assigned, try to find one
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }
    }
    
    private void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }
    
    private void HandleMovement()
    {
        // Check if grounded
        isGrounded = characterController.isGrounded;
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        // Calculate movement direction using new input
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        
        // Determine speed (sprint or walk)
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        
        // Move the character
        characterController.Move(move * currentSpeed * Time.deltaTime);
        
        // Jumping
        if (jumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        jumpPressed = false;
        
        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    
    private void HandleMouseLook()
    {
        if (playerCamera == null) return;
        
        // Get mouse input from new Input System
        float mouseX = lookInput.x * mouseSensitivity * 0.01f;
        float mouseY = lookInput.y * mouseSensitivity * 0.01f;
        
        // Rotate camera up/down
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        
        // Rotate player left/right
        transform.rotation *= Quaternion.Euler(0, mouseX, 0);
    }
}
