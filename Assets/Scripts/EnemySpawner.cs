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
    public WaveProgressUI waveUI; 
    
    [Header("Wave Timing")]
    public float extraDelayBetweenWaves = 3f;

    private int enemiesAlive;
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
        waveUI.SetNextWaveTimer(0);
        yield return new WaitForSeconds(0);

        while (currentWave < waves.Count)
        {
            waveUI.SetWave(currentWave);
            yield return StartCoroutine(SpawnWave(waves[currentWave]));

            currentWave++;

            if (currentWave < waves.Count)
            {
                float timeToNextWave = waves[currentWave].spawnDuration + extraDelayBetweenWaves;
                waveUI.SetNextWaveTimer(timeToNextWave);
                yield return new WaitForSeconds(timeToNextWave);
            }
            else
            {
                waveUI.SetNextWaveTimer(0f);
            }
        }
    }


    IEnumerator SpawnWave(Wave wave)
    {
        enemiesAlive = wave.enemyCount;
        waveUI.SetEnemiesRemaining(enemiesAlive);
        float interval = wave.enemyCount > 0 ? wave.spawnDuration / wave.enemyCount : 0.5f;

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
        enemy.GetComponent<Enemy>().OnDeath += HandleEnemyDeath;
    }

    private void HandleEnemyDeath()
    {
        enemiesAlive--;
        waveUI.SetEnemiesRemaining(enemiesAlive);
    }
}
