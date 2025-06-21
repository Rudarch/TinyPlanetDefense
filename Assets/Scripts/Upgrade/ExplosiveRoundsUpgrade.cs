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
    [SerializeField] private float knockbackForce = 1.5f;
    public float splashDamageMultiplier;
    public float explosionRadius;

    protected override void ApplyUpgradeInternal()
    {
        explosionRadius = baseExplosionRadius + (radiusPerLevel * currentLevel);
        splashDamageMultiplier = baseSplashDamageMultiplier + (splashDamageMultiplierPerLevel * currentLevel);
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.ExplosiveRounds = this;
        explosionRadius = 0;
        splashDamageMultiplier = 0;
    }

    public override string GetUpgradeEffectText()
    {
        return $"Explodes in {GetExplosionRadius:F2} radius, {splashDamageMultiplier * 100f:F0}% splash, knockback nearby enemies.";
    }

    float GetExplosionRadius => baseExplosionRadius + radiusPerLevel * NextLevel;

    public void ApplyKnockbackToEnemies(Vector3 origin)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, explosionRadius);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                Vector2 direction = (enemy.transform.position - origin).normalized;
                float distance = Vector2.Distance(enemy.transform.position, origin);
                float distanceFactor = Mathf.Clamp01(1f - (distance / explosionRadius));

                float scaledForce = knockbackForce * distanceFactor;
                enemy.ApplyKnockback(direction * scaledForce);
            }
        }
    }

}
