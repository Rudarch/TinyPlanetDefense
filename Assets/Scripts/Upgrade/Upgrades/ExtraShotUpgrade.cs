using UnityEngine;

[CreateAssetMenu(fileName = "ExtraShotUpgrade", menuName = "Upgrades/ExtraShot")]
public class ExtraShotUpgrade : Upgrade
{
    public int extraShotsAdded = 1;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        var upgradeStateManager = UpgradeStateManager.Instance;
        var state = upgradeStateManager.CannonUpgrades;
        if (state != null)
        {
            state.extraShots += extraShotsAdded;
            upgradeStateManager.SetCannonUpgrades(state);
        }
    }

    public override string GetEffectText()
    {
        return $"+{extraShotsAdded} Extra Shot{(extraShotsAdded > 1 ? "s" : "")}";
    }
}
