using UnityEngine;

public class OrbitingMovement : EnemyMovementBase
{
    public float orbitRadius = 3f;
    private bool inOrbit = false;
    private float angle;

    public override void TickMovement()
    {
        if (enemy.IsStunned() || enemy.planetTarget == null) return;

        Vector3 toPlanet = enemy.planetTarget.position - transform.position;
        float distance = toPlanet.magnitude;

        if (!inOrbit)
        {
            Vector3 direction = toPlanet.normalized;
            float moveStep = enemy.moveSpeed * Time.deltaTime;

            if (Mathf.Abs(distance - orbitRadius) > 0.1f)
            {
                transform.position += direction * Mathf.Sign(distance - orbitRadius) * moveStep;

                float angleToPlanet = Mathf.Atan2(toPlanet.y, toPlanet.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angleToPlanet - 90f);
            }
            else
            {
                inOrbit = true;
                angle = Mathf.Atan2(toPlanet.y, toPlanet.x) * Mathf.Rad2Deg + 180f;
            }
        }
        else
        {
            float angularSpeedDegPerSec = (enemy.moveSpeed / orbitRadius) * Mathf.Rad2Deg;
            angle += angularSpeedDegPerSec * Time.deltaTime;

            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * orbitRadius;
            transform.position = enemy.planetTarget.position + offset;

            Vector3 basePosition = enemy.planetTarget.position + offset;
            Vector3 perpendicular = new Vector3(-Mathf.Sin(rad), Mathf.Cos(rad), 0f);
            transform.position = ApplyWiggleOffset(basePosition, perpendicular);

            // Face forward (top-facing sprite = +Y)
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

    }
}