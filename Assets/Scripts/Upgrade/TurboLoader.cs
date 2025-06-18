using UnityEngine;

[CreateAssetMenu(fileName = "TurboLoader", menuName = "Upgrades/TurboLoader")]
public class TurboLoader : Upgrade
{
    [SerializeField] [Range(0f, 1f)] public float cooldownReductionPercent = 0.25f;
    [SerializeField] float baseCooldownReductionMultiplier = 1f;
    [SerializeField] private float cooldownReductionMultiplier;

    protected override void ApplyUpgradeInternal()
    {
        cooldownReductionMultiplier = GetCooldownReductionMultiplierInternal();
    }

    public override string GetUpgradeEffectText()
    {
        return $"-{cooldownReductionPercent * 100f}%, -{100 - (Mathf.Abs(GetCooldownReductionMultiplierInternal()) * 100)}% in total.";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.ReduceCooldown = this;
        cooldownReductionMultiplier = baseCooldownReductionMultiplier;
    }

    public float CooldownReductionMultiplier
    {
        get
        {
            if (IsActivated) return cooldownReductionMultiplier;
            else return baseCooldownReductionMultiplier;
        }
    }

    private float GetCooldownReductionMultiplierInternal()
    {
        float multiplier = Mathf.Clamp01(1f - cooldownReductionPercent);

        return Mathf.Max(0.1f, cooldownReductionMultiplier * multiplier);
    }
}
