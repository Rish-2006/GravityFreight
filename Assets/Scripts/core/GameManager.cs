using UnityEngine;
using UnityEngine.SceneManagement;

// Define the different "modes" your game can be in
public enum GameState { MainMenu, Playing, Paused, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private GameState _state = GameState.MainMenu;

    void Awake()
    {
        // Singleton Guard: Ensures only one GameManager exists
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Mobile Optimization: Locks 60FPS to prevent battery drain and overheating
        Application.targetFrameRate = 60;
    }

    public void UpdateState(GameState newState)
    {
        _state = newState;
        Debug.Log($"Game State Changed to: {newState}");
        
        // You can trigger your UI or Physics here based on the state
        if(newState == GameState.GameOver) Time.timeScale = 0.5f; // Slow motion on fail
        else Time.timeScale = 1.0f;
    }

    public void RestartLevel()
    {
        // Resets the physics simulation and reloads the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
