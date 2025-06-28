using UnityEngine;

public class LaunchProjectile : EnemyAbilityBase
{
    public GameObject projectilePrefab;
    public float cooldown = 3f;
    private float timer = 0f;

    public override void OnUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            if (projectilePrefab != null && enemy.planetTarget)
            {
                GameObject proj = Instantiate(projectilePrefab, enemy.transform.position, Quaternion.identity);
                Vector2 dir = (enemy.planetTarget.position - enemy.transform.position).normalized;
                proj.GetComponent<Rigidbody2D>().linearVelocity = dir * 5f;
            }
            timer = cooldown;
        }
    }
}
