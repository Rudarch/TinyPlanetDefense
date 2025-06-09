using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseRotationSpeedUpgrade", menuName = "Upgrades/IncreaseRotationSpeed")]
public class IncreaseRotationSpeedUpgrade : Upgrade
{
    public float speedBoostPerLevel = 15f;
    public float rotationBoost;
    protected override void ApplyUpgradeInternal() 
    {
        rotationBoost = speedBoostPerLevel * currentLevel;
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{speedBoostPerLevel}°/s, {rotationBoost + speedBoostPerLevel} in total";
    }

    public override void Initialize()
    {
        ResetUpgrade();
        Upgrades.Inst.increaseRotationSpeed = this;
        rotationBoost = 0;
    }
}