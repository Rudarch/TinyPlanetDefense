using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class WaveConfig
{
    public int totalValue = 10;
    public float delayBeforeNextWave = 5f;
    public List<EnemyType> enemyTypes = new List<EnemyType>();
}