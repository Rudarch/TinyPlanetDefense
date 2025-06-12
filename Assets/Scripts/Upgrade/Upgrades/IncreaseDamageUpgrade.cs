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
            if (enabled) return bonusDamage;
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

    public override void Initialize()
    {
        ResetUpgrade();
        Upgrades.Inst.increaseDamage = this;
        bonusDamage = 0f;
    }

}