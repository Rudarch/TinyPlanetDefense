using UnityEngine;

[CreateAssetMenu(fileName = "TwinBarrelUpgrade", menuName = "Upgrades/TwinBarrel")]
public class TwinBarrelUpgrade : Upgrade
{
    [SerializeField] float fireSpeedMultiplier = 0.66f;
    [SerializeField] float spreadAngle = 0f;
    protected GameObject planet;
    protected KineticCannon cannon;

    public float FireSpeedMultiplier { get => fireSpeedMultiplier; }

    public float SpreadAngle { get => spreadAngle; }

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
        return $"Fires from both muzzles with rapid but imprecise shots for {activationDuration} seconds.";
    }


    protected override void InitializeInternal()
    {
        Upgrades.Inst.TwinBarrel = this;
    }


    protected override void ActivateInternal()
    {
        cannon?.EnableTwinMuzzles();
    }

    protected override void DeactivateInternal()
    {
        cannon?.EnableSingleMuzzle();
    }
}
