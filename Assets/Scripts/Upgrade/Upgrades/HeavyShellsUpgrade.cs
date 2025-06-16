using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HeavyShellsUpgrade", menuName = "Upgrades/HeavyShells")]
public class HeavyShellsUpgrade : Upgrade
{
    private float bonusDamage = 0f;
    public float bonusDamagePerLevel = 5f;

    public float BonusDamage
    {
        get
        {
            if (IsActivated) return bonusDamage;
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
        Upgrades.Inst.heavyShells = this;
        bonusDamage = 0f;
    }

}