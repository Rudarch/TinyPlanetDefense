using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "AntigravityPulseUpgrade", menuName = "Upgrades/AntigravityPulse")]
public class AntigravityPulseUpgrade : PlanetEffectUpgrade
{
    [SerializeField] float basePushBackValue = 2f;
    [SerializeField] float pushBackValuePerLevel = 2f;

    public float pushBackValue = 0f;
    public float pushBackRadius = 2f;
    public float waveInterval = 10f;

    protected override void InitializeInternal()
    {
        base.InitializeInternal();
        Upgrades.Inst.AntigravityPulse = this;
    }

    protected override void ApplyUpgradeInternal()
    {
        base.ApplyUpgradeInternal();
        pushBackValue = basePushBackValue + (pushBackValuePerLevel * currentLevel);
    }

    protected override void ResetInternal()
    {
        pushBackValue = 0f;
    }

    public override string GetUpgradeEffectText()
    {
        return $"Pushes enemies for {pushBackValue + (pushBackValuePerLevel * NextLevel)} force every {waveInterval} sec.";
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
                enemy.ApplyKnockback(dir * Upgrades.Inst.AntigravityPulse.pushBackValue);
            }
        }
    }
}