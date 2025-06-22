using UnityEngine;

[CreateAssetMenu(fileName = "PiercingAmmoUpgrade", menuName = "Upgrades/PiercingAmmo")]
public class PiercingAmmoUpgrade : Upgrade
{
    [SerializeField] int extraPiercePerLevel = 1;

    public int PierceCount { get => extraPiercePerLevel * currentLevel; }


    public override string GetUpgradeEffectText()
    {
        return $"+{extraPiercePerLevel} Piercing. {extraPiercePerLevel * NextLevel} in total";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.PiercingAmmo = this;
    }
}
