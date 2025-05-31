using UnityEngine;

[CreateAssetMenu(fileName = "RicochetUpgrade", menuName = "Upgrades/Ricochet")]
public class RicochetUpgrade : CannonUpgrade
{
    public int extraRicochets = 1;
    public float extraRange = 2f;

    public override void ApplyUpgrade(GameObject cannon)
    {
        base.ApplyUpgrade(cannon);

        var weapon = cannon.GetComponentInChildren<KineticCannon>();
        if (weapon != null)
        {
            weapon.ricochetEnabled = true;
            weapon.ricochetCount += extraRicochets;
            weapon.ricochetRange += extraRange;
        }
    }

    public override string GetEffectText()
    {
        return $"Bullets ricochet to {extraRicochets} more target(s)";
    }
}
