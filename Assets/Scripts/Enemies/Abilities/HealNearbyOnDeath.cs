using UnityEngine;

public class HealNearbyOnDeath : EnemyAbilityBase
{
    public float healAmount = 5f;
    public float radius = 2f;

    public override void OnDeath()
    {
        foreach (var hit in Physics2D.OverlapCircleAll(enemy.transform.position, radius))
        {
            if (hit.TryGetComponent(out Enemy ally) && ally != enemy)
            {
                ally.TakeDamage(-healAmount); // heals
            }
        }
    }
}
