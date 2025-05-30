using System.Collections;
using UnityEngine;

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

    private float maxHealth;
    private HealthBar healthBar;

    private Vector3 jumpStart;
    private Vector3 jumpEnd;
    private float jumpProgress;
    [SerializeField] private int xpReward = 1;
    public System.Action OnDeath;

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

    IEnumerator JumpTowardPlanet()
    {
        while (true)
        {
            yield return new WaitForSeconds(jumpInterval);
            if (planetTarget == null) yield break;

            Vector3 dir = (planetTarget.position - transform.position).normalized;
            jumpStart = transform.position;
            jumpEnd = jumpStart + dir * jumpDistance;
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
