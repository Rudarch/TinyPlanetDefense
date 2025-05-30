using UnityEngine;

[CreateAssetMenu(fileName = "ExtraShotUpgrade", menuName = "Upgrades/ExtraShot")]
public class ExtraShotUpgrade : CannonUpgrade
{
    public int extraShotsAdded = 1;

    public override void ApplyUpgrade(GameObject cannon)
    {
        base.ApplyUpgrade(cannon);
        var weapon = cannon.GetComponentInChildren<KineticCannon>();
        if (weapon != null)
        {
            weapon.extraShots += extraShotsAdded;
        }
    }

    public override string GetEffectText()
    {
        return $"+{extraShotsAdded} Extra Shot{(extraShotsAdded > 1 ? "s" : "")}";
    }
}
