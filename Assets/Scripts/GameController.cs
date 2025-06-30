using UnityEngine;

public class GameController : MonoBehaviour
{
    public WaveManager waveManager;
    public PlayerPerformanceTracker tracker;
    public Planet planet;

    void Start()
    {
        // Called automatically when the scene loads
        StartGame();
    }

    public void StartGame()
    {
        tracker.enabled = true;
        //planet.ResetPlanet();
        waveManager.StartLevel();
    }

    public void RestartGame()
    {
        // Optional: reload scene or reset state
    }
}
