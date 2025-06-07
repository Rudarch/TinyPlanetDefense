using UnityEngine;

[CreateAssetMenu(fileName = "PiercingAmmoUpgrade", menuName = "Upgrades/PiercingAmmo")]
public class PiercingAmmoUpgrade : Upgrade
{
    [SerializeField] int extraPiercePerLevel = 1;

    public int pierceCount;
    protected override void ApplyUpgradeInternal()
    {
        pierceCount = extraPiercePerLevel * currentLevel;
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{extraPiercePerLevel} Piercing. {extraPiercePerLevel * NextLevel} in total";
    }

    public override void Initialize()
    {
        ResetUpgrade();
        Upgrades.Inst.piercingAmmo = this;
        pierceCount = 0;
    }
}
