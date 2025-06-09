using UnityEngine;

[CreateAssetMenu(fileName = "HighCaliberUpgrade", menuName = "Upgrades/HighCaliber")]
public class HighCaliberUpgrade : Upgrade
{
    public float knockbackForce = 5f;
    public float scaleMultiplier = 1.3f;
    public float projectileScale = 1;

    protected override void ApplyUpgradeInternal()
    {
        projectileScale = scaleMultiplier * currentLevel;
    }

    public override void Initialize()
    {
        ResetUpgrade();
        Upgrades.Inst.highCaliber = this;
        projectileScale = 1;
    }

    public override string GetUpgradeEffectText()
    {
        return $"{knockbackForce} total knockback force";
    }
}
