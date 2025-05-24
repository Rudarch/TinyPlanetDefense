using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public int currentXP = 0;
    public int currentLevel = 1;
    public int baseXPToLevelUp = 10;
    public float xpMultiplier = 1.5f;

    public UnityEvent onLevelUp = new UnityEvent(); // Fix here

    private int xpToNextLevel;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;

        CalculateXPThreshold();
    }

    public void AddExperience(int xp)
    {
        currentXP += xp;
        if (currentXP >= xpToNextLevel)
        {
            currentLevel++;
            currentXP -= xpToNextLevel;
            CalculateXPThreshold();
            Time.timeScale = 0f;
            onLevelUp?.Invoke();
        }
    }

    public float GetExperiencePercentage()
    {
        return (float)currentXP / xpToNextLevel;
    }

    void CalculateXPThreshold()
    {
        xpToNextLevel = Mathf.RoundToInt(baseXPToLevelUp * Mathf.Pow(xpMultiplier, currentLevel - 1));
    }
}
