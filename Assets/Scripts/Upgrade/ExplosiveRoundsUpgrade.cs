using UnityEngine;

[CreateAssetMenu(fileName = "ExplosiveRoundsUpgrade", menuName = "Upgrades/ExplosiveRounds")]
public class ExplosiveRoundsUpgrade : Upgrade
{
    [Header("Configuration")]
    [SerializeField] private float baseExplosionRadius = 0.2f;
    [SerializeField] public float radiusPerLevel = 0.1f;
    [SerializeField] private float baseSplashDamageMultiplier = 0.2f;
    [SerializeField] public float splashDamageMultiplierPerLevel = 0.1f;
    [SerializeField] private float knockbackForce = 0.2f;

    protected override void InitializeInternal()
    {
        Upgrades.Inst.ExplosiveRounds = this;
    }

    public override string GetUpgradeEffectText()
    {
        return $"Explodes in {GetExplosionRadius:F2} radius, {SplashDamageMultiplier * 100f:F0}% splash, knockback nearby enemies.";
    }

    float GetExplosionRadius => baseExplosionRadius + radiusPerLevel * NextLevel;

    public float SplashDamageMultiplier { get => baseSplashDamageMultiplier + splashDamageMultiplierPerLevel * CurrentLevel; }
    public float ExplosionRadius { get => baseExplosionRadius + radiusPerLevel * CurrentLevel; }
    public float KnockbackForce { get => knockbackForce; }

    public void ApplyKnockbackToEnemies(Vector3 origin)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, ExplosionRadius);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                Vector2 direction = (enemy.transform.position - origin).normalized;
                float distance = Vector2.Distance(enemy.transform.position, origin);
                float distanceFactor = Mathf.Clamp01(1f - (distance / ExplosionRadius));

                float scaledForce = KnockbackForce * distanceFactor;
                enemy.ApplyKnockback(direction * scaledForce);
            }
        }
    }

}
