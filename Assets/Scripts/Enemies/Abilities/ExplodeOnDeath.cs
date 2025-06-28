using UnityEngine;

public class ExplodeOnDeath : EnemyAbilityBase
{
    public float radius = 2f;
    public float damage = 5f;
    public GameObject explosionVFX;
    public Sprite explosionHFX;
    public Color color;

    public override void OnDeath()
    {
        if (explosionVFX != null)
        {
            var explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            var we = explosion.GetComponent<WaveEffect>();
            if (we != null) 
            {
                we.waveImage = explosionHFX;
                we.maxRadius = radius;
                we.effectColor = color;
            }
        }
        var hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hit in hits)
        {
            var other = hit.GetComponent<Planet>();
            if (other != null && other != enemy)
            {
                other.TakeDamage(damage);
            }
        }
    }
}
