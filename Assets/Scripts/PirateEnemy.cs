using UnityEngine;

/// <summary>
/// Pirate enemy - aggressive melee and ranged attacker
/// Focuses on stealing rum barrels
/// </summary>
public class PirateEnemy : EnemyAI
{
    [Header("Pirate Specific")]
    public bool canStealBarrels = true;
    public float stealTime = 3f;
    
    [Header("Combat Style")]
    public bool prefersMelee = true;
    public float meleeRange = 2f;
    
    private bool isStealingBarrel = false;
    private float stealProgress = 0f;
    
    private void Start()
    {
        // Set pirate-specific values
        baseSpeed = 4f;
        attackDamage = 15f;
        attackCooldown = 1.2f;
    }
    
    private void Update()
    {
        // Handle barrel stealing if close enough
        if (canStealBarrels && currentTarget != null && currentTarget.CompareTag("Barrel"))
        {
            float distanceToBarrel = Vector3.Distance(transform.position, currentTarget.position);
            
            if (distanceToBarrel <= attackRange)
            {
                if (!isStealingBarrel)
                {
                    StartStealingBarrel();
                }
                else
                {
                    ContinueStealingBarrel();
                }
                return;
            }
        }
        
        // Normal AI behavior
        base.Update();
    }
    
    private void StartStealingBarrel()
    {
        isStealingBarrel = true;
        stealProgress = 0f;
        Debug.Log($"{gameObject.name} started stealing barrel!");
    }
    
    private void ContinueStealingBarrel()
    {
        stealProgress += Time.deltaTime;
        
        if (stealProgress >= stealTime)
        {
            StealBarrel();
        }
    }
    
    private void StealBarrel()
    {
        Debug.Log($"{gameObject.name} stole a barrel!");
        
        // Destroy the barrel
        BarrelHealth barrel = currentTarget.GetComponent<BarrelHealth>();
        if (barrel != null)
        {
            barrel.TakeDamage(barrel.maxHealth);
        }
        
        isStealingBarrel = false;
        stealProgress = 0f;
        
        // Select new target
        SelectTarget();
    }
}
