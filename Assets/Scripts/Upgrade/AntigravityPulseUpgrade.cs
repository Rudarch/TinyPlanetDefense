using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "AntigravityPulseUpgrade", menuName = "Upgrades/AntigravityPulse")]
public class AntigravityPulseUpgrade : PlanetEffectUpgrade
{
    [Header("Configuration Settings")]
    [SerializeField] float basePushBackValue = 2f;
    [SerializeField] float pushBackValuePerLevel = 2f;
    [SerializeField] float pushBackRadius = 2f;
    [SerializeField] float waveInterval = 10f;

    protected override void InitializeInternal()
    {
        Upgrades.Inst.AntigravityPulse = this;
    }

    public float GetPushBackValue()
    {
        return basePushBackValue + (pushBackValuePerLevel * currentLevel);
    }

    public override string GetUpgradeEffectText()
    {
        return $"Pushes enemies for {basePushBackValue + (pushBackValuePerLevel * NextLevel)} force every {waveInterval} sec.";
    }

    protected override IEnumerator Trigger()
    {
        while (IsActivated)
        {
            TriggerWaveEffectVFX(Upgrades.Inst.AntigravityPulse.pushBackRadius);

            yield return new WaitForSeconds(waveInterval);

            var enemies = EnemyManager.Inst.GetEnemiesInRange(Planet.transform.position, Upgrades.Inst.AntigravityPulse.pushBackRadius);
            foreach (var enemy in enemies)
            {
                Vector2 dir = (enemy.transform.position - Planet.transform.position).normalized;
                enemy.ApplyKnockback(dir * GetPushBackValue());
            }
        }
    }
}