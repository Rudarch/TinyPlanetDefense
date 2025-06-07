using UnityEngine;

[CreateAssetMenu(fileName = "TwinBarrelUpgrade", menuName = "Upgrades/TwinBarrel")]
public class TwinBarrelUpgrade : Upgrade
{
    protected override void ApplyUpgradeInternal()
    {
        var cannon = FindFirstObjectByType<KineticCannon>();
        cannon?.EnableTwinMuzzles();
    }

    public override string GetUpgradeEffectText()
    {
        return "Adds a second barrel";
    }

    public override void Initialize()
    {
        ResetUpgrade();
        Upgrades.Inst.twinBarrel = this;
    }
}
