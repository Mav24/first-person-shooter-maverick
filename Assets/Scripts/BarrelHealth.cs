using UnityEngine;

/// <summary>
/// Manages barrel health and destruction
/// Barrels are the primary objective to defend
/// </summary>
public class BarrelHealth : MonoBehaviour
{
    [Header("Barrel Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    
    [Header("Rum Spill Effects")]
    public float rumSpillRadius = 5f;
    public float rumDrunkennessAmount = 30f;
    
    [Header("Visual")]
    public GameObject barrelModel;
    public GameObject destroyedBarrelPrefab;
    
    private bool isDestroyed = false;
    
    private void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(float damage)
    {
        if (isDestroyed) return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        if (currentHealth <= 0)
        {
            DestroyBarrel();
        }
    }
    
    private void DestroyBarrel()
    {
        isDestroyed = true;
        
        // Spill rum and affect nearby enemies
        SpillRum();
        
        // Notify game manager
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.OnBarrelDestroyed(this);
        }
        
        // Visual effects
        if (destroyedBarrelPrefab != null)
        {
            Instantiate(destroyedBarrelPrefab, transform.position, transform.rotation);
        }
        
        // Disable or destroy this barrel
        if (barrelModel != null)
        {
            barrelModel.SetActive(false);
        }
        
        Debug.Log($"Barrel destroyed at {transform.position}!");
        
        // Keep the GameObject but disable collider
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
    }
    
    private void SpillRum()
    {
        // Find all enemies in radius and make them drunk
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, rumSpillRadius);
        
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy") || hitCollider.CompareTag("Pirate") || 
                hitCollider.CompareTag("EmpireSoldier") || hitCollider.CompareTag("SeaCreature"))
            {
                DrunkennessSystem drunkenness = hitCollider.GetComponent<DrunkennessSystem>();
                if (drunkenness != null)
                {
                    drunkenness.AddDrunkenness(rumDrunkennessAmount);
                    Debug.Log($"{hitCollider.name} got drunk from rum spill!");
                }
            }
        }
    }
    
    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
    
    private void OnDrawGizmosSelected()
    {
        // Draw rum spill radius
        Gizmos.color = new Color(0.8f, 0.4f, 0.1f, 0.3f);
        Gizmos.DrawSphere(transform.position, rumSpillRadius);
    }
}
