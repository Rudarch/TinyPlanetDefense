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
        return $"+{speedBoostPerLevel}°, {rotationBoost + speedBoostPerLevel}° in total";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.IncreaseRotationSpeed = this;
        rotationBoost = 0;
    }
}