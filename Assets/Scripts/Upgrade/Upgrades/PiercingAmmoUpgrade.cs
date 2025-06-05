using UnityEngine;

[CreateAssetMenu(fileName = "PiercingAmmoUpgrade", menuName = "Upgrades/PiercingAmmo")]
public class PiercingAmmoUpgrade : Upgrade
{
    public int extraPierce = 1;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        if (IsMaxedOut) return;

        var state = Upgrades.Inst.Projectile;
        state.piercingEnabled = true;
        state.pierceCount += extraPierce;
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{extraPierce} Piercing";
    }
}
