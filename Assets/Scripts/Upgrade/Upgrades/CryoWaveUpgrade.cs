using UnityEngine;
using static UnityEngine.UI.Image;
using System.Collections;
[CreateAssetMenu(fileName = "CryoWaveUpgrade", menuName = "Upgrades/CryoWave")]
public class CryoWaveUpgrade : PlanetEffectUpgrade
{
    [SerializeField] private float slowDuration = 3f;

    protected override void InitializeInternal()
    {
        base.InitializeInternal();
        Upgrades.Inst.cryoWave = this;
    }
    public override string GetUpgradeEffectText()
    {
        return $"{baseEffectValue + (effectValuePerLevel * NextLevel) * 100}% slow every {waveInterval} sec.";
    }
    protected override IEnumerator Trigger()
    {
        while (IsActivated)
        {
            yield return new WaitForSeconds(waveInterval);
            var enemies = EnemyManager.Inst.GetEnemiesInRange(Planet.transform.position, Upgrades.Inst.cryoWave.effectRadius);
            foreach (var enemy in enemies)
            {
                var slow = enemy.GetComponent<EnemySlow>();
                if (slow != null)
                {
                    slow.ApplySlow(Upgrades.Inst.cryoWave.effectValue, slowDuration);
                }
            }

            var fx = GameObject.Instantiate(effectVFX, Planet.transform.position, Quaternion.identity);
            var effect = fx.GetComponent<FreezeEffect>();
            effect.maxRadius = Upgrades.Inst.cryoWave.effectRadius;
        }
    }
}