using UnityEngine;

[CreateAssetMenu(fileName = "RicochetUpgrade", menuName = "Upgrades/Ricochet")]
public class RicochetUpgrade : Upgrade
{
    [Header("Configuration")]
    [SerializeField] int extraRicochets = 1;
    [SerializeField] float extraRange = 0.1f;
    public float ricochetDamageMultiplier = 0.7f;

    public override string GetUpgradeEffectText()
    {
        return $"+{extraRicochets}({extraRicochets * NextLevel} total) in {extraRange * NextLevel} range";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.Ricochet = this;
    }

    public int RicochetCount { get => extraRicochets * currentLevel; }
    public float RicochetRange { get => extraRange * currentLevel; }
}
