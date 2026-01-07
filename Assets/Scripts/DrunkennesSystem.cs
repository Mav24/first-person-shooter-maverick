using UnityEngine;

/// <summary>
/// Enum representing enemy drunkenness states
/// The drunker the enemy, the more chaotic their behavior
/// </summary>
public enum DrunkenState
{
    Sober,      // Normal behavior
    Tipsy,      // Slightly impaired
    Drunk,      // Significantly impaired, erratic
    Wasted      // Nearly incapacitated
}

/// <summary>
/// Manages the drunkenness level of enemies
/// Affects movement speed, accuracy, and behavior
/// </summary>
public class DrunkennesSystem : MonoBehaviour
{
    [Header("Drunkenness Settings")]
    [Range(0f, 100f)]
    public float drunkenness = 0f;
    
    [Header("State Thresholds")]
    public float tipsyThreshold = 25f;
    public float drunkThreshold = 50f;
    public float wastedThreshold = 75f;
    
    [Header("Effects")]
    public float soberSpeedMultiplier = 1f;
    public float tipsySpeedMultiplier = 0.9f;
    public float drunkSpeedMultiplier = 0.7f;
    public float wastedSpeedMultiplier = 0.4f;
    
    public float soberAccuracyMultiplier = 1f;
    public float tipsyAccuracyMultiplier = 0.8f;
    public float drunkAccuracyMultiplier = 0.5f;
    public float wastedAccuracyMultiplier = 0.2f;
    
    [Header("Recovery")]
    public float soberUpRate = 2f; // Points per second when not drinking
    
    private DrunkenState currentState = DrunkenState.Sober;
    
    private void Update()
    {
        // Slowly sober up over time
        if (drunkenness > 0)
        {
            drunkenness -= soberUpRate * Time.deltaTime;
            drunkenness = Mathf.Clamp(drunkenness, 0, 100);
        }
        
        UpdateDrunkenState();
    }
    
    /// <summary>
    /// Increase drunkenness level
    /// </summary>
    public void AddDrunkenness(float amount)
    {
        drunkenness += amount;
        drunkenness = Mathf.Clamp(drunkenness, 0, 100);
        UpdateDrunkenState();
    }
    
    /// <summary>
    /// Update the current drunken state based on drunkenness level
    /// </summary>
    private void UpdateDrunkenState()
    {
        DrunkenState previousState = currentState;
        
        if (drunkenness >= wastedThreshold)
        {
            currentState = DrunkenState.Wasted;
        }
        else if (drunkenness >= drunkThreshold)
        {
            currentState = DrunkenState.Drunk;
        }
        else if (drunkenness >= tipsyThreshold)
        {
            currentState = DrunkenState.Tipsy;
        }
        else
        {
            currentState = DrunkenState.Sober;
        }
        
        // Notify if state changed
        if (previousState != currentState)
        {
            OnStateChanged(previousState, currentState);
        }
    }
    
    /// <summary>
    /// Called when drunkenness state changes
    /// </summary>
    private void OnStateChanged(DrunkenState from, DrunkenState to)
    {
        Debug.Log($"{gameObject.name} changed from {from} to {to}");
    }
    
    /// <summary>
    /// Get the current speed multiplier based on drunkenness
    /// </summary>
    public float GetSpeedMultiplier()
    {
        switch (currentState)
        {
            case DrunkenState.Sober: return soberSpeedMultiplier;
            case DrunkenState.Tipsy: return tipsySpeedMultiplier;
            case DrunkenState.Drunk: return drunkSpeedMultiplier;
            case DrunkenState.Wasted: return wastedSpeedMultiplier;
            default: return 1f;
        }
    }
    
    /// <summary>
    /// Get the current accuracy multiplier based on drunkenness
    /// </summary>
    public float GetAccuracyMultiplier()
    {
        switch (currentState)
        {
            case DrunkenState.Sober: return soberAccuracyMultiplier;
            case DrunkenState.Tipsy: return tipsyAccuracyMultiplier;
            case DrunkenState.Drunk: return drunkAccuracyMultiplier;
            case DrunkenState.Wasted: return wastedAccuracyMultiplier;
            default: return 1f;
        }
    }
    
    /// <summary>
    /// Check if enemy should stumble based on drunkenness
    /// </summary>
    public bool ShouldStumble()
    {
        if (currentState == DrunkenState.Wasted)
        {
            return Random.value < 0.3f; // 30% chance per frame when wasted
        }
        else if (currentState == DrunkenState.Drunk)
        {
            return Random.value < 0.1f; // 10% chance per frame when drunk
        }
        return false;
    }
    
    public DrunkenState GetCurrentState()
    {
        return currentState;
    }
}
