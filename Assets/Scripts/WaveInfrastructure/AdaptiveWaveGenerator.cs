using System.Collections.Generic;
using UnityEngine;

public class AdaptiveWaveGenerator : MonoBehaviour
{
    public List<GameObject> enemyTypes;
    public float baseBudget = 10f;
    public float budgetGrowth = 2f;
    //public int maxGroupSize = 5;

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
            do
            {
                zone = Random.Range(0, maxSpawnZones);
            } while (chosenZones.Contains(zone));
            chosenZones.Add(zone);
        }

        HashSet<EnemyRole> spawnedRoles = new();

        int iterationLimit = 100;
        int iterations = 0;

        while (budget > 0f && iterations < iterationLimit)
        {
            iterations++;

            var enemy = GetRandomEnemyByCost();
            var meta = enemy.GetComponent<EnemyMetadata>();
            if (meta == null) continue;

            float cost = meta.cost;
            int count = Mathf.FloorToInt(budget / cost);
            if (count < 1) continue;

            count = Mathf.Clamp(count, 1, waveNumber);

            // Role synergy logic
            if (meta.requiresMix)
            {
                bool synergyOK = false;
                foreach (var needed in meta.synergyWith)
                {
                    if (spawnedRoles.Contains(needed))
                    {
                        synergyOK = true;
                        break;
                    }
                }

                if (!synergyOK) continue;
            }

            if (meta.role == EnemyRole.Swarm && count < meta.groupMinCount)
                continue;

            var info = new WaveSpawnInfo
            {
                enemyPrefab = enemy,
                count = count,
                duration = Mathf.Max(1f, count * 0.5f),
                spawnZoneIndex = chosenZones[Random.Range(0, chosenZones.Count)],
                modifier = EnemyModifierType.None
            };

            wave.spawns.Add(info);
            spawnedRoles.Add(meta.role);
            budget -= cost * count;
        }

        return wave;
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
            {
                weightedList.Add(e);
            }
        }

        return weightedList[Random.Range(0, weightedList.Count)];
    }

    private float GetCost(GameObject enemy)
    {
        var meta = enemy.GetComponent<EnemyMetadata>();
        return meta ? meta.cost : 1f;
    }
}