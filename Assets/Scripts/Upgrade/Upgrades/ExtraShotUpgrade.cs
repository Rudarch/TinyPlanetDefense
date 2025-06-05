using UnityEngine;

[CreateAssetMenu(fileName = "ExtraShotUpgrade", menuName = "Upgrades/ExtraShot")]
public class ExtraShotUpgrade : Upgrade
{
    public int extraShotsAdded = 1;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        if (IsMaxedOut) return;

        var state = Upgrades.Inst.Cannon;
        state.extraShots += extraShotsAdded;
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{extraShotsAdded} Extra Shot{(extraShotsAdded > 1 ? "s" : "")}";
    }
}
