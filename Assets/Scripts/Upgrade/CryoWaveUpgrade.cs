using UnityEngine;
using static UnityEngine.UI.Image;
using System.Collections;
[CreateAssetMenu(fileName = "CryoWaveUpgrade", menuName = "Upgrades/CryoWave")]
public class CryoWaveUpgrade : PlanetEffectUpgrade
{
    [SerializeField] private float slowDuration = 3f;
    [SerializeField] protected float baseEffectValue = 2f;
    [SerializeField] protected float effectValuePerLevel = 2f;

    public float effectValue = 0f;
    public float effectRadius = 2f;
    public float waveInterval = 10f;

    protected override void InitializeInternal()
    {
        base.InitializeInternal();
        Upgrades.Inst.CryoWave = this;
    }
    public override string GetUpgradeEffectText()
    {
        return $"{baseEffectValue + (effectValuePerLevel * NextLevel) * 100}% slow every {waveInterval} sec.";
    }
    protected override IEnumerator Trigger()
    {
        while (IsActivated)
        {
            TriggerWaveEffectVFX(Upgrades.Inst.CryoWave.effectRadius);

            yield return new WaitForSeconds(waveInterval);

            var enemies = EnemyManager.Inst.GetEnemiesInRange(Planet.transform.position, Upgrades.Inst.CryoWave.effectRadius);
            foreach (var enemy in enemies)
            {
                var slow = enemy.GetComponent<EnemySlow>();
                if (slow != null)
                {
                    slow.ApplySlow(Upgrades.Inst.CryoWave.effectValue, slowDuration);
                }
            }
        }
    }
}