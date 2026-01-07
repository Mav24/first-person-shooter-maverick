using UnityEngine;

/// <summary>
/// Central game manager that tracks game state, score, and objectives
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Game State")]
    public bool isGameActive = true;
    public int currentScore = 0;
    
    [Header("Barrel Objectives")]
    public int totalBarrels = 0;
    public int barrelsDestroyed = 0;
    public int maxBarrelsLost = 3; // Game over if more than this many barrels are destroyed
    
    [Header("Enemy Tracking")]
    public int enemiesKilled = 0;
    public int totalEnemiesSpawned = 0;
    
    [Header("Wave Tracking")]
    public int currentWave = 0;
    public int wavesCompleted = 0;
    
    private static GameManager instance;
    
    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        InitializeGame();
    }
    
    private void InitializeGame()
    {
        // Count all barrels in the scene
        GameObject[] barrels = GameObject.FindGameObjectsWithTag("Barrel");
        totalBarrels = barrels.Length;
        barrelsDestroyed = 0;
        
        Debug.Log($"Game initialized with {totalBarrels} barrels to protect");
    }
    
    public void OnBarrelDestroyed(BarrelHealth barrel)
    {
        barrelsDestroyed++;
        
        Debug.Log($"Barrel destroyed! {barrelsDestroyed}/{totalBarrels}");
        
        // Check if too many barrels lost
        if (barrelsDestroyed >= maxBarrelsLost)
        {
            GameOver(false);
        }
    }
    
    public void OnEnemyKilled(EnemyHealth enemy)
    {
        enemiesKilled++;
        currentScore += 100; // Base score per enemy
        
        // Notify wave manager
        WaveManager waveManager = FindObjectOfType<WaveManager>();
        if (waveManager != null)
        {
            waveManager.OnEnemyDeath();
        }
        
        Debug.Log($"Enemy killed! Score: {currentScore}");
    }
    
    public void OnWaveCompleted(int waveNumber)
    {
        wavesCompleted++;
        currentScore += 500; // Bonus for completing wave
        
        Debug.Log($"Wave {waveNumber} completed! Total waves: {wavesCompleted}");
    }
    
    public void GameOver(bool victory)
    {
        isGameActive = false;
        
        if (victory)
        {
            Debug.Log($"VICTORY! Final Score: {currentScore}");
        }
        else
        {
            Debug.Log($"GAME OVER! Too many barrels lost. Final Score: {currentScore}");
        }
        
        // In actual game, show game over UI, allow restart, etc.
    }
    
    public void RestartGame()
    {
        // Reset game state
        isGameActive = true;
        currentScore = 0;
        enemiesKilled = 0;
        barrelsDestroyed = 0;
        wavesCompleted = 0;
        currentWave = 0;
        
        // Reload scene or reset objects
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
    
    public float GetBarrelSurvivalPercentage()
    {
        if (totalBarrels == 0) return 0f;
        return (float)(totalBarrels - barrelsDestroyed) / totalBarrels;
    }
    
    public static GameManager Instance
    {
        get { return instance; }
    }
}
