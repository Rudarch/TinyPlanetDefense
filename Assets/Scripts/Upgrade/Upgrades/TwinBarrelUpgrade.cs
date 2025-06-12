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

    public override void Initialize()
    {
        ResetUpgrade();
        Upgrades.Inst.twinBarrel = this;
    }


    public override void OnActivate()
    {
        base.OnActivate();
        cannon?.EnableTwinMuzzles();
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();
        cannon?.EnableSingleMuzzle();
    }
}
