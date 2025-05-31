using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float health = 5f;
    public float damage = 1f;

    [Header("Jump Movement")]
    public float jumpDistance = 1.5f;
    public float jumpInterval = 1.0f;
    public float jumpHeight = 0.5f;

    [Header("References")]
    public Transform planetTarget;
    public System.Action OnDeath;

    [SerializeField] private int xpReward = 1;

    private float maxHealth;
    private HealthBar healthBar;

    private Vector3 jumpStart;
    private Vector3 jumpEnd;
    private float jumpProgress;
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

        StartCoroutine(JumpTowardPlanet());
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

    IEnumerator JumpTowardPlanet()
    {
        while (true)
        {
            yield return new WaitForSeconds(jumpInterval);
            if (planetTarget == null) yield break;

            Vector3 dir = (planetTarget.position - transform.position).normalized;
            jumpStart = transform.position;
            float effectiveJumpDistance = jumpDistance * (moveSpeed / 2f); // 2f is base moveSpeed
            jumpEnd = jumpStart + dir * effectiveJumpDistance;
            jumpProgress = 0f;

            while (jumpProgress < 1f)
            {
                jumpProgress += Time.deltaTime / 0.2f;
                float curved = Mathf.Sin(jumpProgress * Mathf.PI);
                transform.position = Vector3.Lerp(jumpStart, jumpEnd, jumpProgress) + Vector3.up * curved * jumpHeight;
                yield return null;
            }
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
