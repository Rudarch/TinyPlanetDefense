using UnityEngine;

public class AcceleratingMovement : StraightLineMovement
{
    public float timeToMaxSpeed = 5f;
    private float timer = 0f;
    private float maxSpeed;

    public override void Initialize(Enemy enemy)
    {
        base.Initialize(enemy);
        maxSpeed = enemy.moveSpeed;
        enemy.moveSpeed = 0.1f;
        timer = 0f;
    }

    public override void TickMovement()
    {
        if (enemy == null || enemy.IsStunned()) return;

        // accelerate up to maxSpeed
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / timeToMaxSpeed);
        enemy.moveSpeed = Mathf.Lerp(0.1f, maxSpeed, t);

        base.TickMovement(); // reuse straight line motion
    }
}