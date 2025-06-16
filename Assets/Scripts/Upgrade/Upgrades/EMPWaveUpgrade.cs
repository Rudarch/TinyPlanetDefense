using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

[CreateAssetMenu(menuName = "Upgrades/EMPWaveUpgrade")]
public class EMPWaveUpgrade : PlanetEffectUpgrade
{
    protected override void InitializeInternal()
    {
        Upgrades.Inst.empWave = this;
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
            yield return new WaitForSeconds(waveInterval);
            var enemies = EnemyManager.Inst.GetEnemiesInRange(planet.transform.position, Upgrades.Inst.empWave.effectRadius);
            foreach (var enemy in enemies)
            {
                var stunEffect = enemy.GetComponent<EMPStunEffect>();
                stunEffect?.ApplyStun(Upgrades.Inst.empWave.effectValue);
            }

            var fx = GameObject.Instantiate(effectVFX, planet.transform.position, Quaternion.identity);
            var effect = fx.GetComponent<EMPShockwaveEffect>();
            effect.maxRadius = Upgrades.Inst.empWave.effectRadius;
        }
    }
}
