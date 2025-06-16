using UnityEngine;

[CreateAssetMenu(fileName = "RicochetUpgrade", menuName = "Upgrades/Ricochet")]
public class RicochetUpgrade : Upgrade
{
    [Header("Configuration")]
    [SerializeField] int extraRicochets = 1;
    [SerializeField] float extraRange = 0.1f;
    public float ricochetDamageMultiplier = 0.7f;

    [Header("Values")]
    public int ricochetCount;
    public float ricochetRange;
    protected override void ApplyUpgradeInternal()
    {
        ricochetCount = extraRicochets * currentLevel;
        ricochetRange = extraRange * currentLevel;
    }
    public override string GetUpgradeEffectText()
    {
        return $"+{extraRicochets}, {GetExtraRicochets} in total, in {GetExtraRange} ricochet range";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.ricochet = this;
        ricochetCount = 0;
        ricochetRange = 0;
    }

    float GetExtraRicochets => ricochetCount + extraRicochets;
    float GetExtraRange => ricochetRange + extraRange;
}
