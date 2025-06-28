using UnityEngine;

public class SpawnMineOnDeath : EnemyAbilityBase
{
    public GameObject minePrefab;

    public override void OnDeath()
    {
        if (minePrefab != null)
        {
            Instantiate(minePrefab, enemy.transform.position, Quaternion.identity);
        }
    }
}
