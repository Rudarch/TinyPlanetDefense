using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    public float damage = 10f;

    [Header("Visuals")]
    public GameObject ricochetLinePrefab;

    [Header("Pierceing")]
    public int pierceCount = 0; 
    private int enemiesHit = 0;

    [Header("Explosive")]
    public bool isExplosive = false;
    public float explosionRadius = 0f;
    public GameObject explosionEffectPrefab;

    [Header("Knockback")]
    public bool knockbackEnabled = false;
    public float knockbackForce = 5f;

    [Header("Ricochet")]
    public bool enableRicochet = false;
    public int maxRicochets = 1;
    public float ricochetRange = 5f;

    private int ricochetsDone = 0;
    private List<Enemy> hitEnemies = new();
    private Vector2 direction;
    private bool hasHitThisFrame = false;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
    void LateUpdate()
    {
        hasHitThisFrame = false; // reset every frame
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); // Face forward
    }

    void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (var hit in hits)
        {
            var enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        if (explosionEffectPrefab != null)
        {
            GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            ShockwaveEffect shock = effect.GetComponent<ShockwaveEffect>();
            if (shock != null)
            {
                shock.maxRadius = explosionRadius;
            }
            Debug.DrawLine(transform.position, transform.position + Vector3.right * explosionRadius, Color.green, 2f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHitThisFrame) return;
        hasHitThisFrame = true;

        var enemy = other.GetComponent<Enemy>();
        if (enemy == null || hitEnemies.Contains(enemy)) return;

        hitEnemies.Add(enemy);
        enemy.TakeDamage(damage);

        if (knockbackEnabled)
        {
            Vector2 knockDir = (enemy.transform.position - transform.position).normalized;
            enemy.ApplyKnockback(knockDir * knockbackForce);
        }

        if (enableRicochet && ricochetsDone < maxRicochets)
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
                return; // don't destroy yet
            }
        }

        enemiesHit++;
        if (enemiesHit > pierceCount)
        {
            if (isExplosive && explosionRadius > 0f)
                Explode();

            Destroy(gameObject);
        }
    }


    private Enemy FindNextEnemy(Vector3 fromPosition)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, ricochetRange);
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
