using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int enemyCount = 5;
        public float spawnDuration = 5f; // Total time to spawn all enemies
    }

    public List<Wave> waves = new List<Wave>();
    public GameObject enemyPrefab;
    public float spawnRadius = 8f;
    public Transform planetCenter;
    public Transform enemyParent;

    private int currentWave = 0;

    void Start()
    {
        if (planetCenter == null)
        {
            GameObject planet = GameObject.FindWithTag("Planet");
            if (planet != null) planetCenter = planet.transform;
        }

        StartCoroutine(SpawnWaveRoutine());
    }

    IEnumerator SpawnWaveRoutine()
    {
        while (currentWave < waves.Count)
        {
            yield return StartCoroutine(SpawnWave(waves[currentWave]));
            currentWave++;
            yield return new WaitForSeconds(5f); // Optional delay between waves
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        float interval = wave.spawnDuration / wave.enemyCount;

        for (int i = 0; i < wave.enemyCount; i++)
        {
            SpawnEnemyAtRandomPosition();
            yield return new WaitForSeconds(interval);
        }
    }

    void SpawnEnemyAtRandomPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPos = planetCenter.position + (Vector3)(randomDirection * spawnRadius);
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, enemyParent);
    }
}
