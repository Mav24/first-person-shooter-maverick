using UnityEngine;

/// <summary>
/// Manages player health, damage, and death
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    
    [Header("Damage Effects")]
    public float damageFlashDuration = 0.1f;
    
    [Header("Respawn")]
    public bool canRespawn = true;
    public float respawnDelay = 3f;
    
    private bool isDead = false;
    
    private void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(float damage)
    {
        if (isDead) return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        // Trigger damage effects
        OnDamageTaken();
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Heal(float amount)
    {
        if (isDead) return;
        
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
    
    private void OnDamageTaken()
    {
        // Play damage sound, screen flash, etc.
        Debug.Log($"Player took damage! Current health: {currentHealth}");
    }
    
    private void Die()
    {
        isDead = true;
        Debug.Log("Player died!");
        
        // Disable player controls
        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.enabled = false;
        }
        
        // Trigger death effects, game over UI, etc.
        if (canRespawn)
        {
            Invoke(nameof(Respawn), respawnDelay);
        }
    }
    
    private void Respawn()
    {
        isDead = false;
        currentHealth = maxHealth;
        
        // Re-enable player controls
        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.enabled = true;
        }
        
        Debug.Log("Player respawned!");
    }
    
    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}
