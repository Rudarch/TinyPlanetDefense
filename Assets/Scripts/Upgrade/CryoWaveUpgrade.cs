using UnityEngine;
using static UnityEngine.UI.Image;
using System.Collections;
[CreateAssetMenu(fileName = "CryoWaveUpgrade", menuName = "Upgrades/CryoWave")]
public class CryoWaveUpgrade : PlanetEffectUpgrade
{
    [Header("Configuration Settings")]
    [SerializeField] float slowDuration = 3f;
    [SerializeField] float baseSlowAmount = 2f;
    [SerializeField] float slowAmountPerLevel = 2f;
    [SerializeField] float radius = 2f;
    [SerializeField] float waveInterval = 10f;

    public float SlowAmount { get => baseSlowAmount + (slowAmountPerLevel * CurrentLevel); }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.CryoWave = this;
    }

    public override string GetUpgradeEffectText()
    {
        return $"{Mathf.Round(baseSlowAmount + (slowAmountPerLevel * NextLevel) * 100)}% slow every {waveInterval} sec.";
    }
    protected override IEnumerator Trigger()
    {
        while (IsActivated)
        {
            TriggerWaveEffectVFX(Upgrades.Inst.CryoWave.radius);

            yield return new WaitForSeconds(waveInterval);

            var enemies = EnemyManager.Inst.GetEnemiesInRange(Planet.transform.position, Upgrades.Inst.CryoWave.radius);
            foreach (var enemy in enemies)
            {
                var slow = enemy.GetComponent<EnemySlow>();
                if (slow != null)
                {
                    slow.ApplySlow(Upgrades.Inst.CryoWave.SlowAmount, slowDuration);
                }
            }
        }
    }
}