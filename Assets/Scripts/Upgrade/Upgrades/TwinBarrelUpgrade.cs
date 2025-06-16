using UnityEngine;

[CreateAssetMenu(fileName = "TwinBarrelUpgrade", menuName = "Upgrades/TwinBarrel")]
public class TwinBarrelUpgrade : Upgrade
{
    protected GameObject planet;
    protected KineticCannon cannon;

    protected void Awake()
    {
        planet = GameObject.FindWithTag("Planet");
        cannon = planet?.GetComponentInChildren<KineticCannon>();
    }

    protected override void ApplyUpgradeInternal()
    {
        cannon?.EnableTwinMuzzles();
    }

    public override string GetUpgradeEffectText()
    {
        return $"More firepower - more fun! For {activationDuration} seconds.";
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
