using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AdaptiveWaveGenerator : MonoBehaviour
{
    public List<GameObject> enemyTypes;
    public float baseBudget = 10f;
    public float budgetGrowth = 2f;

    public int bossWaveInterval = 10;
    public int eliteWaveInterval = 5;
    [Range(0f, 1f)] public float waitForClearChance = 0.3f;

    public WaveData GenerateWave(PlayerPerformanceTracker tracker, int waveNumber, int maxSpawnZones)
    {
        float budget = baseBudget + waveNumber * budgetGrowth;
        budget *= Mathf.Clamp01(tracker.DifficultyScore / 100f + 1f);

        var wave = ScriptableObject.CreateInstance<WaveData>();
        wave.spawns = new List<WaveSpawnInfo>();

        int zoneIndex = Random.Range(0, maxSpawnZones);

        HashSet<EnemyRole> spawnedRoles = new();
        int iterationLimit = 100;
        int iterations = 0;

        while (budget > 0f && iterations < iterationLimit)
        {
            iterations++;

            var enemy = GetRandomAfordableEnemy(budget);
            if (enemy == null) break;

            var meta = enemy.GetComponent<EnemyMetadata>();
            if (meta == null) continue;

            float cost = GetCost(enemy);
            int maxGroup = Mathf.Max(1, meta.maxGroupSize);
            int count = Mathf.FloorToInt(budget / cost);
            if (count < 1) continue;

            count = Mathf.Min(count, maxGroup);

            if (meta.requiresMix)
            {
                bool synergyOK = meta.synergyWith.Any(needed => spawnedRoles.Contains(needed));
                if (!synergyOK) continue;
            }

            if (meta.role == EnemyRole.Swarm && count < meta.groupMinCount)
                continue;

            if (iterations > 5 && Random.value < 0.2f)
                continue;

            // Swarm override: let Swarms spawn in random zones
            List<int> swarmZones = null;
            if (meta.role == EnemyRole.Swarm)
            {
                int swarmZoneCount = Mathf.Min(Random.Range(1, 3), maxSpawnZones); // 1–2 zones
                swarmZones = new();
                while (swarmZones.Count < swarmZoneCount)
                {
                    int z = Random.Range(0, maxSpawnZones);
                    if (!swarmZones.Contains(z)) swarmZones.Add(z);
                }
            }

            int baseCount = meta.role == EnemyRole.Swarm && swarmZones != null ? count / swarmZones.Count : count;
            baseCount = Mathf.Max(1, baseCount); // ensure at least 1 per group

            var modifier = DetermineModifier(waveNumber);
            if (meta.role == EnemyRole.Swarm && swarmZones != null)
            {
                foreach (var swarmZone in swarmZones)
                {
                    wave.spawns.Add(new WaveSpawnInfo
                    {
                        enemyPrefab = enemy,
                        count = baseCount,
                        duration = Mathf.Max(1f, baseCount * 0.4f),
                        spawnZoneIndex = swarmZone,
                        modifier = modifier
                    });
                }
            }
            else
            {
                wave.spawns.Add(new WaveSpawnInfo
                {
                    enemyPrefab = enemy,
                    count = count,
                    duration = Mathf.Max(1f, count * 0.4f),
                    spawnZoneIndex = zoneIndex,
                    modifier = modifier
                });
            }

            spawnedRoles.Add(meta.role);
            budget -= cost * count;

            // Additional group spawning if budget remains
            while (budget >= cost)
            {
                int nextCount = Mathf.Min(Mathf.FloorToInt(budget / cost), maxGroup);
                wave.spawns.Add(new WaveSpawnInfo
                {
                    enemyPrefab = enemy,
                    count = nextCount,
                    duration = Mathf.Max(1f, nextCount * 0.4f),
                    spawnZoneIndex = zoneIndex,
                    modifier = DetermineModifier(waveNumber)
                });
                budget -= cost * nextCount;
            }
        }

        wave.waitForClearBeforeNext = Random.value < waitForClearChance;
        return wave;
    }

    private EnemyModifierType DetermineModifier(int waveNumber)
    {
        float eliteChance = waveNumber % eliteWaveInterval == 0 ? 0.5f : 0.05f;
        float armorChance = 0.05f;
        float fastChance = 0.05f;
        float explodingChance = 0.05f;
        float regeneratingChance = 0.05f;

        float roll = Random.value;

        if (waveNumber % bossWaveInterval == 0)
            return EnemyModifierType.Elite;

        if (roll < eliteChance) return EnemyModifierType.Elite;
        if (roll < eliteChance + armorChance) return EnemyModifierType.Armored;
        if (roll < eliteChance + armorChance + fastChance) return EnemyModifierType.Fast;
        if (roll < eliteChance + armorChance + fastChance + explodingChance) return EnemyModifierType.Exploding;
        if (roll < eliteChance + armorChance + fastChance + explodingChance + regeneratingChance) return EnemyModifierType.Regenerating;

        return EnemyModifierType.None;
    }

    private GameObject GetRandomAfordableEnemy(float budget)
    {
        var enemiesByCost = enemyTypes.Where(enemy => GetCost(enemy) <= budget).ToList();
        if (enemiesByCost.Count == 0) return null;

        return enemiesByCost[Random.Range(0, enemiesByCost.Count)];
    }

    private float GetCost(GameObject enemy)
    {
        var meta = enemy.GetComponent<EnemyMetadata>();
        return meta ? meta.cost : 1f;
    }
}