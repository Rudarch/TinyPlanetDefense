using UnityEngine;

public class HealAlliesNearby : EnemyAbilityBase
{
    public float healAmount = 5f;
    public float radius = 3f;
    public float cooldown = 10f;

    public float fadeDuration = 1f;
    public GameObject healVFX;
    public Sprite waveImage;
    public Color color;

    private float timer = 0f;

    public override void OnUpdate()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        var nearby = EnemyManager.Inst.GetEnemiesInRange(enemy.transform.position, radius);
        bool healedAny = false;

        foreach (var target in nearby)
        {
            if (target.health < target.maxHealth)
            {
                target.UpdateHealth(-healAmount);
                healedAny = true;
            }
        }

        if (healedAny)
        {
            var healEffect = Instantiate(healVFX, transform.position, Quaternion.identity);
            var we = healEffect.GetComponent<WaveEffect>();
            we.UpdateVisuals(waveImage, color, radius, fadeDuration);
            timer = cooldown;
        }
    }
}