using UnityEngine;

public class KineticCannon : WeaponSystem
{
    public GameObject projectilePrefab;
    public float baseDamage = 10f;
    public float bonusDamage = 0f;
    public float cooldown = 2f;
    public int extraPierce = 0;
    public bool explosiveEnabled = false;
    public float explosionRadius = 0f;

    private float lastFireTime = -Mathf.Infinity;

    public override void TryFireAt(Transform target)
    {
        if (Time.time - lastFireTime < cooldown) return;
        lastFireTime = Time.time;

        Vector2 dir = (target.position - transform.position).normalized;
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        var projectile = proj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.damage = baseDamage + bonusDamage;
            projectile.pierceCount = extraPierce;

            projectile.isExplosive = explosiveEnabled;
            projectile.explosionRadius = explosionRadius;

            projectile.SetDirection(dir);
        }
    }
}
