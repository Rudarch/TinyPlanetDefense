using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<WaveData> levelWaves;
    public Transform[] spawnZones;
    public AdaptiveWaveGenerator waveGenerator;
    public PlayerPerformanceTracker performanceTracker;
    public bool useAdaptiveWaves = false;

    private int currentWave = 0;
    private bool isSpawning = false;

    public void StartLevel()
    {
        currentWave = 0;
        StartNextWave();
    }

    public void StartNextWave()
    {
        if (isSpawning) return;

        WaveEvents.OnWaveStarted?.Invoke(currentWave);

        if (useAdaptiveWaves)
        {
            WaveData wave = waveGenerator.GenerateWave(performanceTracker, currentWave, spawnZones.Length);
            levelWaves.Add(wave);
            currentWave++;
            StartCoroutine(RunWave(wave));
        }
        else
        {
            if (currentWave >= levelWaves.Count) return;
            StartCoroutine(RunWave(levelWaves[currentWave]));
            currentWave++;
        }
    }

    IEnumerator RunWave(WaveData wave)
    {
        isSpawning = true;
        yield return new WaitForSeconds(wave.delayBeforeStart);

        foreach (var spawn in wave.spawns)
        {
            float interval = spawn.duration / Mathf.Max(1, spawn.count);
            for (int i = 0; i < spawn.count; i++)
            {
                SpawnEnemy(spawn.enemyPrefab, spawn.spawnZoneIndex, spawn.modifier);
                yield return new WaitForSeconds(interval);
            }

            yield return new WaitForSeconds(spawn.delayAfter);
        }

        isSpawning = false;

        if (wave.waitForClearBeforeNext)
        {
            yield return new WaitUntil(() => EnemyManager.Inst != null && EnemyManager.Inst.GetAllEnemies().Count == 0);
        }

        yield return new WaitForSeconds(1f); // pacing buffer

        StartNextWave();
    }

    void SpawnEnemy(GameObject prefab, int zoneIndex, EnemyModifierType modifier = EnemyModifierType.None)
    {
        if (prefab == null || spawnZones.Length == 0) return;

        int index = (zoneIndex >= 0 && zoneIndex < spawnZones.Length)
            ? zoneIndex
            : Random.Range(0, spawnZones.Length);

        var box = spawnZones[index].GetComponent<BoxCollider2D>();
        Vector2 pos = RandomPointInBounds(box.bounds);
        GameObject go = Instantiate(prefab, pos, Quaternion.identity);

        var enemy = go.GetComponent<Enemy>();
        if (enemy != null)
        {
            ApplyModifier(enemy, modifier);
        }
    }

    Vector2 RandomPointInBounds(Bounds bounds)
    {
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );
    }

    void ApplyModifier(Enemy enemy, EnemyModifierType modifier)
    {
        switch (modifier)
        {
            case EnemyModifierType.Fast:
                enemy.moveSpeed *= 1.5f;
                break;
            case EnemyModifierType.Armored:
                enemy.health *= 2f;
                break;
            case EnemyModifierType.Elite:
                enemy.moveSpeed *= 1.2f;
                enemy.health *= 2f;
                break;
            case EnemyModifierType.Regenerating:
                enemy.gameObject.AddComponent<RegenerationAbility>();
                break;
            case EnemyModifierType.Exploding:
                enemy.gameObject.AddComponent<ExplodeOnDeath>();
                break;
        }
    }
}