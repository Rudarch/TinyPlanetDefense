using UnityEngine;

public class LaunchProjectile : EnemyAbilityBase
{
    public GameObject projectilePrefab;
    public float cooldown = 3f;
    public float attackRange = 2f;
    private float timer = 0f;

    public override void OnUpdate()
    {
        var distanceToTarget = Vector3.Distance(transform.position, enemy.planetTarget.position);
        if (distanceToTarget <= attackRange) enemy.shouldMove = false;
        else enemy.shouldMove = true;

        timer -= Time.deltaTime;
        if (timer <= 0f && distanceToTarget <= attackRange)
        {
            if (projectilePrefab != null && enemy.planetTarget)
            {
                GameObject proj = Instantiate(projectilePrefab, enemy.transform.position, Quaternion.identity);
                Vector2 dir = (enemy.planetTarget.position - enemy.transform.position).normalized;
                //proj.GetComponent<Rigidbody2D>().linearVelocity = dir * 5f;
                var enemyProjecctile = proj.GetComponent<EnemyProjectile>();
                enemyProjecctile.SetDirection(dir);
            }
            timer = cooldown;
        }
    }
}
