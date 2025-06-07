using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string description;
    public Sprite icon;
    public bool enabled = false;
    public int maxLevel = 1;
    public int currentLevel = 0;
    public bool activatable = false;
    public float energyCostPerSecond = 1f;

    public bool IsMaxedOut => currentLevel >= maxLevel;
    protected int NextLevel => currentLevel + 1;

    public abstract void Initialize();
    public void ApplyUpgrade()
    {
        if (IsMaxedOut)
        {
            Debug.LogWarning($"{upgradeName} has reached max level.");
            return;
        }

        ApplyUpgradeInternal();

        enabled = true;
        currentLevel++;
        Debug.Log($"Applied upgrade: (Level {currentLevel}/{maxLevel})");
    }

    protected void ResetUpgrade()
    {
        currentLevel = 0;
        enabled = false;
    }

    public virtual string GetUpgradeEffectText()
    {
        return string.Empty;
    }

    protected virtual void ApplyUpgradeInternal() { }


    public virtual void OnActivate()
    {
        enabled = true;
    }

    public virtual void OnDeactivate()
    {
        enabled = false;
    }
}
