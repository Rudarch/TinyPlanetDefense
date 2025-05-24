using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    private GameOverManager gameOverManager;

    void Awake()
    {
        currentHealth = maxHealth;
        gameOverManager = FindFirstObjectByType<GameOverManager>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (gameOverManager != null)
            {
                gameOverManager.ShowGameOver(CalculateScore());
            }
            Time.timeScale = 0f;
        }
    }

    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }

    private int CalculateScore()
    {
        return Time.frameCount;
    }
}