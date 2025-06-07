using UnityEngine;

[CreateAssetMenu(fileName = "ThermiteRoundsUpgrade", menuName = "Upgrades/ThermiteRounds")]
public class ThermiteRoundsUpgrade : Upgrade
{
    [Range(0.1f, 1f)] public float baseDamagePercent = 0.3f;
    [Range(0.1f, 1f)] public float damagePercentPerLevel = 0.15f;
    public float burnDuration = 3f;
    public float thermiteDPSPercent;

    protected override void ApplyUpgradeInternal()
    {
        thermiteDPSPercent = baseDamagePercent + (damagePercentPerLevel * currentLevel);
    }

    public override string GetUpgradeEffectText()
    {
        return $"Enemies hit are burnedby {baseDamagePercent + (damagePercentPerLevel * NextLevel)}% damage for {burnDuration}s ).";
    }

    public override void Initialize()
    {
        ResetUpgrade();
        Upgrades.Inst.thermiteRounds = this;
        thermiteDPSPercent = baseDamagePercent;
    }
}
