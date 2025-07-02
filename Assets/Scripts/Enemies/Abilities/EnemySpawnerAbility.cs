using UnityEngine;

public class EnemySpawnerAbility : EnemyAbilityBase
{
    public GameObject enemyToSpawn;
    public float spawnInterval = 5f;
    public int maxSpanws = 5;
    public Transform spawnPointOverride;

    private float timer = 0f;
    private int spanwned = 0;

    public override void OnUpdate()
    {
        if (enemy == null || enemyToSpawn == null || enemy.IsStunned()) return;

        timer -= Time.deltaTime;
        if (timer <= 0f && spanwned < maxSpanws)
        {
            timer = spawnInterval;

            Vector3 spawnPosition = spawnPointOverride != null
                ? spawnPointOverride.position
                : transform.position;

            GameObject spawned = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            var newEnemy = spawned.GetComponent<Enemy>();
            if (newEnemy != null)
            {
                newEnemy.OnDeath += () =>
                {
                    EnemyManager.Inst?.UnregisterEnemy(newEnemy);
                };

                EnemyManager.Inst?.RegisterEnemy(newEnemy);
            }

            spanwned++;
        }
    }
}