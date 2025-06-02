using UnityEngine;

[CreateAssetMenu(fileName = "HighCaliberUpgrade", menuName = "Upgrades/HighCaliber")]
public class HighCaliberUpgrade : Upgrade
{
    public float knockbackForce = 5f;
    public float scaleMultiplier = 1.3f;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();

        var upgradeStateManager = UpgradeStateManager.Instance;
        var state = upgradeStateManager.ProjectileUpgrades;
        if (state != null)
        {
            state.knockbackEnabled = true;
            state.knockbackForce = knockbackForce;
            state.projectileScale *= scaleMultiplier;

            upgradeStateManager.SetProjectileUpgrades(state);
        }
    }

    public override string GetEffectText()
    {
        return $"Projectiles knock enemies back by {knockbackForce} and are {Mathf.RoundToInt((scaleMultiplier - 1f) * 100)}% larger.";
    }
}
