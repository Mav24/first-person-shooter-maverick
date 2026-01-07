using UnityEngine;

/// <summary>
/// Cannonball projectile script
/// Handles collision and explosion on impact
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Cannonball : MonoBehaviour
{
    [Header("Settings")]
    public float damage = 100f;
    public float explosionRadius = 5f;
    public GameObject explosionEffect;
    
    private bool hasExploded = false;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;
        
        Explode();
    }
    
    private void Explode()
    {
        hasExploded = true;
        
        // Find all colliders in explosion radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        
        foreach (Collider hitCollider in hitColliders)
        {
            // Damage enemies
            if (hitCollider.CompareTag("Enemy") || hitCollider.CompareTag("Pirate") || 
                hitCollider.CompareTag("EmpireSoldier") || hitCollider.CompareTag("SeaCreature"))
            {
                EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    // Calculate damage based on distance
                    float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                    float damageMultiplier = 1f - (distance / explosionRadius);
                    enemyHealth.TakeDamage(damage * damageMultiplier);
                }
                
                // Apply knockback
                Rigidbody rb = hitCollider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 direction = (hitCollider.transform.position - transform.position).normalized;
                    rb.AddForce(direction * 1000f);
                }
            }
        }
        
        // Spawn explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        
        // Destroy cannonball
        Destroy(gameObject);
    }
}
