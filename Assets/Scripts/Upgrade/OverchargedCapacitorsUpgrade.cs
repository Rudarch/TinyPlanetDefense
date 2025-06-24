using System;
using UnityEngine;

[CreateAssetMenu(fileName = "OverchargedCapacitorsUpgrade", menuName = "Upgrades/OverchargedCapacitors")]
public class OverchargedCapacitorsUpgrade : Upgrade
{
    public float maxEnergyPerLevel = 20f;

    public float BonusMaxEnergy
    {
        get
        {
            if (IsActivated) return maxEnergyPerLevel * CurrentLevel;
            else return 0;
        }
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{maxEnergyPerLevel}({maxEnergyPerLevel * NextLevel} total) max energy.";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.OverchargedCapacitors = this;
    }

}