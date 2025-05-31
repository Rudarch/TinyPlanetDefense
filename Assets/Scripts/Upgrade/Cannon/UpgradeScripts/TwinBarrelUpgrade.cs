using UnityEngine;

[CreateAssetMenu(fileName = "TwinBarrelUpgrade", menuName = "Upgrades/TwinBarrel")]
public class TwinBarrelUpgrade : CannonUpgrade
{
    public override void ApplyUpgrade(GameObject cannon)
    {
        base.ApplyUpgrade(cannon);

        var weapon = cannon.GetComponentInChildren<KineticCannon>();
        if (weapon != null)
        {
            weapon.twinBarrelEnabled = true;
            weapon.EnableTwinMuzzles();
        }
    }

    public override string GetEffectText()
    {
        return "Adds a second barrel — 2 bullets per shot";
    }

    private void OnEnable()
    {
        isUnique = true;
    }
}
