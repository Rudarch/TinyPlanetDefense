using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Inst { get; private set; }

    private readonly List<Enemy> activeEnemies = new();

    private PlayerPerformanceTracker performanceTracker;

    void Awake()
    {
        if (Inst != null && Inst != this) { Destroy(gameObject); return; }
        Inst = this;
        performanceTracker = FindAnyObjectByType<PlayerPerformanceTracker>();
    }

    public void RegisterEnemy(Enemy enemy)
    {
        if (!activeEnemies.Contains(enemy))
        {
            activeEnemies.Add(enemy);
        }
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);

            if (performanceTracker != null)
            {
                performanceTracker.NotifyEnemyKilled(enemy.gameObject);
            }
        }
    }

    public List<Enemy> GetEnemiesInRange(Vector3 position, float radius)
    {
        var enemiesInRange = new List<Enemy>();
        float radiusSqr = radius * radius;

        foreach (var enemy in activeEnemies)
        {
            if (enemy == null) continue;

            if ((enemy.transform.position - position).sqrMagnitude <= radiusSqr)
            {
                enemiesInRange.Add(enemy);
            }
        }

        return enemiesInRange;
    }

    public List<Enemy> GetAllEnemies()
    {
        return new List<Enemy>(activeEnemies);
    }
}
