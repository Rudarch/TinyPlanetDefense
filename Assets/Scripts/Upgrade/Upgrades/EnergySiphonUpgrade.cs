using UnityEngine;

[CreateAssetMenu(fileName = "EnergySiphonUpgrade", menuName = "Upgrades/EnergySiphon")]
public class EnergySiphonUpgrade : Upgrade
{
    [Range(0f, 1f)] public float healFraction = 0.01f;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        if (IsMaxedOut) return;

        var state = Upgrades.Inst.Projectile; 
        state.energySiphonEnabled = true;
        state.energySiphonFraction = healFraction;
    }

    public override string GetUpgradeEffectText()
    {
        return $"Kills heal the planet for {healFraction * 100f}% HP.";
    }
}
