using UnityEngine;

[CreateAssetMenu(fileName = "CannonMasteryUpgrade", menuName = "Upgrades/CannonMastery")]
public class CannonMasteryUpgrade : Upgrade
{
    [SerializeField] private float bonusRotationSpeed = 15f;
    [SerializeField] private float reloadSpeedPercent = 7f;

    protected override void InitializeInternal()
    {
        Upgrades.Inst.CannonMastery = this;
    }

    public float BonusRotationSpeed => bonusRotationSpeed * CurrentLevel;
    public float ReloadSpeedMultiplier => 1f + (reloadSpeedPercent / 100f * CurrentLevel);

    public override string GetUpgradeEffectText()
    {
        return $"+{bonusRotationSpeed * NextLevel:F0}°/s Aim Speed, +{reloadSpeedPercent * NextLevel:F0}% Reload Speed";
    }
}