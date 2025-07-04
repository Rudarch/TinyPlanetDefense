using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerAbility : EnemyAbilityBase
{
    [System.Serializable]
    public class WeightedEnemyEntry
    {
        public GameObject prefab;
        public float relativeChance = 1f; // Default to 1 if not set
    }

    public List<WeightedEnemyEntry> enemyPrefabs;
    public float spawnInterval = 5f;
    public int maxSpawns = 5;
    public Transform spawnPointOverride;

    private float timer = 0f;
    private int spawned = 0;

    public override void OnUpdate()
    {
        if (enemy == null || enemyPrefabs == null || enemyPrefabs.Count == 0 || enemy.IsStunned()) return;

        timer -= Time.deltaTime;
        if (timer <= 0f && spawned < maxSpawns)
        {
            timer = spawnInterval;

            GameObject prefabToSpawn = GetRandomPrefab();
            if (prefabToSpawn == null) return;

            Vector3 spawnPosition = spawnPointOverride != null
                ? spawnPointOverride.position
                : transform.position;

            GameObject spawnedObj = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            var newEnemy = spawnedObj.GetComponent<Enemy>();
            if (newEnemy != null)
            {
                newEnemy.OnDeath += () => EnemyManager.Inst?.UnregisterEnemy(newEnemy);
                EnemyManager.Inst?.RegisterEnemy(newEnemy);
            }

            spawned++;
        }
    }

    private GameObject GetRandomPrefab()
    {
        float totalWeight = 0f;
        foreach (var entry in enemyPrefabs)
        {
            if (entry.prefab != null && entry.relativeChance > 0)
                totalWeight += entry.relativeChance;
        }

        if (totalWeight <= 0f) return null;

        float roll = Random.value * totalWeight;
        float cumulative = 0f;

        foreach (var entry in enemyPrefabs)
        {
            if (entry.prefab == null || entry.relativeChance <= 0f) continue;

            cumulative += entry.relativeChance;
            if (roll <= cumulative)
                return entry.prefab;
        }

        return null; // fallback, shouldn’t happen
    }
}
