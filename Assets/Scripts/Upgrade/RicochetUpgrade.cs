using UnityEngine;

[CreateAssetMenu(fileName = "RicochetUpgrade", menuName = "Upgrades/Ricochet")]
public class RicochetUpgrade : Upgrade
{
    [Header("Configuration")]
    [SerializeField] int extraRicochets = 1;
    [SerializeField] float extraRange = 0.1f;
    [SerializeField] float baseRicochetMultiplier = 0.7f;
    [SerializeField] float damageGrowthFactor = 1.1f;

    public float ricochetDamageMultiplier = 0.7f;

    public override string GetUpgradeEffectText()
    {
        float basePercent = baseRicochetMultiplier * 100f;
        float currentPercent = ricochetDamageMultiplier * 100f;

        return $"+{extraRicochets} ({extraRicochets * NextLevel} total) ricochets in {RicochetRange} range. Ricochet damage {currentPercent:F0}%";

    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.Ricochet = this;
        ricochetDamageMultiplier = baseRicochetMultiplier;
    }

    protected override void ApplyUpgradeInternal()
    {
        ricochetDamageMultiplier = baseRicochetMultiplier;
        for (int i = 1; i < CurrentLevel; i++)
        {
            ricochetDamageMultiplier *= damageGrowthFactor;
        }
    }

    public int RicochetCount { get => extraRicochets * CurrentLevel; }
    public float RicochetRange { get => extraRange; }
}