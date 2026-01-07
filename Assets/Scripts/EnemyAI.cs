using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Base enemy AI that uses NavMesh for pathfinding
/// Includes integration with drunkenness system
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform player;
    public Transform[] barrelTargets; // Rum barrels to attack
    
    [Header("Combat Settings")]
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.5f;
    public float detectionRange = 20f;
    
    [Header("Movement")]
    public float baseSpeed = 3.5f;
    
    private NavMeshAgent agent;
    private DrunkennesSystem drunkennessSystem;
    private float lastAttackTime;
    private Transform currentTarget;
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        drunkennessSystem = GetComponent<DrunkennesSystem>();
        
        // Try to find player if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
        
        // Find barrel targets if not assigned
        if (barrelTargets == null || barrelTargets.Length == 0)
        {
            GameObject[] barrels = GameObject.FindGameObjectsWithTag("Barrel");
            barrelTargets = new Transform[barrels.Length];
            for (int i = 0; i < barrels.Length; i++)
            {
                barrelTargets[i] = barrels[i].transform;
            }
        }
        
        SelectTarget();
    }
    
    private void Update()
    {
        if (currentTarget == null)
        {
            SelectTarget();
            return;
        }
        
        // Apply drunkenness speed modifier
        if (drunkennessSystem != null)
        {
            agent.speed = baseSpeed * drunkennessSystem.GetSpeedMultiplier();
            
            // Check if enemy should stumble
            if (drunkennessSystem.ShouldStumble())
            {
                agent.isStopped = true;
                Invoke(nameof(RecoverFromStumble), Random.Range(0.5f, 2f));
                return;
            }
        }
        else
        {
            agent.speed = baseSpeed;
        }
        
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
        
        // Move towards target
        if (distanceToTarget > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(currentTarget.position);
        }
        else
        {
            // Stop and attack
            agent.isStopped = true;
            AttackTarget();
        }
    }
    
    private void SelectTarget()
    {
        // Randomly decide between attacking player or barrels
        // More likely to target barrels (60% chance)
        if (Random.value < 0.6f && barrelTargets != null && barrelTargets.Length > 0)
        {
            // Select closest barrel
            Transform closestBarrel = null;
            float closestDistance = Mathf.Infinity;
            
            foreach (Transform barrel in barrelTargets)
            {
                if (barrel == null) continue;
                
                float distance = Vector3.Distance(transform.position, barrel.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestBarrel = barrel;
                }
            }
            
            currentTarget = closestBarrel;
        }
        else if (player != null)
        {
            currentTarget = player;
        }
    }
    
    private void AttackTarget()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        
        lastAttackTime = Time.time;
        
        // Apply accuracy modifier from drunkenness
        float accuracy = 1f;
        if (drunkennessSystem != null)
        {
            accuracy = drunkennessSystem.GetAccuracyMultiplier();
        }
        
        // Random miss chance based on accuracy
        if (Random.value > accuracy)
        {
            Debug.Log($"{gameObject.name} missed attack due to drunkenness!");
            return;
        }
        
        // Deal damage to target
        if (currentTarget.CompareTag("Player"))
        {
            PlayerHealth playerHealth = currentTarget.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
        else if (currentTarget.CompareTag("Barrel"))
        {
            BarrelHealth barrelHealth = currentTarget.GetComponent<BarrelHealth>();
            if (barrelHealth != null)
            {
                barrelHealth.TakeDamage(attackDamage);
            }
        }
    }
    
    private void RecoverFromStumble()
    {
        agent.isStopped = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
