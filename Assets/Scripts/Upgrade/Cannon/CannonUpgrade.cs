using UnityEngine;

[CreateAssetMenu(fileName = "NewCannonUpgrade", menuName = "Upgrades/CannonUpgrade")]
public class CannonUpgrade : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string description;
    public Sprite icon;

    public bool isUnique = false;

    protected string effect;
    public virtual void ApplyUpgrade(GameObject cannon)
    {
        Debug.Log($"Applied upgrade: {upgradeName}");
    }

    public virtual string GetEffectText()
    {
        return string.Empty;
    }
}
