using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseDamageUpgrade", menuName = "Upgrades/IncreaseDamage")]
public class IncreaseDamageUpgrade : Upgrade
{
    private float bonusDamage = 0f;
    public float bonusDamagePerLevel = 5f;

    public float BonusDamage
    {
        get
        {
            if (IsEnabled) return bonusDamage;
            else return 0;
        }
    }

    protected override void ApplyUpgradeInternal()
    {
        bonusDamage = bonusDamagePerLevel * currentLevel;
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{bonusDamagePerLevel} Damage. {bonusDamage + bonusDamagePerLevel} in total.";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.increaseDamage = this;
        bonusDamage = 0f;
    }

}