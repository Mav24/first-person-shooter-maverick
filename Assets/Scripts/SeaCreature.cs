using UnityEngine;

/// <summary>
/// Mythical sea creature enemy - unique abilities and behaviors
/// Can emerge from water, has special attacks
/// </summary>
public class SeaCreature : EnemyAI
{
    [Header("Sea Creature Specific")]
    public SeaCreatureType creatureType = SeaCreatureType.Kraken;
    public bool canEmergeFromWater = true;
    public float specialAbilityCooldown = 10f;
    
    [Header("Special Abilities")]
    public float areaAttackRadius = 8f;
    public float tentacleReach = 10f;
    
    private float lastSpecialAbilityTime;
    
    public enum SeaCreatureType
    {
        Kraken,
        Merfolk,
        CursedSailor
    }
    
    private void Start()
    {
        // Set creature-specific values based on type
        switch (creatureType)
        {
            case SeaCreatureType.Kraken:
                baseSpeed = 2f;
                attackDamage = 25f;
                attackCooldown = 2.5f;
                break;
            case SeaCreatureType.Merfolk:
                baseSpeed = 4.5f;
                attackDamage = 15f;
                attackCooldown = 1f;
                break;
            case SeaCreatureType.CursedSailor:
                baseSpeed = 3f;
                attackDamage = 20f;
                attackCooldown = 1.5f;
                break;
        }
    }
    
    private void Update()
    {
        // Check if should use special ability
        if (Time.time - lastSpecialAbilityTime >= specialAbilityCooldown)
        {
            if (currentTarget != null)
            {
                float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
                
                if (distanceToTarget <= areaAttackRadius)
                {
                    UseSpecialAbility();
                }
            }
        }
        
        // Normal AI behavior
        base.Update();
    }
    
    private void UseSpecialAbility()
    {
        lastSpecialAbilityTime = Time.time;
        
        switch (creatureType)
        {
            case SeaCreatureType.Kraken:
                TentacleSlam();
                break;
            case SeaCreatureType.Merfolk:
                SonicScream();
                break;
            case SeaCreatureType.CursedSailor:
                SummonMinions();
                break;
        }
    }
    
    private void TentacleSlam()
    {
        Debug.Log($"{gameObject.name} uses Tentacle Slam!");
        
        // Area attack - damage all nearby targets
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, areaAttackRadius);
        
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage * 1.5f);
                }
            }
            else if (hitCollider.CompareTag("Barrel"))
            {
                BarrelHealth barrel = hitCollider.GetComponent<BarrelHealth>();
                if (barrel != null)
                {
                    barrel.TakeDamage(attackDamage);
                }
            }
        }
    }
    
    private void SonicScream()
    {
        Debug.Log($"{gameObject.name} uses Sonic Scream!");
        
        // Stun player and damage
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, areaAttackRadius);
        
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage * 0.5f);
                }
                
                // Disable player controls briefly
                PlayerController controller = hitCollider.GetComponent<PlayerController>();
                if (controller != null)
                {
                    controller.enabled = false;
                    StartCoroutine(ReenablePlayerAfterStun(controller));
                }
            }
        }
    }
    
    private System.Collections.IEnumerator ReenablePlayerAfterStun(PlayerController controller)
    {
        yield return new WaitForSeconds(2f);
        if (controller != null)
        {
            controller.enabled = true;
        }
    }
    
    private void SummonMinions()
    {
        Debug.Log($"{gameObject.name} summons minions!");
        
        // Spawn additional cursed sailors around this creature
        // This would require a minion prefab to be set up
        // For now, just log the ability
    }
    
    private void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        
        // Draw special ability range
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, areaAttackRadius);
    }
}
