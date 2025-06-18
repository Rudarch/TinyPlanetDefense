using System;
using UnityEngine;

[CreateAssetMenu(fileName = "OverchargedCapacitorsUpgrade", menuName = "Upgrades/OverchargedCapacitors")]
public class OverchargedCapacitorsUpgrade : Upgrade
{
    private float bonusMaxEnergy = 0f;
    public float maxEnergyLevel = 20f;

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
        bonusMaxEnergy = maxEnergyLevel * currentLevel;
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{bonusMaxEnergy}({maxEnergyLevel * NextLevel} total) max energy.";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.OverchargedCapacitors = this;
        bonusMaxEnergy = 0f;
    }

}