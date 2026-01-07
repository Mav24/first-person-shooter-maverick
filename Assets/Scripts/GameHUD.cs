using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple HUD displaying player health, score, wave info, and barrel status
/// Uses Unity's legacy UI system for simplicity
/// </summary>
public class GameHUD : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public GameManager gameManager;
    public WaveManager waveManager;
    
    [Header("UI Elements - Create these in the scene")]
    public Text healthText;
    public Text scoreText;
    public Text waveText;
    public Text barrelText;
    public Text ammoText;
    public Image healthBar;
    
    [Header("Weapon Reference")]
    public WeaponSystem currentWeapon;
    
    private void Start()
    {
        // Auto-find references if not assigned
        if (playerHealth == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<PlayerHealth>();
            }
        }
        
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        
        if (waveManager == null)
        {
            waveManager = FindObjectOfType<WaveManager>();
        }
    }
    
    private void Update()
    {
        UpdateHealthDisplay();
        UpdateScoreDisplay();
        UpdateWaveDisplay();
        UpdateBarrelDisplay();
        UpdateAmmoDisplay();
    }
    
    private void UpdateHealthDisplay()
    {
        if (playerHealth != null)
        {
            if (healthText != null)
            {
                healthText.text = $"Health: {Mathf.Ceil(playerHealth.currentHealth)}/{playerHealth.maxHealth}";
            }
            
            if (healthBar != null)
            {
                healthBar.fillAmount = playerHealth.GetHealthPercentage();
            }
        }
    }
    
    private void UpdateScoreDisplay()
    {
        if (gameManager != null && scoreText != null)
        {
            scoreText.text = $"Score: {gameManager.currentScore}";
        }
    }
    
    private void UpdateWaveDisplay()
    {
        if (waveManager != null && waveText != null)
        {
            if (waveManager.isWaveActive)
            {
                waveText.text = $"Wave: {waveManager.currentWaveIndex + 1}";
            }
            else
            {
                waveText.text = "Preparing...";
            }
        }
    }
    
    private void UpdateBarrelDisplay()
    {
        if (gameManager != null && barrelText != null)
        {
            int barrelsRemaining = gameManager.totalBarrels - gameManager.barrelsDestroyed;
            barrelText.text = $"Barrels: {barrelsRemaining}/{gameManager.totalBarrels}";
            
            // Change color if running low
            if (barrelsRemaining <= gameManager.maxBarrelsLost)
            {
                barrelText.color = Color.red;
            }
            else
            {
                barrelText.color = Color.white;
            }
        }
    }
    
    private void UpdateAmmoDisplay()
    {
        if (currentWeapon != null && ammoText != null)
        {
            if (currentWeapon.isReloading)
            {
                ammoText.text = "Reloading...";
            }
            else
            {
                ammoText.text = $"Ammo: {currentWeapon.currentAmmo}/{currentWeapon.maxAmmo}";
            }
        }
    }
    
    /// <summary>
    /// Display a temporary message on screen
    /// </summary>
    public void ShowMessage(string message, float duration = 2f)
    {
        StartCoroutine(DisplayTemporaryMessage(message, duration));
    }
    
    private System.Collections.IEnumerator DisplayTemporaryMessage(string message, float duration)
    {
        // This would require a Text element for messages
        Debug.Log($"HUD Message: {message}");
        yield return new WaitForSeconds(duration);
    }
}
