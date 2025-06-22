using System.Collections;
using UnityEngine;

public class InterceptorDrone : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;
    public float interceptorRange = 5f;
    public int shotsPerMission = 3;
    public float reloadTime = 5f;
    public float fireCooldown = 0.3f;
    public GameObject projectilePrefab;
    public GameObject thrusterFX;

    private enum State { Idle, Seeking, Attacking, Returning }
    private State state = State.Idle;

    private Transform planet;
    private int shotsRemaining;
    private Enemy currentTarget;
    private float fireTimer = 0f;

    public void Initialize()
    {
        GameObject planetObj = GameObject.FindWithTag("Planet");
        if (planetObj != null)
            planet = planetObj.transform;
        else
            Debug.Log("Planet not found.");

        state = State.Seeking;
        SetThruster(true);
    }

    void Update()
    {
        switch (state)
        {
            case State.Seeking:
                SetThruster(true);
                SeekTarget();
                break;

            case State.Attacking:
                SetThruster(false);
                AttackTarget();
                break;

            case State.Returning:
                SetThruster(true);
                ReturnToPlanet();
                break;
        }

        RotateTowardsTarget();
    }


    void SeekTarget()
    {
        if (currentTarget == null || !IsTargetValid(currentTarget))
        {
            currentTarget = FindNearestEnemy();
        }

        if (currentTarget != null)
        {
            float dist = Vector3.Distance(transform.position, currentTarget.transform.position);
            if (dist <= interceptorRange)
            {
                shotsRemaining = shotsPerMission;
                state = State.Attacking;
                fireTimer = 0f;
                return;
            }

            Vector3 dir = (currentTarget.transform.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    void AttackTarget()
    {
        if (currentTarget == null || !IsTargetValid(currentTarget))
        {
            state = State.Returning;
            return;
        }

        Vector3 dir = (currentTarget.transform.position - transform.position).normalized;
        float dist = Vector3.Distance(transform.position, currentTarget.transform.position);

        if (dist > interceptorRange)
        {
            state = State.Seeking;
            return;
        }

        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f && shotsRemaining > 0)
        {
            fireTimer = fireCooldown;
            Shoot(dir);
            shotsRemaining--;
        }

        if (shotsRemaining <= 0)
        {
            state = State.Returning;
        }
    }

    void ReturnToPlanet()
    {
        if (planet == null) return;

        Vector3 dir = (planet.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, planet.position) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    void RotateTowardsTarget()
    {
        Vector3 targetDir = Vector3.zero;

        if (state == State.Seeking && currentTarget != null)
            targetDir = currentTarget.transform.position - transform.position;
        else if (state == State.Attacking && currentTarget != null)
            targetDir = currentTarget.transform.position - transform.position;
        else if (state == State.Returning && planet != null)
            targetDir = planet.position - transform.position;

        if (targetDir != Vector3.zero)
        {
            float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRot = Quaternion.Euler(0f, 0f, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }

    void Shoot(Vector3 direction)
    {
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        var projectile = proj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.SetDirection(direction.normalized);
        }
    }

    Enemy FindNearestEnemy()
    {
        Enemy[] all = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Enemy closest = null;
        float closestDist = float.MaxValue;

        foreach (var enemy in all)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = enemy;
            }
        }

        return closest;
    }
    void SetThruster(bool active)
    {
        if (thrusterFX != null)
            thrusterFX.SetActive(active);
    }

    bool IsTargetValid(Enemy enemy)
    {
        return enemy != null && enemy.gameObject.activeInHierarchy;
    }
}
