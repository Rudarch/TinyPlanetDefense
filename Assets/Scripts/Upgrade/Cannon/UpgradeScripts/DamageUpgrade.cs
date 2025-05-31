using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseDamageUpgrade", menuName = "Upgrades/IncreaseDamage")]
public class IncreaseDamageUpgrade : CannonUpgrade
{
    public float bonusDamage = 5f;

    public override void ApplyUpgrade(GameObject cannon)
    {
        base.ApplyUpgrade(cannon);
        var weapon = cannon.GetComponentInChildren<KineticCannon>();
        if (weapon != null)
        {
            weapon.bonusDamage += bonusDamage;
        }
    }

    public override string GetEffectText()
    {
        return $"+{bonusDamage} Damage";
    }

}