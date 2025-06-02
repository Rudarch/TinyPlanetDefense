using UnityEngine;

[CreateAssetMenu(fileName = "PiercingAmmoUpgrade", menuName = "Upgrades/PiercingAmmo")]
public class PiercingAmmoUpgrade : Upgrade
{
    public int extraPierce = 1;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        var upgradeStateManager = UpgradeStateManager.Instance;
        var state = upgradeStateManager.ProjectileUpgrades;
        if (state != null)
        {
            state.piercingEnabled = true;
            state.pierceCount += extraPierce;
            upgradeStateManager.SetProjectileUpgrades(state);
        }
        else Debug.Log($"{this.GetType().Name} cannot retrieve the state.");
    }

    public override string GetEffectText()
    {
        return $"+{extraPierce} Piercing";
    }
}
