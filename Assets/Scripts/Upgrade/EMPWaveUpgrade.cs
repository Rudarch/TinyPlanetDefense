using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

[CreateAssetMenu(menuName = "Upgrades/EMPWaveUpgrade")]
public class EMPWaveUpgrade : PlanetEffectUpgrade
{
    [SerializeField] protected float baseEffectValue = 2f;
    [SerializeField] protected float effectValuePerLevel = 2f;

    public float effectValue = 0f;
    public float effectRadius = 2f;
    public float waveInterval = 10f;
    protected override void InitializeInternal()
    {
        Upgrades.Inst.EmpWave = this;
        base.InitializeInternal();

    }

    public override string GetUpgradeEffectText()
    {
        return $"Every {waveInterval} seconds for {effectValue} sec.";
    }


    protected override IEnumerator Trigger()
    {
        while (IsActivated)
        {
            TriggerWaveEffectVFX(Upgrades.Inst.EmpWave.effectRadius);

            yield return new WaitForSeconds(waveInterval);

            var enemies = EnemyManager.Inst.GetEnemiesInRange(Planet.transform.position, Upgrades.Inst.EmpWave.effectRadius);
            foreach (var enemy in enemies)
            {
                var stunEffect = enemy.GetComponent<EMPStunEffect>();
                stunEffect?.ApplyStun(Upgrades.Inst.EmpWave.effectValue);
            }
        }
    }
}
