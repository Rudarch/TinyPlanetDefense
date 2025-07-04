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

    public float StunTime { get => baseStunTime + (effectStunTimePerLevel * CurrentLevel); }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.EmpWave = this;
    }

    public override string GetUpgradeEffectText()
    {
        return $"{baseStunTime + (effectStunTimePerLevel * NextLevel)} seconds stuns in {radius} radius";
    }


    protected override IEnumerator Trigger()
    {
        TriggerWaveEffectVFX(Upgrades.Inst.EmpWave.radius);

        var enemies = EnemyManager.Inst.GetEnemiesInRange(Planet.transform.position, Upgrades.Inst.EmpWave.radius);
        foreach (var enemy in enemies)
        {
            var stunEffect = enemy.GetComponent<EMPStunEffect>();
            stunEffect?.ApplyStun(Upgrades.Inst.EmpWave.StunTime);
        }

        yield return new WaitForSeconds(0);
    }
}
