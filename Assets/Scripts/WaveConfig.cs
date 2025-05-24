using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class WaveConfig
{
    public int totalValue = 10;
    public float delayBeforeNextWave = 5f;
    public List<EnemyTypeValue> enemyValues = new List<EnemyTypeValue>();

    public Dictionary<EnemyType, int> ToDictionary()
    {
        Dictionary<EnemyType, int> dict = new Dictionary<EnemyType, int>();
        foreach (var pair in enemyValues)
        {
            dict[pair.type] = pair.value;
        }
        return dict;
    }
}

[Serializable]
public class EnemyTypeValue
{
    public EnemyType type;
    public int value;
}