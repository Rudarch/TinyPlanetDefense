using UnityEngine;

[CreateAssetMenu(fileName = "PiercingAmmoUpgrade", menuName = "Upgrades/PiercingAmmo")]
public class PiercingAmmoUpgrade : CannonUpgrade
{
    public int extraPierce = 1;

    public override void ApplyUpgrade(GameObject cannon)
    {
        base.ApplyUpgrade(cannon);

        var weapon = cannon.GetComponentInChildren<KineticCannon>();
        if (weapon != null)
        {
            weapon.extraPierce += extraPierce;
        }
    }

    public override string GetEffectText()
    {
        return $"+{extraPierce} Piercing";
    }
}
