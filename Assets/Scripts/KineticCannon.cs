using UnityEngine;

public class KineticCannon : WeaponSystem
{
    public GameObject projectilePrefab;
    public float damage = 10f;
    public float cooldown = 2f;

    private float lastFireTime = -Mathf.Infinity;

    public override void TryFireAt(Transform target)
    {
        if (Time.time - lastFireTime < cooldown) return;
        lastFireTime = Time.time;

        // Shoot
        Vector3 dir = (target.position - transform.position).normalized;
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().linearVelocity = dir * 10f;

        // You could assign damage or effects here
    }
}
