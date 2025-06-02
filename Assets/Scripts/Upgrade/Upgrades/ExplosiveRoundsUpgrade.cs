using UnityEngine;

[CreateAssetMenu(fileName = "ExplosiveRoundsUpgrade", menuName = "Upgrades/ExplosiveRounds")]
public class ExplosiveRoundsUpgrade : Upgrade
{
    public float extraRadius = 0.5f;
    public float splashDamageMultiplier = 0.3f;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();

        var upgradeStateManager = UpgradeStateManager.Instance;
        var state = upgradeStateManager.ProjectileUpgrades;
        if (state != null)
        {
            state.explosiveEnabled = true;
            state.explosionRadius += extraRadius;
            state.splashDamageMultiplier = splashDamageMultiplier;
            upgradeStateManager.SetProjectileUpgrades(state);
        }
    }

    public override string GetEffectText()
    {
        return $"Enables AoE, +{extraRadius} Radius";
    }
}
