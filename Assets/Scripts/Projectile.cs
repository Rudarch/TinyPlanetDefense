using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    public float damage = 10f;
    public bool empActivated = false;

    [Header("Visuals")]
    public GameObject ricochetLinePrefab;
    public GameObject explosionEffectPrefab;
    public GameObject empEffectPrefab; //todo add


    private int pierceCount = 0; 
    private int enemiesHit = 0;
    private int ricochetsDone = 0;
    private List<Enemy> hitEnemies = new();
    private Vector2 direction;
    private bool hasHitThisFrame = false;
    private Enemy directHitEnemy = null;
    private ProjectileUpgradeState upgradeState;

    void Start()
    {
        upgradeState = Upgrades.Inst.Projectile;
        damage += upgradeState.bonusDamage;

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
    void LateUpdate()
    {
        hasHitThisFrame = false;
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, upgradeState.explosionRadius);

        foreach (var hit in hits)
        {
            var enemy = hit.GetComponent<Enemy>();
            if (enemy != null && enemy != directHitEnemy)
            {
                float aoeDamage = damage * upgradeState.splashDamageMultiplier;
                enemy.TakeDamage(aoeDamage);

                if (upgradeState.knockbackEnabled)
                {
                    Vector2 awayFromCenter = (enemy.transform.position - transform.position).normalized;
                    enemy.ApplyKnockback(awayFromCenter * upgradeState.knockbackForce);
                }

                if (upgradeState.cryoEnabled)
                {
                    var slowable = enemy.GetComponent<EnemySlow>();
                    if (slowable != null)
                    {
                        slowable.ApplySlow(
                            upgradeState.cryoAmount,
                            upgradeState.cryoDuration);
                    }
                }

                if (upgradeState.thermiteEnabled)
                {
                    var burning = enemy.GetComponent<BurningEffect>();
                    if (burning == null)
                    {
                        burning = enemy.gameObject.AddComponent<BurningEffect>();
                        burning.baseDamagePerSecond = upgradeState.thermiteDPS;
                        burning.burnDuration = upgradeState.thermiteDuration;
                    }

                    burning.ApplyOrRefresh(
                        upgradeState.thermiteDPS, 
                        upgradeState.thermiteDuration);
                }
            }
        }

        if (explosionEffectPrefab != null)
        {
            GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            ShockwaveEffect shock = effect.GetComponent<ShockwaveEffect>();
            if (shock != null)
            {
                shock.maxRadius = upgradeState.explosionRadius * 2f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHitThisFrame) return;
        hasHitThisFrame = true;

        var enemy = other.GetComponent<Enemy>();
        if (enemy == null || hitEnemies.Contains(enemy)) return;

        if (directHitEnemy == null)
            directHitEnemy = enemy;

        hitEnemies.Add(enemy);
        enemy.TakeDamage(damage);

        if (upgradeState.knockbackEnabled)
        {
            Vector2 knockDir = (enemy.transform.position - transform.position).normalized;
            enemy.ApplyKnockback(knockDir * upgradeState.knockbackForce);
        }

        if (upgradeState.cryoEnabled)
        {
            var slowable = enemy.GetComponent<EnemySlow>();
            if (slowable != null)
            {
                slowable.ApplySlow(upgradeState.cryoAmount, upgradeState.cryoDuration);
            }
        }

        if (upgradeState.thermiteEnabled)
        {
            var burning = enemy.GetComponent<BurningEffect>();
            if (burning == null)
            {
                burning = enemy.gameObject.AddComponent<BurningEffect>();
                burning.baseDamagePerSecond = upgradeState.thermiteDPS;
                burning.burnDuration = upgradeState.thermiteDuration;
            }

            burning.ApplyOrRefresh(upgradeState.thermiteDPS, upgradeState.thermiteDuration);
        }

        if (upgradeState.ricochetEnabled && ricochetsDone < upgradeState.ricochetCount)
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

                SetDirection((next.transform.position - (Vector3)transform.position).normalized);
                return;
            }
        }

        enemiesHit++;
        if (enemiesHit > pierceCount)
        {
            if (upgradeState.explosiveEnabled && upgradeState.explosionRadius > 0f)
                Explode();

            if (upgradeState.empEnabled && empActivated)
            {

                var empShockwaveObj = Instantiate(empEffectPrefab, transform.position, Quaternion.identity);
                var empShockwave = empShockwaveObj.GetComponent<EMPShockwaveEffect>();
                if (empShockwave != null)
                {
                    empShockwave.StunNearbyEnemies(transform.position);
                }
            }

            Destroy(gameObject);
        }
    }

    private Enemy FindNextEnemy(Vector3 fromPosition)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, upgradeState.ricochetRange);
        Enemy closest = null;
        float minDist = float.MaxValue;

        foreach (var col in hits)
        {
            var candidate = col.GetComponent<Enemy>();
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
