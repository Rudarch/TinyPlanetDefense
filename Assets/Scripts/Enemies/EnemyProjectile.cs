using UnityEngine;

public class EnemyProjectile : BaseProjectile
{
    public float damage = 10f;

    protected override void HandleHit(Collider2D other)
    {
        if (other.TryGetComponent(out Planet planet))
        {
            planet.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
