using System;
using UnityEngine;

[CreateAssetMenu(fileName = "OverchargedCapacitorsUpgrade", menuName = "Upgrades/OverchargedCapacitors")]
public class OverchargedCapacitorsUpgrade : Upgrade
{
    private float bonusMaxEnergy = 0f;
    public float maxEnergyPerLevel = 20f;

    public float BonusMaxEnergy
    {
        get
        {
            if (IsActivated) return bonusMaxEnergy;
            else return 0;
        }
    }

    protected override void ApplyUpgradeInternal()
    {
        bonusMaxEnergy = maxEnergyPerLevel * currentLevel;
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{maxEnergyPerLevel}({maxEnergyPerLevel * NextLevel} total) max energy.";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.OverchargedCapacitors = this;
        bonusMaxEnergy = 0f;
    }

}