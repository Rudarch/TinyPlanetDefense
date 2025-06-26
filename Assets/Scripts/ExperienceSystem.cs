using UnityEngine;
using UnityEngine.Events;

public class ExperienceSystem : MonoBehaviour
{
    [Header("XP Settings")]
    public int currentXP = 0;
    public int currentLevel = 1;
    public int baseXPToLevel = 5;
    public float levelMultiplier = 1.5f;
    public UpgradePopup upgradePopup;

    public UnityEvent OnLevelUp;
    public UnityEvent<float, float> OnXPChanged;

    private int pendingLevelUps = 0;
    private bool isChoosingUpgrade = false;
    private int xpToNextLevel;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        xpToNextLevel = GetXPRequirementForLevel(currentLevel);
        OnXPChanged?.Invoke(currentXP, xpToNextLevel);
    }

    public void AddXP(int amount)
    {
        currentXP += amount;

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

        if (audioSource != null)
        {
            audioSource.Play();
        }

        isChoosingUpgrade = true;
        Time.timeScale = 0f; // Pause game
        upgradePopup.ShowTacticalChoices();
    }

    public void OnUpgradeSelected()
    {
        pendingLevelUps--;

        if (pendingLevelUps > 0)
        {
            upgradePopup.ShowTacticalChoices();
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
