using UnityEngine;
using UnityEngine.Events;

public class CannonXPSystem : MonoBehaviour
{
    [Header("XP Settings")]
    public int currentXP = 0;
    public int currentLevel = 1;
    public int baseXPToLevel = 5;
    public float levelMultiplier = 1.5f;

    public UnityEvent<int> OnLevelUp; // Passes new level
    public UnityEvent<int, int> OnXPChanged;
    public UpgradePopup upgradePopup;
    public GameObject cannonGameObject;

    private int pendingLevelUps = 0;
    private bool isChoosingUpgrade = false;
    private int xpToNextLevel;

    void Start()
    {
        xpToNextLevel = GetXPRequirementForLevel(currentLevel);
        OnXPChanged?.Invoke(currentXP, xpToNextLevel);
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
        else
        {
            OnXPChanged?.Invoke(currentXP, xpToNextLevel);
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = GetXPRequirementForLevel(currentLevel);

        OnLevelUp?.Invoke(currentLevel);
        OnXPChanged?.Invoke(currentXP, xpToNextLevel);

        Debug.Log($"Cannon leveled up! Now level {currentLevel}");

        if (upgradePopup != null)
        {
            upgradePopup.Show(cannonGameObject != null ? cannonGameObject : gameObject);
            Time.timeScale = 0f; // Pause while selecting upgrade (optional)
        }
    }


    private int GetXPRequirementForLevel(int level)
    {
        return Mathf.RoundToInt(baseXPToLevel * Mathf.Pow(levelMultiplier, level - 1));
    }
}
