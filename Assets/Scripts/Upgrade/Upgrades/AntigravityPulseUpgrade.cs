using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "AntigravityPulseUpgrade", menuName = "Upgrades/AntigravityPulse")]
public class AntigravityPulseUpgrade : PlanetEffectUpgrade
{
    protected override void InitializeInternal()
    {
        base.InitializeInternal();
        Upgrades.Inst.antigravityPulse = this;
    }

    public override string GetUpgradeEffectText()
    {
        return $"Pushes enemies for {baseEffectValue + (effectValuePerLevel * NextLevel)} force every {waveInterval} sec.";
    }

    protected override IEnumerator Trigger()
    {
        while (IsActivated)
        {
            yield return new WaitForSeconds(waveInterval);
            var enemies = EnemyManager.Inst.GetEnemiesInRange(planet.transform.position, Upgrades.Inst.antigravityPulse.effectRadius);
            foreach (var enemy in enemies)
            {
                Vector2 dir = (enemy.transform.position - planet.transform.position).normalized;
                enemy.ApplyKnockback(dir * Upgrades.Inst.antigravityPulse.effectValue);
            }

            var fx = GameObject.Instantiate(effectVFX, planet.transform.position, Quaternion.identity);
            var effect = fx.GetComponent<AntigravityPulseEffect>();
            effect.maxRadius = Upgrades.Inst.antigravityPulse.effectRadius;
        }
    }
}