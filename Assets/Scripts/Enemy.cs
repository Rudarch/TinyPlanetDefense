using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float health = 5f;
    public float damage = 1f;

    [Header("Dash Movement")]
    public float dashDistance = 1.5f;
    public float dashInterval = 1.0f;

    [Header("References")]
    public Transform planetTarget;
    public System.Action OnDeath;

    [Header("VFX")]
    public GameObject thrusterFX;

    [SerializeField] private int xpReward = 1;

    private float maxHealth;
    private HealthBar healthBar;
    private Rigidbody2D rb;
    public float moveSpeed = 2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        maxHealth = health;

        if (planetTarget == null)
        {
            GameObject planet = GameObject.FindWithTag("Planet");
            if (planet != null)
                planetTarget = planet.transform;
        }

        healthBar = GetComponentInChildren<HealthBar>(true);
        if (healthBar != null)
        {
            healthBar.SetHealth(health, maxHealth);
            healthBar.gameObject.SetActive(false);
        }

        StartCoroutine(DashTowardPlanet());
    }

    public void ApplyKnockback(Vector2 force)
    {
        if (rb != null)
        {
            StopCoroutine("KnockbackRoutine"); // Prevent stacking
            StartCoroutine(KnockbackRoutine(force));
        }
    }
    private IEnumerator KnockbackRoutine(Vector2 force)
    {
        rb.linearVelocity = Vector2.zero; // Stop current movement
        rb.AddForce(force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.1f); // Duration of knockback

        rb.linearVelocity = Vector2.zero; // Stop enemy again
    }

    IEnumerator DashTowardPlanet()
    {
        while (true)
        {
            float interval = dashInterval / (moveSpeed / 2f);
            yield return new WaitForSeconds(interval);

            if (planetTarget == null) yield break;

            Vector3 dir = (planetTarget.position - transform.position).normalized;
            // Rotate enemy to face the planet
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); // Adjust by -90 if sprite faces "up"

            float effectiveDashDistance = dashDistance * (moveSpeed / 2f);
            Vector3 targetPos = transform.position + dir * effectiveDashDistance;

            if (thrusterFX != null) thrusterFX.SetActive(true);

            float dashDuration = 0.2f;
            float timer = 0f;
            Vector3 start = transform.position;

            while (timer < dashDuration)
            {
                timer += Time.deltaTime;
                float t = timer / dashDuration;
                transform.position = Vector3.Lerp(start, targetPos, t);
                yield return null;
            }

            if (thrusterFX != null) thrusterFX.SetActive(false);
        }
    }



    public void TakeDamage(float amount)
    {
        health -= amount;

        if (healthBar != null)
        {
            if (!healthBar.gameObject.activeSelf)
                healthBar.gameObject.SetActive(true);

            healthBar.SetHealth(health, maxHealth);
        }

        if (health <= 0f)
            Die();
    }
    void Die()
    {
        // Award XP to cannon
        GameObject cannon = GameObject.FindWithTag("Cannon");
        if (cannon != null)
        {
            var xpSystem = cannon.GetComponent<CannonXPSystem>();
            if (xpSystem != null)
            {
                xpSystem.AddXP(xpReward);
            }
        }

        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Planet"))
        {
            GameObject.FindWithTag("Planet").GetComponent<Planet>().TakeDamage(damage);
            Debug.Log($"Enemy dealt {damage} damage to the planet.");
            Destroy(gameObject);
        }
    }
}
