using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseDamageUpgrade", menuName = "Upgrades/IncreaseDamage")]
public class IncreaseDamageUpgrade : Upgrade
{
    public float bonusDamage = 5f;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();

        var upgradeStateManager = Upgrades.Inst;
        var state = upgradeStateManager.Projectile;
        if (state != null)
        {
            state.bonusDamage += bonusDamage;
            upgradeStateManager.SetProjectileUpgrades(state);
        }
    }

    public override string GetEffectText()
    {
        return $"+{bonusDamage} Damage";
    }

}