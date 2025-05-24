using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int health;
    public EnemyType enemyType;
    public int experienceReward = 1;

    private void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            AwardTechPoints();
            AwardExperience();
            Destroy(gameObject);
        }
    }

    void AwardTechPoints()
    {
        if (TechPointSettings.Instance != null)
        {
            Vector2Int range = TechPointSettings.Instance.GetTechPointRange(enemyType);
            int earned = Random.Range(range.x, range.y + 1);
            TechPointManager.Instance?.AddTechPoints(earned);
        }
    }

    void AwardExperience()
    {
        LevelManager.Instance?.AddExperience(experienceReward);
    }
}