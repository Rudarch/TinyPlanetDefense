using UnityEngine;

[CreateAssetMenu(fileName = "CryoShellsUpgrade", menuName = "Upgrades/CryoShells")]
public class CryoShellsUpgrade : Upgrade
{
    public float slowAmount = 0.3f;
    public float slowDuration = 2f;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        var upgradeStateManager = UpgradeStateManager.Instance;
        var state = upgradeStateManager.ProjectileUpgrades;
        if (state != null)
        {
            state.cryoEnabled = true;
            state.cryoAmount = slowAmount;
            state.cryoDuration = slowDuration;
            upgradeStateManager.SetProjectileUpgrades(state);
        }
    }

    public override string GetEffectText()
    {
        return $"Projectiles slow enemies by {slowAmount * 100f}% for {slowDuration} seconds.";
    }

    private void OnEnable()
    {
        isUnique = true;
    }
}
