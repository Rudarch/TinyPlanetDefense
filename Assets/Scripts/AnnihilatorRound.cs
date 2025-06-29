
using UnityEngine;

public class AnnihilatorRound : MonoBehaviour
{
    public float speed = 7f;
    public float rotateSpeed = 400f;
    public float lifetime = 6f;
    public float damage = 100f;
    public float targetSearchRadius = 10f;

    private Enemy target;
    private Rigidbody2D rb;
    private ParticleSystem trail;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = FindHighestHealthEnemy();

        trail = GetComponentInChildren<ParticleSystem>();
        if (trail != null)
            trail?.Play();
        else Debug.Log("code102jkf9803434. trail is null. ");

        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            target = FindNearestEnemyWithinRadius(transform.position, targetSearchRadius);
            if (target == null) return;
        }

        Vector2 direction = ((Vector2)target.transform.position - rb.position).normalized;
        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.linearVelocity = transform.up * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.UpdateHealth(damage);
            TriggerDestruction();
        }
    }

    Enemy FindHighestHealthEnemy()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Enemy strongest = null;
        float maxHP = float.MinValue;

        foreach (var e in enemies)
        {
            if (e != null && e.health > maxHP)
            {
                maxHP = e.health;
                strongest = e;
            }
        }

        return strongest;
    }

    Enemy FindNearestEnemyWithinRadius(Vector3 fromPos, float radius)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(fromPos, radius);
        float minDist = float.MaxValue;
        Enemy nearest = null;

        foreach (var hit in hits)
        {
            var candidate = hit.GetComponent<Enemy>();
            if (candidate != null && candidate.gameObject.activeInHierarchy)
            {
                float dist = Vector3.Distance(fromPos, candidate.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = candidate;
                }
            }
        }

        return nearest;
    }
    public void TriggerDestruction(float lifetime = 0f)
    {
        if (trail != null)
        {
            trail.transform.SetParent(null, true);  // Detach with world position
            trail.Stop();
            Destroy(trail.gameObject, 0.5f);  // Let trail fade
        }

        Destroy(gameObject, lifetime); // Destroy projectile AFTER trail is detached
    }
}
