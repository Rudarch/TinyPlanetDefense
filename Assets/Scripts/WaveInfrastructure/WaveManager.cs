using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject spawnEffectPrefab;
    public List<WaveData> levelWaves;
    public Transform[] spawnZones;
    public AdaptiveWaveGenerator waveGenerator;
    public PlayerPerformanceTracker performanceTracker;
    public bool useAdaptiveWaves = false;

    [SerializeField] private float hpScalingPerWave = 0.1f; // 10%

    private int currentWave = 0;
    private bool isSpawning = false;

    public void StartLevel()
    {
        StartNextWave();
    }

    public void StartNextWave()
    {
        if (isSpawning) return;

        currentWave++;
        performanceTracker?.SetCurrentWave(currentWave);

        if (useAdaptiveWaves)
        {
            WaveData wave = waveGenerator.GenerateWave(performanceTracker, currentWave, spawnZones.Length);
            levelWaves.Add(wave);
            WaveEvents.OnWaveStarted?.Invoke(currentWave);
            StartCoroutine(RunWave(wave));
        }
        else
        {
            if (currentWave >= levelWaves.Count) return;
            StartCoroutine(RunWave(levelWaves[currentWave]));
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

        yield return new WaitForSeconds(3f);

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

        // Spawn visual effect first
        if (spawnEffectPrefab != null)
        {
            GameObject fx = Instantiate(spawnEffectPrefab, pos, Quaternion.identity);
            if (prefab != null)
            {
                Vector3 enemyScale = prefab.transform.localScale;
                fx.transform.localScale = enemyScale;
            }

            Destroy(fx, 1.5f); // or however long the animation lasts
        }

        // Then spawn the actual enemy
        GameObject go = Instantiate(prefab, pos, Quaternion.identity);

        var enemy = go.GetComponent<Enemy>();
        if (enemy != null)
        {
            float hpMultiplier = Mathf.Pow(1f + hpScalingPerWave, currentWave);
            enemy.ApplyHealthScaling(hpMultiplier);

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

        enemy.ApplyModifier(modifier);
    }
}