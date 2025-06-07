using UnityEngine;

[CreateAssetMenu(fileName = "RicochetUpgrade", menuName = "Upgrades/Ricochet")]
public class RicochetUpgrade : Upgrade
{
    [SerializeField] int extraRicochets = 1;
    [SerializeField] float extraRange = 0.2f;

    public int ricochetCount;
    public float ricochetRange;
    protected override void ApplyUpgradeInternal()
    {
        ricochetCount = extraRicochets * currentLevel;
        ricochetRange = extraRange * currentLevel;
    }
    public override string GetUpgradeEffectText()
    {
        return $"Bullets ricochet to {GetExtraRicochets} more target(s) and in {GetExtraRange} extra range";
    }

    public override void Initialize()
    {
        ResetUpgrade();
        Upgrades.Inst.ricochet = this;
    }

    float GetExtraRicochets => ricochetCount + extraRicochets;
    float GetExtraRange => ricochetRange + extraRange;
}
