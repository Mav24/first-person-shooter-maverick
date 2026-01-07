using UnityEngine;

/// <summary>
/// Manages cannon firing mechanics
/// Player can interact with cannons to fire at enemies
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
    public KeyCode useKey = KeyCode.E;
    
    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public GameObject explosionEffect;
    public AudioClip fireSound;
    
    private bool isPlayerUsing = false;
    private bool isReloading = false;
    private Transform player;
    private PlayerController playerController;
    
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
            if (Input.GetKeyDown(useKey))
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
        if (Input.GetKeyDown(useKey))
        {
            StopUsingCannon();
            return;
        }
        
        // Aim cannon with mouse
        if (aimPivot != null)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            
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
        if (Input.GetButtonDown("Fire1") && !isReloading)
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
                rb.velocity = firePoint.forward * projectileSpeed;
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
