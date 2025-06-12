using UnityEngine;
using UnityEngine.Events;

public class CannonXPSystem : MonoBehaviour
{
    [Header("XP Settings")]
    public int currentXP = 0;
    public int currentLevel = 1;
    public int baseXPToLevel = 5;
    public float levelMultiplier = 1.5f;
    public UnityEvent OnLevelUp;
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

        // Queue all level-ups that can happen with this XP
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            currentLevel++;
            pendingLevelUps++;

            OnLevelUp?.Invoke();

            xpToNextLevel = GetXPRequirementForLevel(currentLevel);
        }

        OnXPChanged?.Invoke(currentXP, xpToNextLevel);

        TryShowUpgradePopup();
    }

    private void TryShowUpgradePopup()
    {
        if (isChoosingUpgrade || pendingLevelUps <= 0 || upgradePopup == null)
            return;

        isChoosingUpgrade = true;
        Time.timeScale = 0f; // Pause game
        upgradePopup.Show(cannonGameObject != null ? cannonGameObject : gameObject);
    }

    public void OnUpgradeSelected()
    {
        pendingLevelUps--;

        if (pendingLevelUps > 0)
        {
            upgradePopup.Show(this.gameObject);
        }
        else
        {
            isChoosingUpgrade = false;
            Time.timeScale = 1f;
        }
    }


    private int GetXPRequirementForLevel(int level)
    {
        return Mathf.RoundToInt(baseXPToLevel * Mathf.Pow(levelMultiplier, level - 1));
    }
}
