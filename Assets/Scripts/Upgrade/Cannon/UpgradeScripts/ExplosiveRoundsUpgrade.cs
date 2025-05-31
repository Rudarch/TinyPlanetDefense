using UnityEngine;

[CreateAssetMenu(fileName = "ExplosiveRoundsUpgrade", menuName = "Upgrades/ExplosiveRounds")]
public class ExplosiveRoundsUpgrade : CannonUpgrade
{
    public float extraRadius = 0.5f;
    public float splashDamageMultiplier = 0.3f;

    public override void ApplyUpgrade(GameObject cannon)
    {
        base.ApplyUpgrade(cannon);

        var weapon = cannon.GetComponentInChildren<KineticCannon>();
        if (weapon != null)
        {
            weapon.explosiveEnabled = true;
            weapon.explosionRadius += extraRadius;
            weapon.splashDamageMultiplier = splashDamageMultiplier;
        }
    }

    public override string GetEffectText()
    {
        return $"Enables AoE, +{extraRadius} Radius";
    }
}
