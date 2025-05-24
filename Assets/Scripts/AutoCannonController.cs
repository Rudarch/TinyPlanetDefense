using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[ExecuteAlways]
public class AutoCannonController : MonoBehaviour
{
    public float maxRotationSpeed = 200f;
    public float rotationAcceleration = 500f;

    public float fireCooldown = 1f;
    public Transform shootPoint;
    public GameObject projectilePrefab;
    public float shootForce = 10f;
    public Transform planetCenter;
    public float aimThresholdAngle = 5f;

    public float firingRange = 8f;
    public Color rangeGizmoColor = Color.green;

    private float fireTimer;
    private float currentAngularVelocity = 0f;
    private Transform currentTarget;

    void Update()
    {
        if (planetCenter == null)
            return;

        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy ||
            Vector2.Distance(currentTarget.position, planetCenter.position) > firingRange)
        {
            GameObject newTarget = FindClosestEnemyInRange();
            currentTarget = newTarget ? newTarget.transform : null;
        }

        if (currentTarget != null)
        {
            Vector3 directionToTarget = (currentTarget.position - planetCenter.position).normalized;
            Vector3 cannonDirection = (transform.position - planetCenter.position).normalized;

            float angleToTarget = Vector3.SignedAngle(cannonDirection, directionToTarget, Vector3.forward);

            if (Mathf.Abs(angleToTarget) > aimThresholdAngle)
            {
                float desiredAngularVelocity = Mathf.Sign(angleToTarget) * maxRotationSpeed;
                currentAngularVelocity = Mathf.MoveTowards(currentAngularVelocity, desiredAngularVelocity, rotationAcceleration * Time.deltaTime);
            }
            else
            {
                currentAngularVelocity = 0f;
            }

            transform.RotateAround(planetCenter.position, Vector3.forward, currentAngularVelocity * Time.deltaTime);

            fireTimer -= Time.deltaTime;
            if (Mathf.Abs(angleToTarget) < aimThresholdAngle && fireTimer <= 0f)
            {
                Shoot();
                fireTimer = fireCooldown;
            }
        }
        else
        {
            currentAngularVelocity = 0f;
        }
    }

    GameObject FindClosestEnemyInRange()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
            return null;

        return enemies
            .Where(e => Vector2.Distance(e.transform.position, planetCenter.position) <= firingRange)
            .OrderBy(e => Vector2.Distance(planetCenter.position, e.transform.position))
            .FirstOrDefault();
    }

    void Shoot()
    {
        if (projectilePrefab != null && shootPoint != null)
        {
            GameObject proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = shootPoint.up * shootForce;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (planetCenter != null)
        {
            Gizmos.color = rangeGizmoColor;
            Gizmos.DrawWireSphere(planetCenter.position, firingRange);
        }
    }
}