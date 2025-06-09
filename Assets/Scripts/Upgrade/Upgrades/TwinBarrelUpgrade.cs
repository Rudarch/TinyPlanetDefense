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
        return "More firepower - more fun!";
    }

    public override void Initialize()
    {
        ResetUpgrade();
        Upgrades.Inst.twinBarrel = this;
    }
}
