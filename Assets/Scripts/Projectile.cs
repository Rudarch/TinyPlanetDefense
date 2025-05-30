using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    public float damage = 10f;

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

    private Vector2 direction;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
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
        var enemy = other.GetComponent<Enemy>();
        if (enemy == null) return;

        enemy.TakeDamage(damage);

        if (knockbackEnabled)
        {
            Vector2 knockDir = (enemy.transform.position - transform.position).normalized;
            enemy.ApplyKnockback(knockDir * knockbackForce);
        }

        enemiesHit++;

        if (enemiesHit > pierceCount)
        {
            if (isExplosive && explosionRadius > 0f)
            {
                Explode();
            }

            Destroy(gameObject);
        }
    }
}
