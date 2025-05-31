using UnityEngine;

public abstract class WeaponSystem : MonoBehaviour
{
    public abstract void TryFireAt(Transform target);

    public abstract void TryFireWithDirection(Vector2 direction);

    public void TryFireAtWorldPosition(Vector3 worldPos)
    {
        Vector2 dir = (worldPos - transform.position).normalized;
        TryFireWithDirection(dir);
    }
}
