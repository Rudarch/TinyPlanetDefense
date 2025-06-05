using UnityEngine;

[CreateAssetMenu(fileName = "ReduceCooldownUpgrade", menuName = "Upgrades/ReduceCooldown")]
public class ReduceCooldownUpgrade : Upgrade
{
    [Range(0f, 1f)]
    public float cooldownReductionPercent = 0.25f;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        if (IsMaxedOut) return;

        float multiplier = Mathf.Clamp01(1f - cooldownReductionPercent);

        var state = Upgrades.Inst.Cannon;
        state.cooldownReductionMultiplier = Mathf.Max(0.1f, state.cooldownReductionMultiplier * multiplier);
        state.shotInterval = Mathf.Max(0.05f, state.shotInterval * multiplier);
    }

    public override string GetUpgradeEffectText()
    {
        int percent = Mathf.RoundToInt(cooldownReductionPercent * 100f);
        return $"-{percent}% shooting cooldown";
    }
}
