using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isStunned = false;

    [Header("Stats")]
    public float maxHealth;
    public float health = 5f;
    public float damage = 1f;

    [Header("Movement")]
    public float moveSpeed = 1f;
    public float maxMovementSpeed;
    public bool shouldMove = true;

    [Header("References")]
    public Transform planetTarget;
    public System.Action OnDeath;

    [Header("VFX")]
    public GameObject thrusterFX;
    public GameObject deathExplosionPrefab;
    public GameObject audioPrefab;
    public AudioClip deathAudioClip;

    [SerializeField] private int xpReward = 1;

    private HealthBar healthBar;
    private Rigidbody2D rb;
    private bool isDying = false;
    private List<EnemyAbilityBase> abilities = new();
    private EnemyMovementBase movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        maxHealth = health;
        moveSpeed = maxMovementSpeed;

        abilities = new(GetComponents<EnemyAbilityBase>());
        foreach (var ab in abilities)
            ab.Initialize(this);

        movement = GetComponent<EnemyMovementBase>();
        movement?.Initialize(this);

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
    }
    void Update()
    {
        foreach (var ab in abilities)
            ab.OnUpdate();

        if (shouldMove)
            movement?.TickMovement();
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

    public void UpdateHealth(float amount)
    {
        health -= amount;
        if (health > maxHealth) health = maxHealth;

        foreach (var ab in abilities)
            if (amount > 0) ab.OnDamaged(amount);

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
        if (isDying) return;
        isDying = true;

        foreach (var ab in abilities)
            ab.OnDeath();

        GameObject gameController = GameObject.FindWithTag("GameController");
        if (gameController != null)
        {
            var xpSystem = gameController.GetComponent<ExperienceSystem>();
            if (xpSystem != null)
            {
                xpSystem.AddXP(xpReward);
            }
        }

        var burn = GetComponent<BurningEffect>();
        if (burn != null && burn.IsActive() && Upgrades.Inst.MoltenCollapse?.IsActivated == true)
        {
            Upgrades.Inst.MoltenCollapse.TriggerExplosion(transform.position);
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

        DestroyObject();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Planet"))
        {
            GameObject.FindWithTag("Planet").GetComponent<Planet>().TakeDamage(damage);
            Debug.Log($"Enemy dealt {damage} damage to the planet.");


            DestroyObject();
        }
    }

    private void DestroyObject()
    {
        OnDeath?.Invoke();

        if (deathAudioClip != null && audioPrefab != null)
        {
            GameObject audioObj = Instantiate(audioPrefab, transform.position, Quaternion.identity);
            AudioSource source = audioObj.GetComponent<AudioSource>();
            source.clip = deathAudioClip;
            source.volume = .3f;
            source.pitch = Random.Range(0.8f, 1.2f);
            source.Play();
            Destroy(audioObj, deathAudioClip.length);
        }

        if (deathExplosionPrefab != null)
        {
            var explosion = Instantiate(deathExplosionPrefab, transform.position, Quaternion.identity);
            explosion.transform.SetParent(null);
            Destroy(explosion, 0.5f);
        }

        Destroy(gameObject);
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
