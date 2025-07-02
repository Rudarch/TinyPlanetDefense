using UnityEngine;

[CreateAssetMenu(fileName = "CannonMasteryUpgrade", menuName = "Upgrades/CannonMastery")]
public class CannonMasteryUpgrade : Upgrade
{
    [SerializeField] private float bonusRotationSpeed = 15f;
    [SerializeField] private float reloadSpeedPercent = 7f;

    private float reloadSpeedMultiplier = 1f;
    protected override void InitializeInternal()
    {
        Upgrades.Inst.CannonMastery = this;
        reloadSpeedMultiplier = 1f;
    }

    public float BonusRotationSpeed => bonusRotationSpeed * CurrentLevel;
    public float ReloadSpeedMultiplier => reloadSpeedMultiplier;

    protected override void ApplyUpgradeInternal()
    {
        reloadSpeedMultiplier = this.CalculateReloadSpeedMultiplier(CurrentLevel);
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{bonusRotationSpeed * NextLevel:F0}°/s Aim Speed, +{(1f - CalculateReloadSpeedMultiplier(NextLevel))*100f:F0}% Reload Speed";
    }
    private float CalculateReloadSpeedMultiplier(int level)
    {

        var finalMultiplier = 1f;
        for (int i = 0; i < level; i++)
        {
            finalMultiplier /= 1f + (reloadSpeedPercent / 100f);
        }

        return finalMultiplier;
    }
}