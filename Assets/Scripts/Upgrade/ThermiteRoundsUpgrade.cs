
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

    public float BurnZoneRadius { get => baseBurnZoneRadius + burnZoneRadiusPercentPerLevel * currentLevel; }
    public float ThermiteDPSPercent { get => baseThermiteDPSPercent + thermiteDPSPercentPerLevel * currentLevel; }

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
                burn.radius = BurnZoneRadius;
                burn.burnDPS = damage * ThermiteDPSPercent;
                burn.burnDuration = enemyBurnDuration;
            }
        }
    }

    public override string GetUpgradeEffectText()
    {
        return $"Burns target and leaves a burning zone({baseBurnZoneRadius + burnZoneRadiusPercentPerLevel * NextLevel}) that damages({baseThermiteDPSPercent + thermiteDPSPercentPerLevel * NextLevel}%/s) other enemies.";
    }
}
