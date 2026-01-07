using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages wave-based enemy spawning
/// Increases difficulty over time
/// </summary>
public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnInfo
    {
        public GameObject enemyPrefab;
        public int count;
    }
    
    [System.Serializable]
    public class Wave
    {
        public string waveName = "Wave 1";
        public List<EnemySpawnInfo> enemies = new List<EnemySpawnInfo>();
        public float timeBetweenSpawns = 1f;
        public float preparationTime = 10f;
    }
    
    [Header("Wave Configuration")]
    public List<Wave> waves = new List<Wave>();
    public Transform[] spawnPoints;
    
    [Header("Current Wave")]
    public int currentWaveIndex = 0;
    public bool isWaveActive = false;
    
    [Header("Dynamic Difficulty")]
    public bool enableDynamicWaves = true;
    public float difficultyMultiplier = 1.1f;
    
    private int enemiesRemaining = 0;
    private bool isSpawning = false;
    
    private void Start()
    {
        // Find spawn points if not assigned
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");
            spawnPoints = new Transform[spawnPointObjects.Length];
            for (int i = 0; i < spawnPointObjects.Length; i++)
            {
                spawnPoints[i] = spawnPointObjects[i].transform;
            }
        }
        
        // Start first wave after delay
        Invoke(nameof(StartNextWave), 5f);
    }
    
    private void Update()
    {
        // Check if wave is complete
        if (isWaveActive && !isSpawning && enemiesRemaining <= 0)
        {
            CompleteWave();
        }
    }
    
    public void StartNextWave()
    {
        if (currentWaveIndex >= waves.Count)
        {
            if (enableDynamicWaves)
            {
                GenerateDynamicWave();
            }
            else
            {
                Debug.Log("All waves completed!");
                return;
            }
        }
        
        isWaveActive = true;
        Wave currentWave = waves[currentWaveIndex];
        
        Debug.Log($"Starting {currentWave.waveName}");
        
        // Start spawning enemies
        StartCoroutine(SpawnWave(currentWave));
    }
    
    private System.Collections.IEnumerator SpawnWave(Wave wave)
    {
        isSpawning = true;
        enemiesRemaining = 0;
        
        // Count total enemies
        foreach (EnemySpawnInfo spawnInfo in wave.enemies)
        {
            enemiesRemaining += spawnInfo.count;
        }
        
        // Spawn each enemy type
        foreach (EnemySpawnInfo spawnInfo in wave.enemies)
        {
            for (int i = 0; i < spawnInfo.count; i++)
            {
                SpawnEnemy(spawnInfo.enemyPrefab);
                yield return new WaitForSeconds(wave.timeBetweenSpawns);
            }
        }
        
        isSpawning = false;
    }
    
    private void SpawnEnemy(GameObject enemyPrefab)
    {
        if (enemyPrefab == null || spawnPoints.Length == 0) return;
        
        // Choose random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        
        // Spawn enemy
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        
        Debug.Log($"Spawned {enemy.name} at {spawnPoint.name}");
    }
    
    private void CompleteWave()
    {
        isWaveActive = false;
        currentWaveIndex++;
        
        Debug.Log($"Wave {currentWaveIndex} completed!");
        
        // Notify game manager
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.OnWaveCompleted(currentWaveIndex - 1);
        }
        
        // Start next wave after preparation time
        if (currentWaveIndex < waves.Count)
        {
            float prepTime = waves[currentWaveIndex - 1].preparationTime;
            Debug.Log($"Next wave in {prepTime} seconds");
            Invoke(nameof(StartNextWave), prepTime);
        }
        else if (enableDynamicWaves)
        {
            Invoke(nameof(StartNextWave), 10f);
        }
    }
    
    public void OnEnemyDeath()
    {
        enemiesRemaining--;
        enemiesRemaining = Mathf.Max(0, enemiesRemaining);
    }
    
    private void GenerateDynamicWave()
    {
        // Create a new wave based on previous wave with increased difficulty
        Wave newWave = new Wave();
        newWave.waveName = $"Wave {currentWaveIndex + 1}";
        newWave.timeBetweenSpawns = 0.8f;
        newWave.preparationTime = 10f;
        
        // Get enemy types from previous waves
        if (waves.Count > 0)
        {
            Wave lastWave = waves[waves.Count - 1];
            foreach (EnemySpawnInfo oldSpawnInfo in lastWave.enemies)
            {
                EnemySpawnInfo newSpawnInfo = new EnemySpawnInfo();
                newSpawnInfo.enemyPrefab = oldSpawnInfo.enemyPrefab;
                newSpawnInfo.count = Mathf.CeilToInt(oldSpawnInfo.count * difficultyMultiplier);
                newWave.enemies.Add(newSpawnInfo);
            }
        }
        
        waves.Add(newWave);
    }
}
