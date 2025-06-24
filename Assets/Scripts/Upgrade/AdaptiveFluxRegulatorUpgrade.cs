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
            if (IsActivated) return energyRegenLevel * CurrentLevel;
            else return 0;
        }
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{bonusEnergyRegen}({energyRegenLevel * NextLevel} total) energy regen.";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.AdaptiveFluxRegulator = this;
        bonusEnergyRegen = 0f;
    }

}