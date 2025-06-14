using UnityEngine;

[CreateAssetMenu(fileName = "ExplosiveRoundsUpgrade", menuName = "Upgrades/ExplosiveRounds")]
public class ExplosiveRoundsUpgrade : Upgrade
{
    public float radiusPerLevel = 0.5f;
    public float splashDamageMultiplier = 0.3f;

    public float explosionRadius;

    protected override void ApplyUpgradeInternal()
    {
        explosionRadius = radiusPerLevel * currentLevel;
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.explosiveRounds = this;
        explosionRadius = 0;
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{radiusPerLevel}, {GetExplosionRadius} total radius";
    }

    float GetExplosionRadius => radiusPerLevel * NextLevel;
}
