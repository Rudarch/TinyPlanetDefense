using UnityEngine;

[CreateAssetMenu(fileName = "RicochetUpgrade", menuName = "Upgrades/Ricochet")]
public class RicochetUpgrade : Upgrade
{
    public int extraRicochets = 1;
    public float extraRange = 2f;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        if (IsMaxedOut) return;

        var state = Upgrades.Inst.Projectile;
        state.ricochetEnabled = true;
        state.ricochetCount += extraRicochets;
        state.ricochetRange += extraRange;
    }
    public override string GetUpgradeEffectText()
    {
        return $"Bullets ricochet to {extraRicochets} more target(s) and on extra range {extraRange}";
    }
}
