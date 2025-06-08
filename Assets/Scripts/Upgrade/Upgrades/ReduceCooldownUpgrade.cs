using UnityEngine;

[CreateAssetMenu(fileName = "ReduceCooldownUpgrade", menuName = "Upgrades/ReduceCooldown")]
public class ReduceCooldownUpgrade : Upgrade
{
    [SerializeField] [Range(0f, 1f)] 
    public float cooldownReductionPercent = 0.25f;

    [SerializeField] float baseCooldownReductionMultiplier = 1f;
    [SerializeField] float baseShotInterval = 0.15f;

    private float cooldownReductionMultiplier;
    private float shotInterval;
    protected override void ApplyUpgradeInternal()
    {
        float multiplier = Mathf.Clamp01(1f - cooldownReductionPercent);

        cooldownReductionMultiplier = Mathf.Max(0.1f, cooldownReductionMultiplier * multiplier);
        shotInterval = Mathf.Max(0.05f, shotInterval * multiplier);
    }

    public override string GetUpgradeEffectText()
    {
        return $"-{cooldownReductionPercent * 100f}% shooting cooldown";
    }

    public override void Initialize()
    {
        ResetUpgrade();
        Upgrades.Inst.reduceCooldown = this;
        cooldownReductionMultiplier = baseCooldownReductionMultiplier;
        shotInterval = baseShotInterval;
    }

    public float GetCooldownReductionMultiplier()
    {
        if (enabled) return cooldownReductionMultiplier;
        else return baseCooldownReductionMultiplier;
    }

    public float GetShotInterval()
    {
        if (enabled) return shotInterval;
        else return baseShotInterval;
    }
}
