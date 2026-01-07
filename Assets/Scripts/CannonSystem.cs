using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages cannon firing mechanics
/// Player can interact with cannons to fire at enemies
/// Uses Unity's new Input System
/// </summary>
public class CannonSystem : MonoBehaviour
{
    [Header("Cannon Settings")]
    public float damage = 100f;
    public float explosionRadius = 5f;
    public float range = 50f;
    public float reloadTime = 3f;
    
    [Header("Projectile")]
    public GameObject cannonballPrefab;
    public float projectileSpeed = 30f;
    
    [Header("References")]
    public Transform firePoint;
    public Transform aimPivot; // The part that rotates for aiming
    
    [Header("Interaction")]
    public float interactionRange = 3f;
    public float aimSensitivity = 0.1f;
    
    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public GameObject explosionEffect;
    public AudioClip fireSound;
    
    private bool isPlayerUsing = false;
    private bool isReloading = false;
    private Transform player;
    private PlayerController playerController;
    
    // Input System
    private GameInputActions inputActions;
    private Vector2 aimInput;
    private bool firePressed;
    private bool interactPressed;
    private bool exitPressed;
    
    private void Awake()
    {
        inputActions = new GameInputActions();
    }
    
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Cannon.Enable();
        
        // Player action map for interaction when not using cannon
        inputActions.Player.Interact.performed += OnInteract;
        
        // Cannon action map for when using cannon
        inputActions.Cannon.Aim.performed += OnAim;
        inputActions.Cannon.Aim.canceled += OnAim;
        inputActions.Cannon.Fire.performed += OnFire;
        inputActions.Cannon.Exit.performed += OnExit;
    }
    
    private void OnDisable()
    {
        inputActions.Player.Interact.performed -= OnInteract;
        
        inputActions.Cannon.Aim.performed -= OnAim;
        inputActions.Cannon.Aim.canceled -= OnAim;
        inputActions.Cannon.Fire.performed -= OnFire;
        inputActions.Cannon.Exit.performed -= OnExit;
        
        inputActions.Player.Disable();
        inputActions.Cannon.Disable();
    }
    
    private void OnInteract(InputAction.CallbackContext context)
    {
        interactPressed = true;
    }
    
    private void OnAim(InputAction.CallbackContext context)
    {
        aimInput = context.ReadValue<Vector2>();
    }
    
    private void OnFire(InputAction.CallbackContext context)
    {
        firePressed = true;
    }
    
    private void OnExit(InputAction.CallbackContext context)
    {
        exitPressed = true;
    }
    
    private void Update()
    {
        if (isPlayerUsing)
        {
            HandleCannonControl();
        }
        else
        {
            CheckForPlayerInteraction();
        }
        
        // Reset one-shot inputs
        interactPressed = false;
        firePressed = false;
        exitPressed = false;
    }
    
    private void CheckForPlayerInteraction()
    {
        // Find player if not found
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                playerController = playerObj.GetComponent<PlayerController>();
            }
            return;
        }
        
        // Check if player is in range
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= interactionRange)
        {
            // Show interaction prompt (in actual game, this would be UI)
            if (interactPressed)
            {
                StartUsingCannon();
            }
        }
    }
    
    private void StartUsingCannon()
    {
        isPlayerUsing = true;
        
        // Disable player movement
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        
        Debug.Log("Player started using cannon. Press E to exit.");
    }
    
    private void StopUsingCannon()
    {
        isPlayerUsing = false;
        
        // Re-enable player movement
        if (playerController != null)
        {
            playerController.enabled = true;
        }
        
        Debug.Log("Player stopped using cannon.");
    }
    
    private void HandleCannonControl()
    {
        // Exit cannon
        if (exitPressed)
        {
            StopUsingCannon();
            return;
        }
        
        // Aim cannon with mouse
        if (aimPivot != null)
        {
            float mouseX = aimInput.x * aimSensitivity;
            float mouseY = aimInput.y * aimSensitivity;
            
            // Rotate horizontally
            transform.Rotate(Vector3.up, mouseX * 2f);
            
            // Rotate vertically (with limits)
            Vector3 currentRotation = aimPivot.localEulerAngles;
            float newXRotation = currentRotation.x - mouseY * 2f;
            
            // Clamp vertical rotation
            if (newXRotation > 180f) newXRotation -= 360f;
            newXRotation = Mathf.Clamp(newXRotation, -20f, 45f);
            
            aimPivot.localEulerAngles = new Vector3(newXRotation, 0, 0);
        }
        
        // Fire cannon
        if (firePressed && !isReloading)
        {
            FireCannon();
        }
    }
    
    private void FireCannon()
    {
        isReloading = true;
        
        Debug.Log("Cannon fired!");
        
        // Play effects
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        
        if (fireSound != null)
        {
            AudioSource.PlayClipAtPoint(fireSound, transform.position);
        }
        
        // Spawn cannonball
        if (cannonballPrefab != null && firePoint != null)
        {
            GameObject cannonball = Instantiate(cannonballPrefab, firePoint.position, firePoint.rotation);
            
            // Add velocity to cannonball
            Rigidbody rb = cannonball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.forward * projectileSpeed;
            }
            
            // Add cannonball script for impact detection
            Cannonball cannonballScript = cannonball.GetComponent<Cannonball>();
            if (cannonballScript != null)
            {
                cannonballScript.damage = damage;
                cannonballScript.explosionRadius = explosionRadius;
                cannonballScript.explosionEffect = explosionEffect;
            }
            
            // Destroy cannonball after time
            Destroy(cannonball, 5f);
        }
        
        // Start reload
        Invoke(nameof(FinishReload), reloadTime);
    }
    
    private void FinishReload()
    {
        isReloading = false;
        Debug.Log("Cannon reloaded!");
    }
    
    private void OnDrawGizmosSelected()
    {
        // Draw interaction range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
        
        // Draw firing direction
        if (firePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(firePoint.position, firePoint.forward * range);
        }
    }
}
