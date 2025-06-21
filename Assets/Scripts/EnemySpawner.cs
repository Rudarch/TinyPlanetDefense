
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemySpawner;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnInfo
    {
        public GameObject enemyPrefab;
        public int count = 5;
        public float spawnDuration = 5f;
        public int spawnZoneIndex = -1; // -1 = random
    }

    [System.Serializable]
    public class Wave
    {
        public List<EnemySpawnInfo> enemyGroups;
        public bool waitForEnemiesToDie = true;
    }

    public List<Wave> waves = new();
    public List<BoxCollider2D> spawnZones = new(); // Each zone is a BoxCollider2D
    public Transform enemyParent;
    public Transform planetCenter;
    public WaveProgressUI waveUI;
    public float extraDelayBetweenWaves = 3f;

    private int enemiesAlive = 0;
    private int currentWave = 0;

    void Start()
    {
        if (planetCenter == null)
        {
            GameObject planet = GameObject.FindWithTag("Planet");
            if (planet != null) planetCenter = planet.transform;
        }

        waveUI.Initialize(waves.Count);
        StartCoroutine(SpawnWaveRoutine());
    }

    IEnumerator SpawnWaveRoutine()
    {
        yield return null;

        while (currentWave < waves.Count)
        {
            Wave wave = waves[currentWave];
            waveUI.SetWave(currentWave);

            List<Coroutine> spawnCoroutines = new();
            foreach (var group in wave.enemyGroups)
            {
                spawnCoroutines.Add(StartCoroutine(SpawnEnemyGroup(group)));
            }

            foreach (var co in spawnCoroutines)
                yield return co;

            if (wave.waitForEnemiesToDie)
            {
                while (enemiesAlive > 0)
                    yield return null;
            }
            else
            {
                yield return new WaitForSeconds(extraDelayBetweenWaves);
            }

            currentWave++;
        }
    }

    IEnumerator SpawnEnemyGroup(EnemySpawnInfo group)
    {
        float interval = group.count > 0 ? group.spawnDuration / group.count : 0.5f;

        for (int i = 0; i < group.count; i++)
        {
            SpawnEnemy(group.enemyPrefab, group.spawnZoneIndex);
            enemiesAlive++;
            waveUI.SetEnemiesRemaining(enemiesAlive);
            yield return new WaitForSeconds(interval);
        }
    }

    void SpawnEnemy(GameObject prefab, int zoneIndex)
    {
        BoxCollider2D zone = (zoneIndex >= 0 && zoneIndex < spawnZones.Count)
            ? spawnZones[zoneIndex]
            : spawnZones[Random.Range(0, spawnZones.Count)];

        Vector2 zoneMin = zone.bounds.min;
        Vector2 zoneMax = zone.bounds.max;
        Vector2 randomPos = new Vector2(
            Random.Range(zoneMin.x, zoneMax.x),
            Random.Range(zoneMin.y, zoneMax.y)
        );

        GameObject enemy = Instantiate(prefab, randomPos, Quaternion.identity, enemyParent);
        try
        {
            enemy.GetComponent<Enemy>().OnDeath += HandleEnemyDeath;
        }
        catch(System.Exception ex) 
        {
            Debug.Log($"Excpetion: {ex.Message}");
        }
    }

    private void HandleEnemyDeath()
    {
        enemiesAlive--;
        waveUI.SetEnemiesRemaining(enemiesAlive);
    }
}
