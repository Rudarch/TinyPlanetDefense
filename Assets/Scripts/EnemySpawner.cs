using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform planetCenter;
    public float spawnBufferDistance = 1f; // This is added to firing range

    private Dictionary<EnemyType, GameObject> prefabLookup;

    void Awake()
    {
        prefabLookup = new Dictionary<EnemyType, GameObject>();
        foreach (var prefab in enemyPrefabs)
        {
            var behavior = prefab.GetComponent<EnemyBehavior>();
            if (behavior != null)
            {
                prefabLookup[behavior.enemyType] = prefab;
            }
        }
    }

    public void SpawnConfiguredWave(WaveConfig config, Dictionary<EnemyType, int> valueMap)
    {
        Dictionary<EnemyType, int> enemyValueMap = valueMap;
        List<EnemyType> types = new List<EnemyType>();
        foreach (var type in config.enemyTypes)
        {
            if (enemyValueMap.ContainsKey(type))
            {
                types.Add(type);
            }
        }

        float spawnDistance = CalculateSpawnDistance();

        int remainingValue = config.totalValue;
        while (remainingValue > 0 && types.Count > 0)
        {
            EnemyType type = types[Random.Range(0, types.Count)];
            int cost = enemyValueMap[type];
            if (cost <= remainingValue)
            {
                if (prefabLookup.TryGetValue(type, out GameObject prefab))
                {
                    SpawnEnemy(prefab, type, spawnDistance);
                    remainingValue -= cost;
                }
            }
        }
    }

    void SpawnEnemy(GameObject prefab, EnemyType type, float radius)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = planetCenter.position + (Vector3)(randomDirection * radius);

        GameObject enemy = Instantiate(prefab, spawnPosition, Quaternion.identity);
        EnemyBehavior behavior = enemy.GetComponent<EnemyBehavior>();
        if (behavior != null)
        {
            behavior.enemyType = type;
            behavior.planetCenter = planetCenter;
        }

        EnemyRangedShooter shooterBehavior = enemy.GetComponent<EnemyRangedShooter>();
        if (shooterBehavior != null)
        {
            shooterBehavior.planetCenter = planetCenter;
        }

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            Vector2 directionToPlanet = (planetCenter.position - spawnPosition).normalized;
            rb.linearVelocity = directionToPlanet * behavior.movementSpeed;
        }
    }

    float CalculateSpawnDistance()
    {
        var cannon = FindFirstObjectByType<AutoCannonController>();
        return (cannon != null) ? cannon.firingRange + spawnBufferDistance : 10f;
    }
}