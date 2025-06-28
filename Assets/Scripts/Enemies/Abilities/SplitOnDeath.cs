using UnityEngine;

public class SplitOnDeath : EnemyAbilityBase
{
    public GameObject splitPrefab;
    public int count = 2;
    public float spreadRadius = 1f;

    public override void OnDeath()
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 offset = Random.insideUnitCircle * spreadRadius;
            GameObject clone = Instantiate(splitPrefab, enemy.transform.position + (Vector3)offset, Quaternion.identity);
        }
    }
}
