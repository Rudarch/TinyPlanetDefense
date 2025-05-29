using UnityEngine;

public class CannonController : MonoBehaviour
{
    public Transform turretHead;
    public float rotationSpeed = 180f; // degrees per second
    public float maxAimAngle = 10f; // Allowed angle deviation to fire

    public Transform planetCenter;
    public float detectionRadius = 20f;
    public LayerMask obstructionMask;

    public WeaponSystem currentWeapon;

    private Enemy currentTarget;

    void Update()
    {
        // Check if current target is still valid
        if (currentTarget == null || !IsTargetValid(currentTarget))
        {
            currentTarget = FindNearestVisibleEnemy();
        }

        if (currentTarget != null)
        {
            Vector3 dir = (currentTarget.transform.position - turretHead.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - 90f);

            turretHead.rotation = Quaternion.RotateTowards(
                turretHead.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            if (IsAimingAt(currentTarget.transform))
            {
                currentWeapon.TryFireAt(currentTarget.transform);
            }
        }
    }


    private bool IsAimingAt(Transform target)
    {
        Vector3 toTarget = (target.position - turretHead.position).normalized;
        Vector3 turretForward = turretHead.up;

        float angle = Vector3.Angle(turretForward, toTarget);
        return angle <= maxAimAngle;
    }

    private Enemy FindNearestVisibleEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        float closestDist = Mathf.Infinity;
        Enemy closestEnemy = null;

        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy == null) continue;

            Vector3 dir = enemy.transform.position - turretHead.position;
            RaycastHit2D hitInfo = Physics2D.Raycast(turretHead.position, dir.normalized, dir.magnitude, obstructionMask);

            if (hitInfo.collider == null && dir.magnitude < closestDist)
            {
                closestDist = dir.magnitude;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
    private bool IsTargetValid(Enemy target)
    {
        if (target == null)
            return false;

        // Check if within detection range
        float dist = Vector3.Distance(turretHead.position, target.transform.position);
        if (dist > detectionRadius)
            return false;

        // Check if obstructed by planet or other object
        Vector3 dir = (target.transform.position - turretHead.position);
        RaycastHit2D hit = Physics2D.Raycast(turretHead.position, dir.normalized, dir.magnitude, obstructionMask);
        return hit.collider == null; // null means no obstruction
    }

    void OnDrawGizmosSelected()
    {
        if (turretHead == null)
            return;

        // Draw the detection/attack radius
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(turretHead.position, detectionRadius);

        // Draw aim cone
        Vector3 forward = turretHead.up;
        float halfAngle = maxAimAngle;

        Gizmos.color = Color.red;
        Quaternion leftRot = Quaternion.AngleAxis(-halfAngle, Vector3.forward);
        Quaternion rightRot = Quaternion.AngleAxis(halfAngle, Vector3.forward);

        Vector3 leftDir = leftRot * forward;
        Vector3 rightDir = rightRot * forward;

        float arcLength = detectionRadius;
        Gizmos.DrawLine(turretHead.position, turretHead.position + leftDir * arcLength);
        Gizmos.DrawLine(turretHead.position, turretHead.position + rightDir * arcLength);
    }
}
