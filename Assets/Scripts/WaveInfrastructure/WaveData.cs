using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WaveData/WaveData")]
public class WaveData : ScriptableObject
{
    public List<WaveSpawnInfo> spawns;
    public float delayBeforeStart = 1f;
    public bool waitForClearBeforeNext = true;
}