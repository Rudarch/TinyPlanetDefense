using UnityEngine;
using System.Collections;

public class DashMovement : EnemyMovementBase
{
    public float dashDistance = 1.5f;
    public float dashInterval = 1.0f;
    private Coroutine dashRoutine;

    public override void Initialize(Enemy owner)
    {
        base.Initialize(owner);
        dashRoutine = StartCoroutine(DashLoop());
    }

    public override void TickMovement()
    {
    }

    IEnumerator DashLoop()
    {
        while (true)
        {
            if (enemy.IsStunned())
            {
                yield return null;
                continue;
            }

            float interval = dashInterval / (enemy.moveSpeed / 2f);
            yield return new WaitForSeconds(interval);

            if (enemy.planetTarget == null) yield break;

            Vector3 dir = (enemy.planetTarget.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

            float effectiveDash = dashDistance * (enemy.moveSpeed / 2f);
            Vector3 targetPos = transform.position + dir * effectiveDash;

            if (enemy.thrusterFX != null) enemy.thrusterFX.SetActive(true);

            float dashTime = 0.2f;
            float t = 0f;
            Vector3 start = transform.position;

            while (t < dashTime)
            {
                if (enemy.IsStunned()) break;

                t += Time.deltaTime;
                transform.position = Vector3.Lerp(start, targetPos, t / dashTime);
                yield return null;
            }

            if (enemy.thrusterFX != null) enemy.thrusterFX.SetActive(false);
        }
    }
}
