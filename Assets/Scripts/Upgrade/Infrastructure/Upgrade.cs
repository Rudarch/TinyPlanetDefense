using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string description;
    public Sprite icon;
    public int maxLevel = 1;
    public int currentLevel = 0;
    public bool activatable = false;
    public float energyCostPerSecond = 0f;
    public float energyCostIncreasePerLevel = 1f;
    public bool IsMaxedOut => currentLevel >= maxLevel;

    protected bool enabled = false;
    protected int NextLevel => currentLevel + 1;

    public bool IsEnabled { get => enabled; protected set => enabled = value; }

    public virtual void Initialize()
    {
        ResetUpgrade();
        InitializeInternal();
    }

    public void ApplyUpgrade()
    {
        if (IsMaxedOut)
        {
            Debug.LogWarning($"{upgradeName} has reached max level.");
            return;
        }

        energyCostPerSecond += GetEnergyCostIncreaseForNextLevel();

        currentLevel++;
        ApplyUpgradeInternal();

        Debug.Log($"Applied upgrade: (Level {currentLevel}/{maxLevel})");
    }

    protected void ResetUpgrade()
    {
        currentLevel = 0;
        IsEnabled = false;
        energyCostPerSecond = 0;
    }

    public virtual string GetUpgradeEffectText()
    {
        return string.Empty;
    }

    public virtual float GetEnergyCostIncreaseForNextLevel() 
    {
        return NextLevel > 1
            ? (energyCostIncreasePerLevel / 2f)
            : energyCostIncreasePerLevel;
    }

    protected virtual void ApplyUpgradeInternal() { }

    protected virtual void InitializeInternal() { }

    public virtual void Activate()
    {
        IsEnabled = true;
    }

    public virtual void Deactivate()
    {
        IsEnabled = false;
    }
}
