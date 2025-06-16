using UnityEngine;
using System.Collections;

public abstract class PlanetEffectUpgrade : Upgrade
{

    [SerializeField] protected GameObject effectVFX;
    [SerializeField] protected float baseEffectValue = 2f;
    [SerializeField] protected float effectValuePerLevel = 2f;

    public float effectValue = 0f;
    public float effectRadius = 2f;
    public float waveInterval = 10f;
    private GameObject planet;

    protected GameObject Planet 
    {
        get
        {
            if (planet == null)
            {
                planet = GameObject.FindWithTag("Planet");
            }

            return planet;
        }
    }

    protected override void InitializeInternal()
    {
        base.InitializeInternal();
        effectValue = 0f;
    }

    protected override void ApplyUpgradeInternal()
    {
        base.ApplyUpgradeInternal();
        effectValue = baseEffectValue + (effectValuePerLevel * currentLevel);
    }

    public override void Activate()
    {
        base.Activate();
        Upgrades.Inst.planetUpgradeHandler.RegisterEffect(this.GetType().Name, Trigger);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        Upgrades.Inst.planetUpgradeHandler.UnregisterEffect(this.GetType().Name);
    }

    protected abstract IEnumerator Trigger();
}
