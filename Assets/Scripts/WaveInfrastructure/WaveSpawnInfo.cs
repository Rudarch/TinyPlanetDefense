using UnityEngine;

[System.Serializable]
public class WaveSpawnInfo
{
    public GameObject enemyPrefab;
    public int count = 5;
    public float duration = 5f;
    public int spawnZoneIndex = -1; // -1 = random
    public float delayAfter = 0f;
    public EnemyModifierType modifier = EnemyModifierType.None;
}