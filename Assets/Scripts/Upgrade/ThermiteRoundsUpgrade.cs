
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "ThermiteRoundsUpgrade", menuName = "Upgrades/ThermiteRounds")]
public class ThermiteRoundsUpgrade : Upgrade
{
    [Header("Configuration")]
    [SerializeField] GameObject burnZonePrefab;

    [SerializeField] float baseThermiteDPSPercent = 0.1f;
    [SerializeField] float thermiteDPSPercentPerLevel = 0.1f;

    [SerializeField] float baseBurnZoneRadius = .2f;
    [SerializeField] float burnZoneRadiusPercentPerLevel = .2f;

    [Header("Values")]
    public float enemyBurnDuration = 2f;
    public float burnZoneDuration = 3f;
    public float burnZoneRadius = .3f;
    public float thermiteDPSPercent = 0.1f;

    protected override void ApplyUpgradeInternal() 
    {
        thermiteDPSPercent = baseThermiteDPSPercent + thermiteDPSPercentPerLevel * currentLevel;
        burnZoneRadius = baseBurnZoneRadius + burnZoneRadiusPercentPerLevel * currentLevel;
    }

    protected override void ResetInternal()
    {
        thermiteDPSPercent = 0f;
        burnZoneRadius = 0f;
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.ThermiteRounds = this;
    }

    public void SpawnBurnZone(Vector3 position, float damage)
    {
        if (burnZonePrefab != null)
        {
            GameObject zone = Instantiate(burnZonePrefab, position, Quaternion.identity);
            var burn = zone.GetComponent<ThermiteBurnZone>();
            if (burn != null)
            {
                burn.duration = burnZoneDuration;
                burn.radius = burnZoneRadius;
                burn.burnDPS = damage * thermiteDPSPercent;
                burn.burnDuration = enemyBurnDuration;
            }
        }
    }

    public override string GetUpgradeEffectText()
    {
        return $"Burns target and leaves a burning zone that damages other enemies.";
    }
}
