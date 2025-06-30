using UnityEngine;

public class AccelerateOverTime : EnemyAbilityBase
{
    public float timeToMaxSpeed = 5f;
    private float timer = 0f;
    private float initialSpeed;

    public override void Initialize(Enemy enemy)
    {
        base.Initialize(enemy);
        initialSpeed = enemy.maxMovementSpeed;
        enemy.moveSpeed = 0.1f; // start nearly stopped
        timer = 0f;
    }

    public override void OnUpdate()
    {
        if (enemy == null || enemy.IsStunned()) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / timeToMaxSpeed);
        enemy.moveSpeed = Mathf.Lerp(0.1f, initialSpeed, t);
    }
}