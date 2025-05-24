using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    public float fireCooldownMultiplier = 1f;
    public float damageMultiplier = 1f;
    public float rotationSpeedMultiplier = 1f;
    public float firingRangeMultiplier = 1f;
    public float maxHealthMultiplier = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;
    }

    public void ApplyUpgrade(UpgradeType type, float multiplier)
    {
        switch (type)
        {
            case UpgradeType.FireCooldown:
                fireCooldownMultiplier *= 1f - (0.1f * multiplier);
                break;
            case UpgradeType.Damage:
                damageMultiplier *= 1f + (0.2f * multiplier);
                break;
            case UpgradeType.RotationSpeed:
                rotationSpeedMultiplier *= 1f + (0.1f * multiplier);
                break;
            case UpgradeType.FiringRange:
                firingRangeMultiplier *= 1f + (0.05f * multiplier);
                var cannon = Object.FindFirstObjectByType<AutoCannonController>();
                if (cannon != null)
                {
                    cannon.firingRange *= firingRangeMultiplier;
                }
                break;
            case UpgradeType.MaxHealth:
                maxHealthMultiplier *= 1f + (0.2f * multiplier);
                var player = Object.FindFirstObjectByType<PlayerHealth>();
                if (player != null)
                {
                    player.maxHealth = Mathf.RoundToInt(player.maxHealth * maxHealthMultiplier);
                    player.currentHealth = player.maxHealth;
                }
                break;
        }
    }
}