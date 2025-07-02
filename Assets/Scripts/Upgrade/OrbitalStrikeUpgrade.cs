
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "OrbitalStrikeUpgrade", menuName = "Upgrades/OrbitalStrike")]
public class OrbitalStrikeUpgrade : PlanetEffectUpgrade
{
    [SerializeField] private int baseStrikes = 1;
    [SerializeField] private float strikeDelay = 0.2f;
    [SerializeField] private float waveInterval = 4f;
    [SerializeField] private float damage = 50f;

    public AudioClip impactSound;

    protected override void InitializeInternal()
    {
        Upgrades.Inst.OrbitalStrike = this;
    }

    public override string GetUpgradeEffectText()
    {
        return $"• Fires {baseStrikes + NextLevel} orbital beam(s) every {waveInterval:F1} sec dealing {damage} damage.";
    }

    protected override IEnumerator Trigger()
    {
        while (IsActivated)
        {
            yield return new WaitForSeconds(waveInterval);

            var enemies = EnemyManager.Inst.GetAllEnemies();
            if (enemies.Count == 0) continue;

            int count = baseStrikes + CurrentLevel;
            for (int i = 0; i < count; i++)
            {
                var target = enemies[Random.Range(0, enemies.Count)];
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
        }
    }

}
