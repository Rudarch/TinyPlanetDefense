using UnityEngine;
using System.Collections;

public class DashMovement : EnemyMovementBase
{
    public float dashDistance = 1.5f;
    public float dashInterval = 1.0f;

    [Header("Deviation (degrees)")]
    public float minPositiveDeviation = 0f;
    public float maxPositiveDeviation = 0f;
    public float minNegativeDeviation = 0f;
    public float maxNegativeDeviation = 0f;

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
            if (enemy.IsStunned() || !enemy.shouldMove)
            {
                yield return null;
                continue;
            }

            float interval = dashInterval / (enemy.moveSpeed / 2f);
            yield return new WaitForSeconds(interval);

            if (enemy.planetTarget == null) yield break;
            // Base direction toward planet
            Vector3 baseDir = (enemy.planetTarget.position - transform.position).normalized;

            // Choose random deviation
            float deviation = Random.value < 0.5f
                ? Random.Range(minNegativeDeviation, maxNegativeDeviation)
                : Random.Range(minPositiveDeviation, maxPositiveDeviation);

            // Apply deviation to direction
            Vector3 dir = Quaternion.Euler(0, 0, deviation) * baseDir;
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
