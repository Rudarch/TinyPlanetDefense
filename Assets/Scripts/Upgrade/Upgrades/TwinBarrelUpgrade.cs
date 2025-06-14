using UnityEngine;

[CreateAssetMenu(fileName = "TwinBarrelUpgrade", menuName = "Upgrades/TwinBarrel")]
public class TwinBarrelUpgrade : Upgrade
{
    KineticCannon cannon;
    void Start()
    {
        cannon = FindFirstObjectByType<KineticCannon>();
    }

    protected override void ApplyUpgradeInternal()
    {
        cannon?.EnableTwinMuzzles();
    }

    public override string GetUpgradeEffectText()
    {
        return "More firepower - more fun!";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.twinBarrel = this;
    }


    public override void Activate()
    {
        base.Activate();
        cannon?.EnableTwinMuzzles();
    }

    public override void Deactivate()
    {
        base.Deactivate();
        cannon?.EnableSingleMuzzle();
    }
}
