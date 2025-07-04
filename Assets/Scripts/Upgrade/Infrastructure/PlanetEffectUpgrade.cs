using UnityEngine;
using System.Collections;

public abstract class PlanetEffectUpgrade : Upgrade
{
    [SerializeField] protected GameObject effectVFX;
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

    protected override void ActivateInternal()
    {
        PlanetUpgradeHandler.Inst.RegisterEffect(this.GetType().Name, Trigger);
    }

    protected override void DeactivateInternal()
    {
        PlanetUpgradeHandler.Inst.UnregisterEffect(this.GetType().Name);
    }

    protected virtual void TriggerWaveEffectVFX(float radius)
    {

        var fx = GameObject.Instantiate(effectVFX, Planet.transform.position, Quaternion.identity);
        var effect = fx.GetComponent<WaveEffect>();
        effect.maxRadius = radius;
    }

    protected abstract IEnumerator Trigger();
}
