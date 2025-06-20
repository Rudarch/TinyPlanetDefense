using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

[CreateAssetMenu(menuName = "Upgrades/EMPWaveUpgrade")]
public class EMPWaveUpgrade : PlanetEffectUpgrade
{
    [Header("Configuration Settings")]
    [SerializeField] float baseStunTime = 2f;
    [SerializeField] float effectStunTimePerLevel = 2f;
    [SerializeField] float radius = 2f;
    [SerializeField] float waveInterval = 10f;

    [Header("Values")]
    public float stunTime = 0f;
    protected override void InitializeInternal()
    {
        base.InitializeInternal();
        Upgrades.Inst.EmpWave = this;
    }

    protected override void ApplyUpgradeInternal()
    {
        base.ApplyUpgradeInternal();
        stunTime = baseStunTime + (effectStunTimePerLevel * currentLevel);
    }

    protected override void ResetInternal()
    {
        base.ResetInternal();
        stunTime = 0;
    }

    public override string GetUpgradeEffectText()
    {
        return $"Stuns in {radius} radius for {baseStunTime + (effectStunTimePerLevel * NextLevel)} sec.";
    }


    protected override IEnumerator Trigger()
    {
        while (IsActivated)
        {
            TriggerWaveEffectVFX(Upgrades.Inst.EmpWave.radius);

            var enemies = EnemyManager.Inst.GetEnemiesInRange(Planet.transform.position, Upgrades.Inst.EmpWave.radius);
            foreach (var enemy in enemies)
            {
                var stunEffect = enemy.GetComponent<EMPStunEffect>();
                stunEffect?.ApplyStun(Upgrades.Inst.EmpWave.stunTime);
            }

            yield return new WaitForSeconds(waveInterval);
        }
    }
}
