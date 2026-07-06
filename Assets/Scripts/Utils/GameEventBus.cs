using System;
using UnityEngine;

public static class GameEventBus
{
    // Day 2 Blueprint Event Channels
    public static event Action<string> OnVehicleStateChanged;
    public static event Action<float> OnCargoTiltChanged; 
    public static event Action OnCargoLost;
    public static event Action OnLevelComplete;
    public static event Action<string> OnGameStateChanged;

    // Direct Firing Hooks called by your components
    public static void FireVehicleStateChanged(string state) => OnVehicleStateChanged?.Invoke(state);
    
    // This perfectly fixes the 'FireCargoTiltChanged' error from your Cargo script
    public static void FireCargoTiltChanged(float angle) => OnCargoTiltChanged?.Invoke(angle);
    
    public static void FireCargoLost() => OnCargoLost?.Invoke();
    
    public static void FireLevelComplete() => OnLevelComplete?.Invoke();
    
    public static void FireGameStateChanged(string state) => OnGameStateChanged?.Invoke(state);
}
