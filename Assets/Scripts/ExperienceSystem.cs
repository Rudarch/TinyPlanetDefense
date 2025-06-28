using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ExperienceSystem : MonoBehaviour
{
    [Header("XP Settings")]
    public int currentXP = 0;
    public int currentLevel = 1;
    public int baseXPToLevel = 5;
    public float levelMultiplier = 1.5f;
    public UpgradePopup upgradePopup;
    public GameObject levelUpButton;

    public UnityEvent OnLevelUp;
    public UnityEvent<int> OnPendingLevelUpChanged;
    public UnityEvent<float, float> OnXPChanged;

    private int pendingLevelUps = 0;
    private bool isChoosingUpgrade = false;
    private int xpToNextLevel;
    private AudioSource audioSource;

    public int PendingLevelUps
    {
        get => pendingLevelUps;
        set
        {
            pendingLevelUps = value;
            OnPendingLevelUpChanged.Invoke(pendingLevelUps);
        }
    }

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
            PendingLevelUps++;

            OnLevelUp?.Invoke();

            if (audioSource != null)
            {
                audioSource.Play();
            }

            levelUpButton?.SetActive(true);

            xpToNextLevel = GetXPRequirementForLevel(currentLevel);
        }

        OnXPChanged?.Invoke(currentXP, xpToNextLevel);
    }

    private void TryShowUpgradePopup()
    {
        if (isChoosingUpgrade || PendingLevelUps <= 0 || upgradePopup == null)
            return;

        isChoosingUpgrade = true;
        Time.timeScale = 0f; // Pause game

        upgradePopup.ShowTacticalChoices();
    }

    public void OnUpgradeSelected()
    {
        PendingLevelUps--;
            isChoosingUpgrade = false;
        if (PendingLevelUps > 0)
        {
            TryShowUpgradePopup();
        }
        else
        {
            levelUpButton.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void OnLevelUpButtonPressed()
    {
        TryShowUpgradePopup();
    }

    private int GetXPRequirementForLevel(int level)
    {
        return Mathf.RoundToInt(baseXPToLevel * Mathf.Pow(levelMultiplier, level - 1));
    }
}
