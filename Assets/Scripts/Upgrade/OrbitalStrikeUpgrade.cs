
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "OrbitalStrikeUpgrade", menuName = "Upgrades/OrbitalStrike")]
public class OrbitalStrikeUpgrade : Upgrade
{
    [SerializeField] private int baseStrikes = 1;
    [SerializeField] private float strikeDelay = 0.2f;
    [SerializeField] private float damage = 50f;
    [SerializeField] protected GameObject effectVFX;
    public AudioClip impactSound;

    private Transform planet;

    protected override void InitializeInternal()
    {
        Upgrades.Inst.OrbitalStrike = this;
        planet = GameObject.FindWithTag("Planet")?.transform;
    }

    public override string GetUpgradeEffectText()
    {
        return $"Fires {baseStrikes + NextLevel} orbital beam(s) every {strikeDelay:F1} sec dealing {damage} damage.";
    }

    protected override void ActivateInternal()
    {
        int shotCount = CurrentLevel;
        if (planet.TryGetComponent(out MonoBehaviour mono))
        {
            mono.StartCoroutine(Trigger());
        }
    }

    private IEnumerator Trigger()
    {
        int count = baseStrikes + CurrentLevel;
        for (int i = 0; i < count; i++)
        {
            var target = EnemyManager.Inst. GetRandomEnemy();
            if (target != null)
            {
                GameObject beam = Instantiate(effectVFX, target.transform.position, Quaternion.identity);
                var beamComp = beam.GetComponent<OrbitalStrikeBeam>();
                beamComp?.Strike(target, damage);

                if (impactSound != null)
                    AudioSource.PlayClipAtPoint(impactSound, target.transform.position);
            }

            yield return new WaitForSeconds(strikeDelay);
        }

        Deactivate();
    }

}
