using UnityEngine;
using UnityEngine.InputSystem;

public class CannonController : MonoBehaviour
{
    public Transform rotatingPart;
    public WeaponSystem weaponSystem;
    public float allowedDeviationAngle = 5f;
    public float baseRotationSpeed = 180f;
    public Transform planetTransform;

    private Transform currentTarget;
    private bool isPlayerAiming = false;
    private Vector3 manualAimPosition;

    void Update()
    {
        HandleManualInput();

        if (isPlayerAiming)
        {
            RotateToward(manualAimPosition);

            Vector2 cannonPos = rotatingPart.position;
            Vector2 aimDir = (manualAimPosition - (Vector3)cannonPos).normalized;

            if (IsInLineOfSight(manualAimPosition) && IsLookingAtDirection(aimDir))
            {
                weaponSystem.TryFireWithDirection(aimDir);
            }
        }
        else
        {
            if (currentTarget == null || !IsTargetStillValid(currentTarget))
            {
                currentTarget = FindNearestVisibleEnemy();
            }

            if (currentTarget != null)
            {
                RotateToward(currentTarget.position);

                if (IsLookingAtTarget(currentTarget.position) && IsInLineOfSight(currentTarget.position))
                {
                    weaponSystem.TryFireAt(currentTarget);
                }
            }
        }
    }

    void HandleManualInput()
    {
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;
            if (touch.press.isPressed)
            {
                Vector2 screenPos = touch.position.ReadValue();
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
                worldPos.z = 0f;
                manualAimPosition = worldPos;
                isPlayerAiming = true;
            }
            else
            {
                isPlayerAiming = false;
            }
        }
        else if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                Vector2 screenPos = Mouse.current.position.ReadValue();
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
                worldPos.z = 0f;
                manualAimPosition = worldPos;
                isPlayerAiming = true;
            }
            else
            {
                isPlayerAiming = false;
            }
        }
    }

    void RotateToward(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - rotatingPart.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        float angle = Mathf.MoveTowardsAngle(rotatingPart.eulerAngles.z, targetAngle, (baseRotationSpeed + Upgrades.Inst.Cannon.rotationSpeed) * Time.deltaTime);
        rotatingPart.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    bool IsLookingAtDirection(Vector2 dir)
    {
        float angleToTarget = Vector2.SignedAngle(rotatingPart.up, dir);
        return Mathf.Abs(angleToTarget) <= allowedDeviationAngle;
    }

    bool IsLookingAtTarget(Vector3 targetPos)
    {
        Vector2 toTarget = (targetPos - rotatingPart.position).normalized;
        return IsLookingAtDirection(toTarget);
    }

    bool IsInLineOfSight(Vector3 targetPos)
    {
        Vector2 origin = rotatingPart.position;
        Vector2 dir = ((Vector2)targetPos - origin).normalized;
        float dist = Vector2.Distance(origin, targetPos);

        RaycastHit2D hit = Physics2D.Raycast(origin, dir, dist);
        if (hit.collider != null && hit.collider.transform == planetTransform)
            return false;

        return true;
    }

    bool IsTargetStillValid(Transform target)
    {
        if (target == null)
            return false;

        return IsInLineOfSight(target.position);
    }

    Transform FindNearestVisibleEnemy()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Transform closest = null;
        float closestDist = float.MaxValue;

        foreach (var enemy in enemies)
        {
            float dist = Vector2.Distance(rotatingPart.position, enemy.transform.position);
            if (dist < closestDist && IsInLineOfSight(enemy.transform.position))
            {
                closest = enemy.transform;
                closestDist = dist;
            }
        }

        return closest;
    }
}
