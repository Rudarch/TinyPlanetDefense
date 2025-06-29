using UnityEngine;

public class UnholyBarrier : EnemyAbilityBase
{
    public float absorbPercent = 0.5f;
    public float activeDuration = 5f;
    public float cooldown = 20f;
    public GameObject barierVFX;

    private float cooldownTimer = 0f;
    private float activeTimer = 0f;
    private bool isActive = false;

    public override void OnUpdate()
    {
        if (isActive)
        {
            activeTimer -= Time.deltaTime;
            if (activeTimer <= 0f)
            {
                isActive = false;
                cooldownTimer = cooldown;
                if (barierVFX) barierVFX.SetActive(false);
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public override void OnDamaged(float amount)
    {
        if (!isActive && cooldownTimer <= 0f && amount > 0f)
        {
            ActivateBarrier();
        }

        if (isActive && amount > 0f)
        {
            float reduced = amount * absorbPercent;
            enemy.TakeDamage(-reduced); // absorb portion of damage
        }
    }

    private void ActivateBarrier()
    {
        isActive = true;
        activeTimer = activeDuration;
        if (barierVFX) barierVFX.SetActive(true);
        // Optional: play sound or visual pulse
    }
}
