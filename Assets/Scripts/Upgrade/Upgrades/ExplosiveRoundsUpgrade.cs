using UnityEngine;

[CreateAssetMenu(fileName = "ExplosiveRoundsUpgrade", menuName = "Upgrades/ExplosiveRounds")]
public class ExplosiveRoundsUpgrade : Upgrade
{
    [Header("Configuration")]
    [SerializeField] private float baseExplosionRadius = 0.2f;
    [SerializeField] public float radiusPerLevel = 0.1f;
    [SerializeField] private float baseSplashDamageMultiplier = 0.2f;
    [SerializeField] public float splashDamageMultiplierPerLevel = 0.1f;

    [Header("Values")]
    public float splashDamageMultiplier;
    public float explosionRadius;

    protected override void ApplyUpgradeInternal()
    {
        explosionRadius = baseExplosionRadius + (radiusPerLevel * currentLevel);
        splashDamageMultiplier = baseSplashDamageMultiplier + (splashDamageMultiplierPerLevel * currentLevel);
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.explosiveRounds = this;
        explosionRadius = 0;
        splashDamageMultiplier = 0;
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{radiusPerLevel}, {GetExplosionRadius} total radius for {activationDuration}";
    }

    float GetExplosionRadius => radiusPerLevel * NextLevel;
}
