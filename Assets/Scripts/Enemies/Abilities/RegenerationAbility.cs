using UnityEngine;

public class RegenerationAbility : EnemyAbilityBase
{
    public float regenRate = 2f; // HP per second
    public float checkInterval = 0.5f;

    private float timer = 0f;

    public override void OnUpdate()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        timer = checkInterval;

        if (enemy != null && enemy.health < enemy.maxHealth)
        {
            enemy.UpdateHealth(-regenRate * checkInterval); // heal as negative damage
        }
    }
}