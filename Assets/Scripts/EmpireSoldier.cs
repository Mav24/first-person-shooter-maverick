using UnityEngine;

/// <summary>
/// Empire Soldier enemy - tactical, organized, uses formations
/// More disciplined than pirates even when drunk
/// </summary>
public class EmpireSoldier : EnemyAI
{
    [Header("Empire Soldier Specific")]
    public bool maintainsFormation = true;
    public float formationDistance = 3f;
    public GameObject[] squadMembers;
    
    [Header("Combat Style")]
    public bool usesRangedWeapons = true;
    public float rangedAttackRange = 15f;
    public GameObject projectilePrefab;
    
    private void Start()
    {
        // Set empire soldier-specific values
        baseSpeed = 3f;
        attackDamage = 12f;
        attackCooldown = 2f;
        
        // More likely to target player
        detectionRange = 25f;
    }
    
    private void Update()
    {
        // Check if should use ranged attack
        if (usesRangedWeapons && currentTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
            
            if (distanceToTarget <= rangedAttackRange && distanceToTarget > attackRange)
            {
                // Stop and shoot
                if (agent != null)
                {
                    agent.isStopped = true;
                }
                
                RangedAttack();
                return;
            }
        }
        
        // Normal AI behavior
        base.Update();
    }
    
    private void RangedAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        
        lastAttackTime = Time.time;
        
        // Apply accuracy modifier from drunkenness
        float accuracy = 1f;
        if (drunkennessSystem != null)
        {
            accuracy = drunkennessSystem.GetAccuracyMultiplier();
        }
        
        // Fire projectile
        if (projectilePrefab != null && currentTarget != null)
        {
            Vector3 direction = (currentTarget.position - transform.position).normalized;
            
            // Add inaccuracy based on drunkenness
            float inaccuracy = (1f - accuracy) * 10f;
            direction.x += Random.Range(-inaccuracy, inaccuracy);
            direction.y += Random.Range(-inaccuracy, inaccuracy);
            direction.z += Random.Range(-inaccuracy, inaccuracy);
            direction.Normalize();
            
            GameObject projectile = Instantiate(projectilePrefab, transform.position + Vector3.up, Quaternion.LookRotation(direction));
            
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * 20f;
            }
            
            Destroy(projectile, 5f);
            
            Debug.Log($"{gameObject.name} fired ranged attack!");
        }
    }
    
    protected override void SelectTarget()
    {
        // Empire soldiers prefer to attack the player
        if (Random.value < 0.7f && player != null)
        {
            currentTarget = player;
        }
        else
        {
            base.SelectTarget();
        }
    }
}
