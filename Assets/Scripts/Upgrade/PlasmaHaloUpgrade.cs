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

    public float DamagePerSecond { get => baseDamagePerSecond * CurrentLevel; }
    public float Radius { get => baseRadius + (radiusIncreasePerLevel * CurrentLevel); }

    protected override void InitializeInternal()
    {
        base.InitializeInternal();
        Upgrades.Inst.PlasmaHalo = this;
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
            TriggerWaveEffectVFX(Upgrades.Inst.PlasmaHalo.Radius);

            yield return wait;

            var enemies = EnemyManager.Inst.GetEnemiesInRange(Planet.transform.position, Upgrades.Inst.PlasmaHalo.Radius);

            foreach (var enemy in enemies)
            {
                enemy.UpdateHealth(DamagePerSecond * tickInterval);
            }
        }
    }
}
