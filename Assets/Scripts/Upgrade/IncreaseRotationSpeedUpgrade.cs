using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseRotationSpeedUpgrade", menuName = "Upgrades/IncreaseRotationSpeed")]
public class IncreaseRotationSpeedUpgrade : Upgrade
{
    public float speedBoostPerLevel = 15f;

    public float RotationBoost { get => speedBoostPerLevel * CurrentLevel; }

    public override string GetUpgradeEffectText()
    {
        return $"+{speedBoostPerLevel}°, {RotationBoost + speedBoostPerLevel}° in total";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.IncreaseRotationSpeed = this;
    }
}