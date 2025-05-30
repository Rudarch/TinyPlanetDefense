using System.Collections;
using UnityEngine;

public class KineticCannon : WeaponSystem
{
    public GameObject projectilePrefab;
    public float baseDamage = 10f;
    public float bonusDamage = 0f;
    public float cooldown = 2f;

    [Header("Pierce Shots")]
    public int extraPierce = 0;

    [Header("Explosive Shots")]
    public bool explosiveEnabled = false;
    public float explosionRadius = 0f;

    [Header("Extra Shots")]
    public int extraShots = 0; // Number of additional shots per fire
    public float shotInterval = 0.15f; // Delay between burst shots

    [Header("Knockback")]
    public bool knockbackEnabled = false;
    public float knockbackForce = 5f;


    private bool isFiring = false;

    public override void TryFireAt(Transform target)
    {
        if (isFiring) return; // Prevent overlapping bursts
        StartCoroutine(FireBurst(target));
    }
    private IEnumerator FireBurst(Transform target)
    {
        isFiring = true;

        for (int i = 0; i <= extraShots; i++) // First + extras
        {
            FireSingleShot(target);
            if (i < extraShots) // Wait only between shots, not after last
                yield return new WaitForSeconds(shotInterval);
        }

        yield return new WaitForSeconds(cooldown); // Full cooldown after burst
        isFiring = false;
    }

    private void FireSingleShot(Transform target)
    {
        if (projectilePrefab == null || target == null) return;

        Vector2 dir = (target.position - transform.position).normalized;
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        var projectile = proj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.damage = baseDamage + bonusDamage;
            projectile.pierceCount = extraPierce;
            projectile.isExplosive = explosiveEnabled;
            projectile.explosionRadius = explosionRadius;

            projectile.knockbackEnabled = knockbackEnabled;
            projectile.knockbackForce = knockbackForce;

            projectile.SetDirection(dir);
        }
    }
}
