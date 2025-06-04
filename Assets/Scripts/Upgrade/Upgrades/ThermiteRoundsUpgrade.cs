using UnityEngine;

[CreateAssetMenu(fileName = "ThermiteRoundsUpgrade", menuName = "Upgrades/ThermiteRounds")]
public class ThermiteRoundsUpgrade : Upgrade
{
    public float burnDuration = 3f;
    public float dps = 1f;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        var upgradeStateManager = Upgrades.Inst;
        var state = upgradeStateManager.Projectile;
        if (state != null)
        {
            state.thermiteEnabled = true;
            state.thermiteDuration = burnDuration;
            state.thermiteDPS = dps;
            upgradeStateManager.SetProjectileUpgrades(state);
        }
    }
    public override string GetEffectText()
    {
        return $"Enemies hit are burned for {burnDuration}s ({dps}/s).";
    }
}
