using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseDamageUpgrade", menuName = "Upgrades/IncreaseDamage")]
public class IncreaseDamageUpgrade : Upgrade
{
    public float bonusDamage = 5f;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        if (IsMaxedOut) return;

        var state = Upgrades.Inst.Projectile;
        state.bonusDamage += bonusDamage;
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{bonusDamage} Damage";
    }

}