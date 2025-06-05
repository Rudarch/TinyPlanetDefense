using UnityEngine;

public class Upgrade : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string description;
    public Sprite icon;

    public int maxLevel = 1;
    public int currentLevel = 0;

    public bool IsMaxedOut => currentLevel >= maxLevel;

    public virtual void ApplyUpgrade()
    {
        if (IsMaxedOut)
        {
            Debug.LogWarning($"{upgradeName} has reached max level.");
            return;
        }

        currentLevel++;
        Debug.Log($"Applied upgrade: (Level {currentLevel}/{maxLevel})");
    }

    public virtual string GetUpgradeEffectText()
    {
        return string.Empty;
    }
}
