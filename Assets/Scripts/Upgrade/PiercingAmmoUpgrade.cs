using UnityEngine;

[CreateAssetMenu(fileName = "PiercingAmmoUpgrade", menuName = "Upgrades/PiercingAmmo")]
public class PiercingAmmoUpgrade : Upgrade
{
    [SerializeField] int extraPiercePerLevel = 1;
    [SerializeField] float pierceDamageMultiplier = 1.1f;

    public int PierceCount { get => extraPiercePerLevel * CurrentLevel; }
    public float PierceDamageMultiplier { get => pierceDamageMultiplier; set => pierceDamageMultiplier = value; }

    public override string GetUpgradeEffectText()
    {
        return $"+{extraPiercePerLevel} Piercing({extraPiercePerLevel * NextLevel} total). {pierceDamageMultiplier} times damage increase per penetration.";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.PiercingAmmo = this;
    }
}
