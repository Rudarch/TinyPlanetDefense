using UnityEngine;

[CreateAssetMenu(fileName = "ReduceCooldownUpgrade", menuName = "Upgrades/ReduceCooldown")]
public class ReduceCooldownUpgrade : CannonUpgrade
{
    [Range(0f, 1f)]
    public float cooldownReductionPercent = 0.25f;

    public override void ApplyUpgrade(GameObject cannon)
    {
        base.ApplyUpgrade(cannon);

        var weapon = cannon.GetComponentInChildren<KineticCannon>();
        if (weapon != null)
        {
            float original = weapon.cooldown;
            float reduction = original * cooldownReductionPercent;
            weapon.cooldown = Mathf.Max(0.05f, original - reduction);
        }
    }

    public override string GetEffectText()
    {
        int percent = Mathf.RoundToInt(cooldownReductionPercent * 100f);
        return $"-{percent}% Cooldown";
    }
}
