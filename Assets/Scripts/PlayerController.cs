using UnityEngine;

/// <summary>
/// First-person controller for player movement and camera look
/// Handles WASD movement, mouse look, jumping, and sprinting
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
        
        // Get input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        
        // Calculate movement direction
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        
        // Determine speed (sprint or walk)
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        
        // Move the character
        characterController.Move(move * currentSpeed * Time.deltaTime);
        
        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    
    private void HandleMouseLook()
    {
        if (playerCamera == null) return;
        
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        // Rotate camera up/down
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        
        // Rotate player left/right
        transform.rotation *= Quaternion.Euler(0, mouseX, 0);
    }
}
