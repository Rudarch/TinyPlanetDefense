using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AdaptiveFluxRegulatorUpgrade", menuName = "Upgrades/AdaptiveFluxRegulator")]
public class AdaptiveFluxRegulatorUpgrade : Upgrade
{
    private float bonusEnergyRegen = 0f;

    public float energyRegenLevel = 1.5f;

    public float BonusEnergyRegen
    {
        get
        {
            if (IsActivated) return bonusEnergyRegen;
            else return 0;
        }
    }

    protected override void ApplyUpgradeInternal()
    {
        bonusEnergyRegen = energyRegenLevel * currentLevel;
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{bonusEnergyRegen}({energyRegenLevel * NextLevel} total) energy regen.";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.adaptiveFluxRegulator = this;
        bonusEnergyRegen = 0f;
    }

}