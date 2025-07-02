using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Projectile : BaseProjectile
{
    [Header("Damage Settings")]
    public float minDamage = 8f;
    public float maxDamage = 12f;
    [HideInInspector] public float damage;

    public GameObject ricochetLinePrefab;
    public GameObject explosionEffectPrefab;
    public GameObject impactFlashPrefab;
    public AudioClip hitSound;
    public AudioClip pierceSFX;

    private AudioSource audioSource;
    private List<Enemy> hitEnemies = new();
    private Enemy directHitEnemy = null;
    private int enemiesHit = 0;
    private int ricochetsDone = 0;
    private int pierceCount = 0;
    private bool hasHitThisFrame = false;
    private bool isOvercharged = false;
    private bool isDestroyed = false;

    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        damage = Random.Range(minDamage, maxDamage);
        damage += Upgrades.Inst.HighCaliber.BonusDamage;
        damage *= Upgrades.Inst.OverheatProtocol.OverheatDamageMultiplier;

        if (isOvercharged)
        {
            damage *= Upgrades.Inst.OverchargedShot.damageMultiplier;
        }

        pierceCount = Upgrades.Inst.PiercingAmmo.IsActivated ? Upgrades.Inst.PiercingAmmo.PierceCount : 0;

        CheckImmediateOverlap();

    }

    void LateUpdate()
    {
        ResetFrame();
    }

    public void ApplyOvercharge(float scaleMultiplier)
    {
        isOvercharged = true;
        transform.localScale *= scaleMultiplier;
    }

    public void CheckImmediateOverlap()
    {
        Collider2D[] overlaps = Physics2D.OverlapCircleAll(transform.position, 0.05f);
        foreach (var col in overlaps)
        {
            if (col.TryGetComponent(out Enemy enemy) && !hitEnemies.Contains(enemy))
            {
                HandleHit(enemy);
                break;
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

    protected override void HandleHit(Collider2D other)
    {
        if (isDestroyed) return;
        if (hasHitThisFrame) return;
        hasHitThisFrame = true;

        var enemy = other.GetComponent<Enemy>();
        if (enemy == null || hitEnemies.Contains(enemy)) return;

        HandleHit(enemy); // your full effect stack
    }

    private void HandleHit(Enemy enemy)
    {
        if (directHitEnemy == null)
            directHitEnemy = enemy;

        hitEnemies.Add(enemy);
        enemy.UpdateHealth(damage);
        if (hitSound != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(hitSound);
        }

        if (Upgrades.Inst.ThermiteRounds.IsActivated)
        {
            Upgrades.Inst.ThermiteRounds.SpawnBurnZone(enemy.transform.position, damage);
        }

        if (Upgrades.Inst.Ricochet.IsActivated && ricochetsDone < Upgrades.Inst.Ricochet.RicochetCount)
        {
            ricochetsDone++;
            damage *= Upgrades.Inst.Ricochet.ricochetDamageMultiplier;

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

                SetDirection((next.transform.position - transform.position).normalized);
                return;
            }
        }

        if (Upgrades.Inst.PiercingAmmo.IsActivated && enemiesHit < pierceCount)
        {
            damage *= Upgrades.Inst.PiercingAmmo.PierceDamageMultiplier;

            if (audioSource != null && pierceSFX != null)
            {
                audioSource.pitch = 1f + (0.1f * pierceCount);
                audioSource.PlayOneShot(pierceSFX);
            }
        }

        enemiesHit++;
        if (enemiesHit > pierceCount)
        {
            if (Upgrades.Inst.ExplosiveRounds.IsActivated && Upgrades.Inst.ExplosiveRounds.ExplosionRadius > 0f)
                Explode();

            isDestroyed = true;
            Destroy(gameObject, 0.05f);
        }
    }

    private void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, Upgrades.Inst.ExplosiveRounds.ExplosionRadius);
        foreach (var hit in hits)
        {
            var enemy = hit.GetComponent<Enemy>();
            if (enemy != null && enemy != directHitEnemy)
            {
                float aoeDamage = damage * Upgrades.Inst.ExplosiveRounds.SplashDamageMultiplier;
                enemy.UpdateHealth(aoeDamage);
            }
        }

        if (explosionEffectPrefab != null)
        {
            GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            var shock = effect.GetComponent<WaveEffect>();
            if (shock != null)
                shock.maxRadius = Upgrades.Inst.ExplosiveRounds.ExplosionRadius * 2f;
        }

        Upgrades.Inst.ExplosiveRounds.ApplyKnockbackToEnemies(transform.position);
    }

    private Enemy FindNextEnemy(Vector3 fromPosition)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, Upgrades.Inst.Ricochet.RicochetRange);
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
