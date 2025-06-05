using UnityEngine;
using System;

public class Planet : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public GameObject healEffectPrefab;
    public AudioClip healSound;

    public static Action<float, float> OnPlanetHealthChanged;

    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        OnPlanetHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        OnPlanetHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        OnPlanetHealthChanged?.Invoke(currentHealth, maxHealth);

        if (healEffectPrefab != null)
        {
            Instantiate(healEffectPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Heal effect prefab is not assigned.");
        }

        if (healSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(healSound);
        }
        else
        {
            Debug.Log("Heal sound or audio source missing.");
        }
    }

    void Die()
    {
        Debug.Log("Planet destroyed!");
        // Trigger game over, effects, etc.
    }
}
