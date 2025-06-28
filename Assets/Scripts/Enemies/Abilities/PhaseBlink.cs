using UnityEngine;

public class PhaseBlink : EnemyAbilityBase
{
    public float blinkCooldown = 5f;
    public float blinkRange = 2f;
    private float cooldownTimer = 0f;

    public override void OnUpdate()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            Vector2 offset = Random.insideUnitCircle.normalized * blinkRange;
            enemy.transform.position += (Vector3)offset;
            cooldownTimer = blinkCooldown;
        }
    }
}
