using UnityEngine;

public class ShieldAlliesNearby : EnemyAbilityBase
{
    public float radius = 2f;
    public float shieldDuration = 2f;

    public override void OnUpdate()
    {
        foreach (var hit in Physics2D.OverlapCircleAll(enemy.transform.position, radius))
        {
            if (hit.TryGetComponent(out Enemy ally) && ally != enemy)
            {
                ally.SetStunned(false); // or apply a real shield if supported
            }
        }
    }
}
