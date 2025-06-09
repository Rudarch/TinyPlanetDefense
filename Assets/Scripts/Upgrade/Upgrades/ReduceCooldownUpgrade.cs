using UnityEngine;

[CreateAssetMenu(fileName = "ReduceCooldownUpgrade", menuName = "Upgrades/ReduceCooldown")]
public class ReduceCooldownUpgrade : Upgrade
{
    [SerializeField] [Range(0f, 1f)] 
    public float cooldownReductionPercent = 0.25f;

    [SerializeField] float baseCooldownReductionMultiplier = 1f;
    //[SerializeField] float baseShotInterval = 0.1f;

    private float cooldownReductionMultiplier;
    protected override void ApplyUpgradeInternal()
    {
        cooldownReductionMultiplier = GetCooldownReductionMultiplierInternal();
    }

    public override string GetUpgradeEffectText()
    {
        return $"-{cooldownReductionPercent * 100f}%, -{100 - (Mathf.Abs(GetCooldownReductionMultiplierInternal()) * 100)}% in total.";
    }

    public override void Initialize()
    {
        ResetUpgrade();
        Upgrades.Inst.reduceCooldown = this;
        cooldownReductionMultiplier = baseCooldownReductionMultiplier;
    }

    public float CooldownReductionMultiplier
    {
        get
        {
            if (enabled) return cooldownReductionMultiplier;
            else return baseCooldownReductionMultiplier;
        }
    }

    private float GetCooldownReductionMultiplierInternal()
    {
        float multiplier = Mathf.Clamp01(1f - cooldownReductionPercent);

        return Mathf.Max(0.1f, cooldownReductionMultiplier * multiplier);
    }
}
