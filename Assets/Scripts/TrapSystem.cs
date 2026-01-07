using UnityEngine;

/// <summary>
/// Types of traps available in the game
/// </summary>
public enum TrapType
{
    RumPuddle,      // Makes enemies drunk
    BarrelBomb,     // Explosive trap
    SpikeTrap,      // Damage trap
    NetTrap,        // Immobilize trap
    FireBarrel      // Fire damage over time
}

/// <summary>
/// Manages trap placement and effects
/// </summary>
public class TrapSystem : MonoBehaviour
{
    [Header("Trap Settings")]
    public TrapType trapType = TrapType.BarrelBomb;
    public float triggerRadius = 2f;
    public float damage = 50f;
    public float drunkennessAmount = 40f;
    public float immobilizeDuration = 3f;
    
    [Header("Effects")]
    public GameObject explosionEffect;
    public GameObject activationEffect;
    
    private bool isTriggered = false;
    private bool isArmed = true;
    
    private void Update()
    {
        if (!isArmed || isTriggered) return;
        
        // Check for enemies in range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, triggerRadius);
        
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy") || hitCollider.CompareTag("Pirate") || 
                hitCollider.CompareTag("EmpireSoldier") || hitCollider.CompareTag("SeaCreature"))
            {
                TriggerTrap();
                break;
            }
        }
    }
    
    private void TriggerTrap()
    {
        if (isTriggered) return;
        
        isTriggered = true;
        Debug.Log($"{trapType} trap triggered!");
        
        // Apply trap effects based on type
        switch (trapType)
        {
            case TrapType.RumPuddle:
                ApplyRumPuddleEffect();
                break;
            case TrapType.BarrelBomb:
                ApplyExplosionEffect();
                break;
            case TrapType.SpikeTrap:
                ApplySpikeEffect();
                break;
            case TrapType.NetTrap:
                ApplyNetEffect();
                break;
            case TrapType.FireBarrel:
                ApplyFireEffect();
                break;
        }
        
        // Spawn activation effect
        if (activationEffect != null)
        {
            Instantiate(activationEffect, transform.position, Quaternion.identity);
        }
        
        // Destroy trap after use (except fire barrel which lasts longer)
        if (trapType != TrapType.FireBarrel)
        {
            Destroy(gameObject, 0.5f);
        }
        else
        {
            Destroy(gameObject, 10f); // Fire barrel lasts 10 seconds
        }
    }
    
    private void ApplyRumPuddleEffect()
    {
        // Make enemies drunk
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, triggerRadius);
        
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy") || hitCollider.CompareTag("Pirate") || 
                hitCollider.CompareTag("EmpireSoldier") || hitCollider.CompareTag("SeaCreature"))
            {
                DrunkennessSystem drunkenness = hitCollider.GetComponent<DrunkennessSystem>();
                if (drunkenness != null)
                {
                    drunkenness.AddDrunkenness(drunkennessAmount);
                }
            }
        }
    }
    
    private void ApplyExplosionEffect()
    {
        // Damage and knock back enemies
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, triggerRadius);
        
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy") || hitCollider.CompareTag("Pirate") || 
                hitCollider.CompareTag("EmpireSoldier") || hitCollider.CompareTag("SeaCreature"))
            {
                EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }
                
                // Apply knockback
                Rigidbody rb = hitCollider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 direction = (hitCollider.transform.position - transform.position).normalized;
                    rb.AddForce(direction * 500f);
                }
            }
        }
        
        // Spawn explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
    }
    
    private void ApplySpikeEffect()
    {
        // Deal damage to enemies
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, triggerRadius);
        
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy") || hitCollider.CompareTag("Pirate") || 
                hitCollider.CompareTag("EmpireSoldier") || hitCollider.CompareTag("SeaCreature"))
            {
                EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }
            }
        }
    }
    
    private void ApplyNetEffect()
    {
        // Immobilize enemies
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, triggerRadius);
        
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy") || hitCollider.CompareTag("Pirate") || 
                hitCollider.CompareTag("EmpireSoldier") || hitCollider.CompareTag("SeaCreature"))
            {
                UnityEngine.AI.NavMeshAgent agent = hitCollider.GetComponent<UnityEngine.AI.NavMeshAgent>();
                if (agent != null)
                {
                    agent.isStopped = true;
                    // Re-enable after duration
                    StartCoroutine(ReleaseFromNet(agent));
                }
            }
        }
    }
    
    private System.Collections.IEnumerator ReleaseFromNet(UnityEngine.AI.NavMeshAgent agent)
    {
        yield return new WaitForSeconds(immobilizeDuration);
        if (agent != null)
        {
            agent.isStopped = false;
        }
    }
    
    private void ApplyFireEffect()
    {
        // Continuous damage over time
        StartCoroutine(FireDamageOverTime());
    }
    
    private System.Collections.IEnumerator FireDamageOverTime()
    {
        float duration = 10f;
        float tickRate = 0.5f;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, triggerRadius);
            
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy") || hitCollider.CompareTag("Pirate") || 
                    hitCollider.CompareTag("EmpireSoldier") || hitCollider.CompareTag("SeaCreature"))
                {
                    EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(damage * tickRate);
                    }
                }
            }
            
            yield return new WaitForSeconds(tickRate);
            elapsed += tickRate;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        // Draw trigger radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}
