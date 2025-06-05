using UnityEngine;

[CreateAssetMenu(fileName = "OverchargedShotUpgrade", menuName = "Upgrades/OverchargedShot")]
public class OverchargedShotUpgrade : Upgrade
{
    public float interval = 5f;
    public float damageMultiplier = 3f;
    public float scaleMultiplier = 1.5f;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        if (IsMaxedOut) return;

        var state = Upgrades.Inst.Projectile;
        state.overchargedEnabled = true;
        state.overchargeInterval = interval;
        state.overchargeDamageMultiplier = damageMultiplier * currentLevel;
        state.overchargeScaleMultiplier = scaleMultiplier;
    }

    public override string GetUpgradeEffectText()
    {
        return $"Every {interval} seconds, your next shot deals {damageMultiplier * (currentLevel + 1)}x damage and is larger.";
    }
}
