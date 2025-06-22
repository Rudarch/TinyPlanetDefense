using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Projectile : MonoBehaviour
{
    [Header("Damage Settings")]
    public float minDamage = 8f;
    public float maxDamage = 12f;
    [HideInInspector] public float damage;

    public float speed = 10f;
    public float lifetime = 5f; 
    public GameObject ricochetLinePrefab;
    public GameObject explosionEffectPrefab;
    public GameObject impactFlashPrefab;
    public AudioClip hitSound;

    private Vector2 direction;
    private AudioSource audioSource;
    private List<Enemy> hitEnemies = new();
    private Enemy directHitEnemy = null;
    private int enemiesHit = 0;
    private int ricochetsDone = 0;
    private int pierceCount = 0;
    private bool hasHitThisFrame = false;
    private bool isOvercharged = false;
    private bool isDestroyed = false;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        damage = Random.Range(minDamage, maxDamage);
        damage += Upgrades.Inst.HeavyShells.BonusDamage;
        damage *= Upgrades.Inst.OverheatProtocol.OverheatDamageMultiplier;

        if (isOvercharged)
        {
            damage *= Upgrades.Inst.OverchargedShot.damageMultiplier;
        }

        pierceCount = Upgrades.Inst.PiercingAmmo.IsActivated ? Upgrades.Inst.PiercingAmmo.PierceCount : 0;

        CheckImmediateOverlap();
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void LateUpdate()
    {
        ResetFrame();
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    public void ApplyOvercharge(float scaleMultiplier)
    {
        isOvercharged = true;
        transform.localScale *= scaleMultiplier;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDestroyed) return;
        OnHit(other);
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

    private void HandleHit(Enemy enemy)
    {
        if (isDestroyed) return;

        if (directHitEnemy == null)
            directHitEnemy = enemy;

        hitEnemies.Add(enemy);
        enemy.TakeDamage(damage);
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
                enemy.TakeDamage(aoeDamage);
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
