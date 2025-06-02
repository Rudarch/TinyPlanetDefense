using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseRotationSpeedUpgrade", menuName = "Upgrades/IncreaseRotationSpeed")]
public class IncreaseRotationSpeedUpgrade : Upgrade
{
    public float speedBoost = 90f;
    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();

        var upgradeStateManager = UpgradeStateManager.Instance;
        var state = upgradeStateManager.CannonUpgrades;
        if (state != null)
        {
            state.rotationSpeed += speedBoost;
            upgradeStateManager.SetCannonUpgrades(state);
        }
    }

    public override string GetEffectText()
    {
        return $"+{speedBoost}°/s Rotation Speed";
    }
}