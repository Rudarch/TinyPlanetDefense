using UnityEngine;

public class Upgrade : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string description;
    public Sprite icon;

    public bool isUnique = false;

    public virtual void ApplyUpgrade()
    {
        Debug.Log($"Applied upgrade: {upgradeName}");
    }

    public virtual string GetEffectText()
    {
        return string.Empty;
    }
}
