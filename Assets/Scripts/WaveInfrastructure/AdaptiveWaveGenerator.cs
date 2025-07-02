using System.Collections.Generic;
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

        int zonesToUse = Random.Range(1, maxSpawnZones + 1);
        List<int> chosenZones = new();
        for (int i = 0; i < zonesToUse; i++)
        {
            int zone;
            do { zone = Random.Range(0, maxSpawnZones); }
            while (chosenZones.Contains(zone));
            chosenZones.Add(zone);
        }

        HashSet<EnemyRole> spawnedRoles = new();
        Dictionary<int, EnemyRole> lastRoleInZone = new();

        int iterationLimit = 100;
        int iterations = 0;

        while (budget > 0f && iterations < iterationLimit)
        {
            iterations++;

            var enemy = GetRandomEnemyByCost();
            var meta = enemy.GetComponent<EnemyMetadata>();
            if (meta == null) continue;

            float cost = GetCost(enemy);
            int maxGroup = Mathf.Max(1, meta.maxGroupSize);
            int count = Mathf.FloorToInt(budget / cost);
            if (count < 1) continue;

            count = Mathf.Min(count, maxGroup);

            // Strict support logic: skip if no synergy role already spawned
            if (meta.requiresMix)
            {
                bool synergyOK = false;
                foreach (var needed in meta.synergyWith)
                {
                    if (spawnedRoles.Contains(needed)) { synergyOK = true; break; }
                }
                if (!synergyOK) continue;
            }

            if (meta.role == EnemyRole.Swarm && count < meta.groupMinCount)
                continue;

            if (iterations > 5 && Random.value < 0.2f)
                continue;

            // Prefer placing different unit types in different zones
            int zoneIndex = chosenZones[Random.Range(0, chosenZones.Count)];
            if (lastRoleInZone.TryGetValue(zoneIndex, out var prevRole))
            {
                if (prevRole == meta.role && Random.value < 0.75f) // 75% chance to skip repeating role in same zone
                    continue;
            }

            var info = new WaveSpawnInfo
            {
                enemyPrefab = enemy,
                count = count,
                duration = Mathf.Max(1f, count * 0.4f),
                spawnZoneIndex = zoneIndex,
                modifier = DetermineModifier(waveNumber)
            };

            wave.spawns.Add(info);
            spawnedRoles.Add(meta.role);
            lastRoleInZone[zoneIndex] = meta.role;
            budget -= cost * count;

            // Additional groups if budget remains
            while (budget >= cost)
            {
                int nextCount = Mathf.Min(Mathf.FloorToInt(budget / cost), maxGroup);
                var extraInfo = new WaveSpawnInfo
                {
                    enemyPrefab = enemy,
                    count = nextCount,
                    duration = Mathf.Max(1f, nextCount * 0.4f),
                    spawnZoneIndex = chosenZones[Random.Range(0, chosenZones.Count)],
                    modifier = DetermineModifier(waveNumber)
                };
                wave.spawns.Add(extraInfo);
                budget -= cost * nextCount;
            }
        }

        wave.waitForClearBeforeNext = Random.value < waitForClearChance;
        return wave;
    }

    private EnemyModifierType DetermineModifier(int waveNumber)
    {
        float eliteChance = waveNumber % eliteWaveInterval == 0 ? 0.5f : 0.1f;
        float fastChance = 0.1f;
        float armorChance = 0.15f;

        float roll = Random.value;

        if (waveNumber % bossWaveInterval == 0)
            return EnemyModifierType.Elite;

        if (roll < eliteChance) return EnemyModifierType.Elite;
        if (roll < eliteChance + armorChance) return EnemyModifierType.Armored;
        if (roll < eliteChance + armorChance + fastChance) return EnemyModifierType.Fast;

        return EnemyModifierType.None;
    }

    private GameObject GetRandomEnemyByCost()
    {
        float maxCost = 0f;
        foreach (var e in enemyTypes)
        {
            float cost = GetCost(e);
            if (cost > maxCost) maxCost = cost;
        }

        List<GameObject> weightedList = new();
        foreach (var e in enemyTypes)
        {
            float cost = GetCost(e);
            int weight = Mathf.RoundToInt(maxCost - cost + 1);
            for (int i = 0; i < weight; i++)
                weightedList.Add(e);
        }

        return weightedList[Random.Range(0, weightedList.Count)];
    }

    private float GetCost(GameObject enemy)
    {
        var meta = enemy.GetComponent<EnemyMetadata>();
        return meta ? meta.cost : 1f;
    }
}