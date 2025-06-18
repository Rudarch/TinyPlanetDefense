using UnityEngine;

[CreateAssetMenu(fileName = "ThermiteRoundsUpgrade", menuName = "Upgrades/ThermiteRounds")]
public class ThermiteRoundsUpgrade : Upgrade
{
    [Header("Configuration")]
    [SerializeField] [Range(0.1f, 1f)] public float baseDamagePercent = 0.3f;
    [SerializeField] [Range(0.1f, 1f)] public float damagePercentPerLevel = 0.15f;
    [SerializeField] public float burnDuration = 3f;

    [Header("Values")]
    public float thermiteDPSPercent;

    protected override void ApplyUpgradeInternal()
    {
        burnDuration = burnDuration <= 0 ? 1 : burnDuration;
        thermiteDPSPercent = (baseDamagePercent + (damagePercentPerLevel * currentLevel)) / burnDuration;
    }

    public override string GetUpgradeEffectText()
    {
        return $"{(baseDamagePercent + (damagePercentPerLevel * currentLevel)) / burnDuration * 100}% damage over {burnDuration} sec.";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.ThermiteRounds = this;
        thermiteDPSPercent = 0;
    }
}
