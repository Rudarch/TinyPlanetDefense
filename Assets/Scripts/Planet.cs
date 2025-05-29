using UnityEngine;
using System;

public class Planet : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public static Action<float, float> OnPlanetHealthChanged;

    void Start()
    {
        currentHealth = maxHealth;
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

    void Die()
    {
        Debug.Log("Planet destroyed!");
        // Trigger game over, effects, etc.
    }
}
