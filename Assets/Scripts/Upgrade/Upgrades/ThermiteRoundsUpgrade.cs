using UnityEngine;

[CreateAssetMenu(fileName = "ThermiteRoundsUpgrade", menuName = "Upgrades/ThermiteRounds")]
public class ThermiteRoundsUpgrade : Upgrade
{
    public float burnDuration = 3f;

    [Range(0.1f, 1f)]
    public float baseDamagePercent = 0.3f;
    [Range(0.1f, 1f)]
    public float damagePercent = 0.1f;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        if (IsMaxedOut) return;

        var state = Upgrades.Inst.Projectile;
        state.thermiteEnabled = true;
        state.thermiteDuration = burnDuration;
        state.thermiteDPSPercent = baseDamagePercent + (damagePercent * currentLevel);
    }
    public override string GetUpgradeEffectText()
    {
        return $"Enemies hit are burnedby {baseDamagePercent + (damagePercent * (currentLevel + 1))}% damage for {burnDuration}s ).";
    }
}
