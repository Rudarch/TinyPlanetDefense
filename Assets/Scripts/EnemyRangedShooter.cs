using UnityEngine;

public class EnemyRangedShooter : MonoBehaviour
{
    public Transform planetCenter;
    public GameObject projectilePrefab;
    public float fireCooldown = 2f;
    public float fireForce = 3f;

    private float fireTimer;
    private float shootingRange;
    private EnemyBehavior behavior;

    void Start()
    {
        var cannon = FindFirstObjectByType<AutoCannonController>();
        shootingRange = (cannon != null) ? cannon.firingRange - 0.1f : 2f;
        behavior = GetComponent<EnemyBehavior>();
    }

    void Update()
    {
        if (planetCenter == null || behavior == null) return;

        Vector2 directionToPlanet = (planetCenter.position - transform.position);
        float distance = directionToPlanet.magnitude;

        if (distance > shootingRange)
        {
            Vector2 moveDir = directionToPlanet.normalized;
            transform.position += (Vector3)(moveDir * behavior.movementSpeed * Time.deltaTime);
        }
        else
        {
            float angle = Mathf.Atan2(directionToPlanet.y, directionToPlanet.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                Shoot(directionToPlanet.normalized);
                fireTimer = fireCooldown;
            }
        }
    }

    void Shoot(Vector2 direction)
    {
        if (projectilePrefab != null)
        {
            GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            proj.layer = 7;
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * fireForce;
            }

            Projectile projComp = proj.GetComponent<Projectile>();
            if (projComp != null)
            {
                projComp.isEnemyProjectile = true;
            }
        }
    }
}