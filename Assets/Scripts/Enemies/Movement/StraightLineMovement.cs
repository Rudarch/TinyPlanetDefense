using UnityEngine;

public class StraightLineMovement : EnemyMovementBase
{
    public override void TickMovement()
    {
        if (enemy.IsStunned() || enemy.planetTarget == null)
            return;

        Vector3 direction = (enemy.planetTarget.position - transform.position).normalized;
        transform.position += direction * enemy.moveSpeed * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}
