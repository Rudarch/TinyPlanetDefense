using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isStunned = false;

    [Header("Stats")]
    [HideInInspector] public float baseHealth;
    [HideInInspector] public float maxHealth;
    public float health = 5f;
    public float damage = 1f;
    float healthMultiplier = 1f;

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
    public GameObject deathExplosionWavePrefab;
    public GameObject audioPrefab;
    public AudioClip deathAudioClip;

    [SerializeField] private int xpReward = 1;

    private EnemyModifierType modifierType;
    private HealthBar healthBar;
    private Rigidbody2D rb;
    private EnemyModifierIcons modifierIconManager;
    private bool isDying = false;
    private List<EnemyAbilityBase> abilities = new();
    private EnemyMovementBase movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        modifierIconManager = GetComponentInChildren<EnemyModifierIcons>();
    }

    void Start()
    {
        health *= healthMultiplier;
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

    public void ApplyHealthScaling(float multiplier)
    {
        healthMultiplier = multiplier;
    }

    public void ApplyModifier(EnemyModifierType modifier)
    {
        this.modifierType = modifier;
        if (modifierIconManager != null)
        {
            modifierIconManager.SetModifierIcon(modifier);
        }
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
                float xpBonus = 1f;

                switch (modifierType)
                {
                    case EnemyModifierType.Elite:
                        xpBonus = 2f;
                        break;
                    case EnemyModifierType.Armored:
                    case EnemyModifierType.Fast:
                    case EnemyModifierType.Regenerating:
                    case EnemyModifierType.Exploding:
                        xpBonus = 1.3f;
                        break;
                }

                int totalXP = Mathf.RoundToInt(xpReward * xpBonus);
                xpSystem.AddXP(totalXP);
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
            explosion.transform.localScale = transform.localScale;
            Destroy(explosion, 0.5f);
        }

        if (deathExplosionWavePrefab != null)
        {
            var explosionWave = Instantiate(deathExplosionWavePrefab, transform.position, Quaternion.identity);
            var wave = explosionWave.GetComponent<WaveEffect>();
            if (wave != null)
            {
                wave.maxRadius = transform.localScale.magnitude;
            }
            explosionWave.transform.SetParent(null);
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
