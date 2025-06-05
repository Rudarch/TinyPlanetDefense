using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseRotationSpeedUpgrade", menuName = "Upgrades/IncreaseRotationSpeed")]
public class IncreaseRotationSpeedUpgrade : Upgrade
{
    public float speedBoost = 90f;
    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        if (IsMaxedOut) return;

        var state = Upgrades.Inst.Cannon;
        state.rotationSpeed += speedBoost;
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{speedBoost}°/s Rotation Speed";
    }
}