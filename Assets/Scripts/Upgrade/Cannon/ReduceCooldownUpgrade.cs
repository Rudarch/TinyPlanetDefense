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
            weapon.cooldown = Mathf.Max(0.1f, weapon.cooldown * 0.75f);
            weapon.shotInterval = Mathf.Max(0.05f, weapon.shotInterval * 0.75f); // Faster bursts
        }
    }

    public override string GetEffectText()
    {
        int percent = Mathf.RoundToInt(cooldownReductionPercent * 100f);
        return $"-{percent}% Cooldown";
    }
}
