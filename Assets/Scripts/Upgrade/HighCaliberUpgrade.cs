using UnityEngine;

[CreateAssetMenu(fileName = "HighCaliberUpgrade", menuName = "Upgrades/HighCaliber")]
public class HighCaliberUpgrade : Upgrade
{
    [SerializeField] private float bonusDamage = 2f;

    protected override void InitializeInternal()
    {
        Upgrades.Inst.HighCaliber = this;
    }

    public float BonusDamage => bonusDamage * CurrentLevel;

    public override string GetUpgradeEffectText()
    {
        return $"+{bonusDamage * NextLevel:F1} Projectile Damage";
    }
}