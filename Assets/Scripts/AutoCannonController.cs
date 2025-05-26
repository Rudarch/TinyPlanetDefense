using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class AutoCannonController : MonoBehaviour
{
    public float baseMaxRotationSpeed = 200f;
    public float baseRotationAcceleration = 500f;
    public float baseFireCooldown = 1f;
    public float baseFiringRange = 8f;

    public Transform shootPoint;
    public GameObject projectilePrefab;
    public float shootForce = 10f;
    public Transform planetCenter;
    public float aimThresholdAngle = 5f;

    public Color rangeGizmoColor = Color.green;

    private float fireTimer;
    private float currentAngularVelocity = 0f;
    private Transform currentTarget;
    private float currentAngle; // degrees

    private float firingRange;
    private float fireCooldown;
    private float maxRotationSpeed;
    private float rotationAcceleration;

    private float orbitRadius;

    void Start()
    {
        if (planetCenter != null)
        {
            orbitRadius = Vector2.Distance(transform.position, planetCenter.position);
            currentAngle = Mathf.Atan2(transform.position.y - planetCenter.position.y, transform.position.x - planetCenter.position.x) * Mathf.Rad2Deg;
        }
    }

    void Update()
    {
        if (planetCenter == null) return;

        ApplyUpgradeModifiers();

        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy ||
            Vector2.Distance(currentTarget.position, planetCenter.position) > firingRange)
        {
            GameObject newTarget = FindClosestEnemyInRange();
            currentTarget = newTarget != null ? newTarget.transform : null;
        }

        if (currentTarget != null)
        {
            Vector2 dirToTarget = currentTarget.position - planetCenter.position;
            float targetAngle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg;
            float angleDiff = Mathf.DeltaAngle(currentAngle, targetAngle);

            float accel = Mathf.Sign(angleDiff) * rotationAcceleration * Time.deltaTime;
            currentAngularVelocity += accel;
            currentAngularVelocity = Mathf.Clamp(currentAngularVelocity, -maxRotationSpeed, maxRotationSpeed);

            if (Mathf.Abs(angleDiff) < aimThresholdAngle)
            {
                currentAngularVelocity = 0f;

                fireTimer -= Time.deltaTime;
                if (fireTimer <= 0f)
                {
                    Shoot();
                    fireTimer = fireCooldown;
                }
            }

            currentAngle += currentAngularVelocity * Time.deltaTime;

            // Clamp angle to [0, 360)
            if (currentAngle < 0f) currentAngle += 360f;
            if (currentAngle >= 360f) currentAngle -= 360f;

            Vector3 offset = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0f) * orbitRadius;
            transform.position = planetCenter.position + offset;

            transform.rotation = Quaternion.Euler(0f, 0f, currentAngle - 90f); // face outward
        }
    }

    void Shoot()
    {
        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = shootPoint.up * shootForce;
        }

        Projectile p = proj.GetComponent<Projectile>();
        if (p != null)
        {
            p.damage = Mathf.RoundToInt(p.damage * UpgradeManager.Instance.damageMultiplier);
        }

        Animator anim = GetComponent<Animator>();
        anim.Play("Cannon_Recoil", -1, 0f); // Play from start
    }

    void ApplyUpgradeModifiers()
    {
        if (UpgradeManager.Instance == null) return;

        fireCooldown = baseFireCooldown * UpgradeManager.Instance.fireCooldownMultiplier;
        firingRange = baseFiringRange * UpgradeManager.Instance.firingRangeMultiplier;
        maxRotationSpeed = baseMaxRotationSpeed * UpgradeManager.Instance.rotationSpeedMultiplier;
        rotationAcceleration = baseRotationAcceleration * UpgradeManager.Instance.rotationSpeedMultiplier;
    }

    GameObject FindClosestEnemyInRange()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDist = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector2.Distance(enemy.transform.position, planetCenter.position);
            if (dist <= firingRange && dist < minDist)
            {
                closest = enemy;
                minDist = dist;
            }
        }

        return closest;
    }

    void OnDrawGizmosSelected()
    {
        if (planetCenter != null)
        {
            Gizmos.color = rangeGizmoColor;
            Gizmos.DrawWireSphere(planetCenter.position, firingRange);
        }
    }

    public float GetFiringRange()
    {
        return firingRange;
    }
}