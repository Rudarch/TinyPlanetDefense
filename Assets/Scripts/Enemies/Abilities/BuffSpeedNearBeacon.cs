using UnityEngine;
public class BuffSpeedNearBeacon : EnemyAbilityBase
{
    public float radius = 3f;
    public float speedMultiplier = 1.3f;

    public override void OnUpdate()
    {
        foreach (var ally in EnemyManager.Inst.GetEnemiesInRange(enemy.transform.position, radius))
        {
            if (ally != enemy)
            {
                ally.moveSpeed = Mathf.Max(ally.moveSpeed, enemy.moveSpeed * speedMultiplier);
            }
        }
    }
}
