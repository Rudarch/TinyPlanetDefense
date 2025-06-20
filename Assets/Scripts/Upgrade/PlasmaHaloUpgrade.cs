using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PlasmaHaloUpgrade", menuName = "Upgrades/PlasmaHalo")]
public class PlasmaHaloUpgrade : PlanetEffectUpgrade
{
    [Header("Configuration Settings")]
    [SerializeField] float baseDamagePerSecond = 1f;
    [SerializeField] float baseRadius = 1f;
    [SerializeField] float radiusIncreasePerLevel = 1f;
    [SerializeField] float tickInterval = 0.2f;

    [Header("Values")]
    public float damagePerSecond = 1f;
    public float radius = 1f;

    protected override void InitializeInternal()
    {
        base.InitializeInternal();
        Upgrades.Inst.PlasmaHalo = this;
    }

    protected override void ApplyUpgradeInternal()
    {
        base.ApplyUpgradeInternal();
        damagePerSecond = damagePerSecond + (baseDamagePerSecond * currentLevel);
        radius = baseRadius + (radiusIncreasePerLevel * currentLevel);
    }

    protected override void ResetInternal()
    {
        radius = 0f;
        damagePerSecond = 0f;
    }

    public override string GetUpgradeEffectText()
    {
        return $"Continuously damages enemies within {baseRadius + radiusIncreasePerLevel * NextLevel} units.";
    }

    protected override IEnumerator Trigger()
    {
        WaitForSeconds wait = new WaitForSeconds(tickInterval);

        while (IsActivated)
        {
            TriggerWaveEffectVFX(Upgrades.Inst.PlasmaHalo.radius);

            yield return wait;

            var enemies = EnemyManager.Inst.GetEnemiesInRange(Planet.transform.position, Upgrades.Inst.PlasmaHalo.radius);

            foreach (var enemy in enemies)
            {
                enemy.TakeDamage(damagePerSecond * tickInterval);
            }
        }
    }
}
