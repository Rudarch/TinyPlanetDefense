using UnityEngine;

[CreateAssetMenu(fileName = "ExplosiveRoundsUpgrade", menuName = "Upgrades/ExplosiveRounds")]
public class ExplosiveRoundsUpgrade : Upgrade
{
    public float extraRadius = 0.5f;
    public float splashDamageMultiplier = 0.3f;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        if (IsMaxedOut) return;

        var state = Upgrades.Inst.Projectile;
        state.explosiveEnabled = true;
        state.explosionRadius += extraRadius;
        state.splashDamageMultiplier = splashDamageMultiplier;
    }

    public override string GetUpgradeEffectText()
    {
        return $"Enables AoE, +{extraRadius} Radius of effects";
    }
}
