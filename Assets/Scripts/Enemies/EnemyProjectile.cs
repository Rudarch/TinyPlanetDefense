using UnityEngine;
using UnityEngine.UIElements;

public class EnemyProjectile : BaseProjectile
{
    public float damage = 10f;
    public GameObject impactVFX;

    protected override void HandleHit(Collider2D other)
    {
        if (other.TryGetComponent(out Planet planet))
        {
            if (impactVFX != null)
            {
                var vfx = Instantiate(impactVFX, gameObject.transform.position, gameObject.transform.rotation);
                vfx.transform.SetParent(null);
                vfx.transform.localScale = gameObject.transform.localScale;

            }

            planet.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
