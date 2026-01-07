using UnityEngine;

/// <summary>
/// Rum bottle grenade weapon
/// Explodes on impact and intoxicates nearby enemies
/// </summary>
public class RumBottleGrenade : MonoBehaviour
{
    [Header("Throw Settings")]
    public float throwForce = 15f;
    public float throwUpAngle = 30f;
    public KeyCode throwKey = KeyCode.G;
    
    [Header("Grenade Prefab")]
    public GameObject bottlePrefab;
    
    [Header("Ammo")]
    public int grenadeCount = 5;
    
    [Header("References")]
    public Transform throwPoint;
    public Camera fpsCam;
    
    private void Start()
    {
        if (fpsCam == null)
        {
            fpsCam = Camera.main;
        }
        
        if (throwPoint == null)
        {
            throwPoint = fpsCam.transform;
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(throwKey) && grenadeCount > 0)
        {
            ThrowGrenade();
        }
    }
    
    private void ThrowGrenade()
    {
        if (bottlePrefab == null) return;
        
        grenadeCount--;
        
        // Calculate throw direction with upward angle
        Vector3 throwDirection = fpsCam.transform.forward;
        throwDirection = Quaternion.AngleAxis(-throwUpAngle, fpsCam.transform.right) * throwDirection;
        
        // Spawn bottle
        GameObject bottle = Instantiate(bottlePrefab, throwPoint.position, Quaternion.identity);
        
        // Add physics
        Rigidbody rb = bottle.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = bottle.AddComponent<Rigidbody>();
        }
        
        rb.velocity = throwDirection * throwForce;
        rb.angularVelocity = Random.insideUnitSphere * 5f; // Spinning bottle
        
        // Add bottle script if not present
        RumBottle bottleScript = bottle.GetComponent<RumBottle>();
        if (bottleScript == null)
        {
            bottleScript = bottle.AddComponent<RumBottle>();
        }
        
        Debug.Log($"Threw rum bottle grenade! Remaining: {grenadeCount}");
    }
    
    private void OnGUI()
    {
        GUI.Label(new Rect(10, Screen.height - 40, 200, 20), $"Rum Grenades (G): {grenadeCount}");
    }
}

/// <summary>
/// Rum bottle projectile that explodes and intoxicates enemies
/// </summary>
public class RumBottle : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float explosionRadius = 6f;
    public float explosionDamage = 30f;
    public float drunkennessAmount = 50f;
    
    [Header("Effects")]
    public GameObject explosionEffect;
    
    private bool hasExploded = false;
    
    private void Start()
    {
        // Auto-destroy after some time
        Destroy(gameObject, 5f);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;
        
        Explode();
    }
    
    private void Explode()
    {
        hasExploded = true;
        
        Debug.Log("Rum bottle exploded!");
        
        // Find all colliders in explosion radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        
        foreach (Collider hitCollider in hitColliders)
        {
            // Damage and intoxicate enemies
            if (hitCollider.CompareTag("Enemy") || hitCollider.CompareTag("Pirate") || 
                hitCollider.CompareTag("EmpireSoldier") || hitCollider.CompareTag("SeaCreature"))
            {
                // Apply damage
                EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                    float damageMultiplier = 1f - (distance / explosionRadius);
                    enemyHealth.TakeDamage(explosionDamage * damageMultiplier);
                }
                
                // Apply drunkenness
                DrunkennesSystem drunkenness = hitCollider.GetComponent<DrunkennesSystem>();
                if (drunkenness != null)
                {
                    float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                    float drunkMultiplier = 1f - (distance / explosionRadius);
                    drunkenness.AddDrunkenness(drunkennessAmount * drunkMultiplier);
                    
                    Debug.Log($"{hitCollider.name} got drunk from rum explosion!");
                }
                
                // Apply knockback
                Rigidbody rb = hitCollider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 direction = (hitCollider.transform.position - transform.position).normalized;
                    rb.AddForce(direction * 300f);
                }
            }
        }
        
        // Spawn explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        
        // Destroy bottle
        Destroy(gameObject);
    }
}
