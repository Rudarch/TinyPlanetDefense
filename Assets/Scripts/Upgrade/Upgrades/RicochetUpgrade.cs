using UnityEngine;

[CreateAssetMenu(fileName = "RicochetUpgrade", menuName = "Upgrades/Ricochet")]
public class RicochetUpgrade : Upgrade
{
    public int extraRicochets = 1;
    public float extraRange = 2f;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        var upgradeStateManager = Upgrades.Inst;
        var state = upgradeStateManager.Projectile;
        if (state != null)
        {
            state.ricochetEnabled = true;
            state.ricochetCount += extraRicochets;
            state.ricochetRange += extraRange;
            upgradeStateManager.SetProjectileUpgrades(state);
        }
    }
    public override string GetEffectText()
    {
        return $"Bullets ricochet to {extraRicochets} more target(s)";
    }
}
