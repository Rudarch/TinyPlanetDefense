using System.Collections.Generic;
using UnityEngine;

public class ProjectileHitHandler : MonoBehaviour
{
    private Projectile projectile;
    private Upgrades upgrades;
    private float damage;
    private int pierceCount;
    private int enemiesHit = 0;
    private int ricochetsDone = 0;
    private bool hasHitThisFrame = false;
    private Enemy directHitEnemy = null;
    private List<Enemy> hitEnemies = new();

    private GameObject ricochetLinePrefab;
    private GameObject explosionEffectPrefab;
    private GameObject impactFlashPrefab;
    private AudioClip hitSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Setup(Projectile proj, Upgrades upgrades, float baseDamage, int pierce, GameObject ricochetFx, GameObject explosionFx, GameObject impactFlashPrefab, AudioClip hitAudio)
    {
        projectile = proj;
        this.upgrades = upgrades;
        damage = baseDamage;
        pierceCount = pierce;
        ricochetLinePrefab = ricochetFx;
        explosionEffectPrefab = explosionFx;
        this.impactFlashPrefab = impactFlashPrefab;
        hitSound = hitAudio;
    }

    public void CheckImmediateOverlap()
    {
        Collider2D[] overlaps = Physics2D.OverlapCircleAll(transform.position, 0.05f);
        foreach (var col in overlaps)
        {
            if (col.TryGetComponent(out Enemy enemy) && !hitEnemies.Contains(enemy))
            {
                HandleHit(enemy);
            }
        }
    }

    public void ResetFrame()
    {
        hasHitThisFrame = false;
    }

    public void OnHit(Collider2D other)
    {
        if (hasHitThisFrame) return;
        hasHitThisFrame = true;

        var enemy = other.GetComponent<Enemy>();
        if (enemy == null || hitEnemies.Contains(enemy)) return;

        HandleHit(enemy);
    }

    private void HandleHit(Enemy enemy)
    {
        if (directHitEnemy == null)
            directHitEnemy = enemy;

        hitEnemies.Add(enemy);
        enemy.TakeDamage(damage);
        if (hitSound != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(hitSound);
        }

        if (upgrades.thermiteRounds.IsActivated)
        {
            var burn = enemy.GetComponent<BurningEffect>();
            if (burn == null)
            {
                burn = enemy.gameObject.AddComponent<BurningEffect>();
                burn.baseDamagePerSecond = damage * upgrades.thermiteRounds.thermiteDPSPercent;
                burn.burnDuration = upgrades.thermiteRounds.burnDuration;
            }

            burn.ApplyOrRefresh(damage * upgrades.thermiteRounds.thermiteDPSPercent, upgrades.thermiteRounds.burnDuration);
        }

        if (upgrades.ricochet.IsActivated && ricochetsDone < upgrades.ricochet.ricochetCount)
        {
            ricochetsDone++;
            Enemy next = FindNextEnemy(enemy.transform.position);
            if (next != null)
            {
                if (ricochetLinePrefab != null)
                {
                    var lineObj = Instantiate(ricochetLinePrefab);
                    var line = lineObj.GetComponent<RicochetLine>();
                    if (line != null)
                        line.SetPoints(transform.position, next.transform.position);
                }

                projectile.SetDirection((next.transform.position - transform.position).normalized);
                return;
            }
        }

        enemiesHit++;
        if (enemiesHit > pierceCount)
        {
            if (upgrades.explosiveRounds.IsActivated && upgrades.explosiveRounds.explosionRadius > 0f)
                Explode();

            Destroy(gameObject, 0.05f);
        }
    }

    private void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, upgrades.explosiveRounds.explosionRadius);
        foreach (var hit in hits)
        {
            var enemy = hit.GetComponent<Enemy>();
            if (enemy != null && enemy != directHitEnemy)
            {
                float aoeDamage = damage * upgrades.explosiveRounds.splashDamageMultiplier;
                enemy.TakeDamage(aoeDamage);

                if (upgrades.thermiteRounds.IsActivated)
                {
                    var burn = enemy.GetComponent<BurningEffect>();
                    if (burn == null)
                    {
                        burn = enemy.gameObject.AddComponent<BurningEffect>();
                        burn.baseDamagePerSecond = damage * upgrades.thermiteRounds.thermiteDPSPercent;
                        burn.burnDuration = upgrades.thermiteRounds.burnDuration;
                    }

                    burn.ApplyOrRefresh(damage * upgrades.thermiteRounds.thermiteDPSPercent, upgrades.thermiteRounds.burnDuration);
                }
            }
        }

        if (explosionEffectPrefab != null)
        {
            GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            var shock = effect.GetComponent<ShockwaveEffect>();
            if (shock != null)
                shock.maxRadius = upgrades.explosiveRounds.explosionRadius * 2f;
        }
    }

    private Enemy FindNextEnemy(Vector3 fromPosition)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, upgrades.ricochet.ricochetRange);
        float minDist = float.MaxValue;
        Enemy closest = null;

        foreach (var hit in hits)
        {
            var candidate = hit.GetComponent<Enemy>();
            if (candidate != null && !hitEnemies.Contains(candidate))
            {
                float dist = Vector3.Distance(candidate.transform.position, fromPosition);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = candidate;
                }
            }
        }

        return closest;
    }
}
