using UnityEngine;

/// <summary>
/// Simple script to place traps in the world
/// Attach to player or create trap placement system
/// </summary>
public class TrapPlacer : MonoBehaviour
{
    [Header("Trap Prefabs")]
    public GameObject rumPuddlePrefab;
    public GameObject barrelBombPrefab;
    public GameObject spikeTrapPrefab;
    public GameObject netTrapPrefab;
    public GameObject fireBarrelPrefab;
    
    [Header("Placement Settings")]
    public float placementRange = 5f;
    public LayerMask placementLayers;
    public KeyCode placementKey = KeyCode.T;
    
    [Header("Current Trap")]
    public int currentTrapIndex = 0;
    public string[] trapNames = { "Rum Puddle", "Barrel Bomb", "Spike Trap", "Net Trap", "Fire Barrel" };
    
    [Header("Resources")]
    public int rumPuddleCount = 5;
    public int barrelBombCount = 3;
    public int spikeTrapCount = 4;
    public int netTrapCount = 3;
    public int fireBarrelCount = 2;
    
    private Camera fpsCam;
    
    private void Start()
    {
        fpsCam = Camera.main;
    }
    
    private void Update()
    {
        // Switch trap type with number keys
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentTrapIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentTrapIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) currentTrapIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) currentTrapIndex = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) currentTrapIndex = 4;
        
        // Place trap
        if (Input.GetKeyDown(placementKey))
        {
            PlaceTrap();
        }
    }
    
    private void PlaceTrap()
    {
        // Raycast to find placement position
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, placementRange, placementLayers))
        {
            GameObject trapPrefab = GetCurrentTrapPrefab();
            
            if (trapPrefab == null)
            {
                Debug.Log("No trap prefab assigned for this type!");
                return;
            }
            
            // Check if player has enough resources
            if (!HasTrapResources(currentTrapIndex))
            {
                Debug.Log($"Not enough {trapNames[currentTrapIndex]} traps!");
                return;
            }
            
            // Instantiate trap
            GameObject trap = Instantiate(trapPrefab, hit.point, Quaternion.identity);
            
            // Deduct resources
            DeductTrapResources(currentTrapIndex);
            
            Debug.Log($"Placed {trapNames[currentTrapIndex]} at {hit.point}");
        }
    }
    
    private GameObject GetCurrentTrapPrefab()
    {
        switch (currentTrapIndex)
        {
            case 0: return rumPuddlePrefab;
            case 1: return barrelBombPrefab;
            case 2: return spikeTrapPrefab;
            case 3: return netTrapPrefab;
            case 4: return fireBarrelPrefab;
            default: return null;
        }
    }
    
    private bool HasTrapResources(int trapIndex)
    {
        switch (trapIndex)
        {
            case 0: return rumPuddleCount > 0;
            case 1: return barrelBombCount > 0;
            case 2: return spikeTrapCount > 0;
            case 3: return netTrapCount > 0;
            case 4: return fireBarrelCount > 0;
            default: return false;
        }
    }
    
    private void DeductTrapResources(int trapIndex)
    {
        switch (trapIndex)
        {
            case 0: rumPuddleCount--; break;
            case 1: barrelBombCount--; break;
            case 2: spikeTrapCount--; break;
            case 3: netTrapCount--; break;
            case 4: fireBarrelCount--; break;
        }
    }
    
    private void OnGUI()
    {
        // Simple UI showing current trap and counts
        GUI.Box(new Rect(10, 10, 250, 150), "Trap Placer");
        
        GUI.Label(new Rect(20, 40, 200, 20), $"Current: {trapNames[currentTrapIndex]}");
        GUI.Label(new Rect(20, 60, 200, 20), "Press T to place");
        GUI.Label(new Rect(20, 80, 200, 20), "Press 1-5 to switch");
        
        GUI.Label(new Rect(20, 100, 200, 20), $"1. Rum Puddle: {rumPuddleCount}");
        GUI.Label(new Rect(20, 115, 200, 20), $"2. Barrel Bomb: {barrelBombCount}");
        GUI.Label(new Rect(20, 130, 200, 20), $"3. Spike Trap: {spikeTrapCount}");
        GUI.Label(new Rect(20, 145, 200, 20), $"4. Net Trap: {netTrapCount}");
        GUI.Label(new Rect(20, 160, 200, 20), $"5. Fire Barrel: {fireBarrelCount}");
    }
}
