using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isStunned = false;

    [Header("Stats")]
    public float health = 5f;
    public float damage = 1f;

    [Header("Dash Movement")]
    public float dashDistance = 1.5f;
    public float dashInterval = 1.0f;
    public float moveSpeed = 1f;

    [Header("References")]
    public Transform planetTarget;
    public System.Action OnDeath;

    [Header("VFX")]
    public GameObject thrusterFX;
    public GameObject deathExplosionPrefab;

    [SerializeField] private int xpReward = 1;

    private float maxHealth;
    private HealthBar healthBar;
    private Rigidbody2D rb;
    private Coroutine dashRoutine;

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

        dashRoutine = StartCoroutine(DashTowardPlanet());
    }

    public void ApplyKnockback(Vector2 force)
    {
        if (rb != null)
        {
            StopCoroutine("KnockbackRoutine");
            StartCoroutine(KnockbackRoutine(force));
        }
    }

    private IEnumerator KnockbackRoutine(Vector2 force)
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.1f);

        rb.linearVelocity = Vector2.zero;
    }

    IEnumerator DashTowardPlanet()
    {
        while (true)
        {
            if (isStunned)
            {
                yield return null;
                continue;
            }

            float interval = dashInterval;
            yield return new WaitForSeconds(interval);

            if (planetTarget == null) yield break;

            Vector3 dir = (planetTarget.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

            float effectiveDashDistance = dashDistance * moveSpeed;
            Vector3 targetPos = transform.position + dir * effectiveDashDistance;

            if (thrusterFX != null) thrusterFX.SetActive(true);

            float dashDuration = 0.2f;
            float timer = 0f;
            Vector3 start = transform.position;

            while (timer < dashDuration)
            {
                if (isStunned)
                    break;

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
        GameObject gameController = GameObject.FindWithTag("GameController");
        if (gameController != null)
        {
            var xpSystem = gameController.GetComponent<ExperienceSystem>();
            if (xpSystem != null)
            {
                xpSystem.AddXP(xpReward);
            }
        }

        OnDeath?.Invoke();

        if (deathExplosionPrefab != null)
        {
            Instantiate(deathExplosionPrefab, transform.position, Quaternion.identity);
        }

        if (Upgrades.Inst.LifeSiphon.IsActivated)
        {
            var planet = GameObject.FindWithTag("Planet");
            if (planet != null)
            {
                var planetComp = planet.GetComponent<Planet>();
                if (planetComp != null)
                {
                    float healAmount = planetComp.maxHealth * Upgrades.Inst.LifeSiphon.lifeSiphonFraction;
                    planetComp.Heal(healAmount);
                }
            }
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Planet"))
        {
            GameObject.FindWithTag("Planet").GetComponent<Planet>().TakeDamage(damage);
            Debug.Log($"Enemy dealt {damage} damage to the planet.");

            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }

    public void SetStunned(bool stunned)
    {
        isStunned = stunned;
    }

    public bool IsStunned()
    {
        return isStunned;
    }

    void OnEnable()
    {
        EnemyManager.Inst?.RegisterEnemy(this);
    }

    void OnDisable()
    {
        EnemyManager.Inst?.UnregisterEnemy(this);
    }
}
