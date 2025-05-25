using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 1;
    public bool isEnemyProjectile = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEnemyProjectile && collision.CompareTag("Player"))
        {
            var health = collision.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
        else if (!isEnemyProjectile && collision.CompareTag("Enemy"))
        {
            var health = collision.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
