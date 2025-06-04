using UnityEngine;

public class EMPShockwaveEffect : MonoBehaviour
{
    public float expandSpeed = 5f;
    public float fadeDuration = 0.2f;
    public Color effectColor;

    private SpriteRenderer sr;
    private float timer = 0f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        transform.localScale = Vector3.zero;
        sr.color = effectColor;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float scale = Mathf.Min(Upgrades.Inst.Projectile.empRadius, timer * expandSpeed);
        float worldScale = scale * 2f;

        transform.localScale = new Vector3(worldScale, worldScale, 1f);

        float alpha = Mathf.Lerp(0.5f, 0f, timer / fadeDuration);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

        if (timer > fadeDuration)
            Destroy(gameObject);
    }


    public void StunNearbyEnemies(Vector3 center)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, Upgrades.Inst.Projectile.empRadius);
        foreach (var hit in hits)
        {
            var enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                var stun = enemy.GetComponent<EMPStunEffect>();
                if (stun == null)
                    stun = enemy.gameObject.AddComponent<EMPStunEffect>();

                stun.ApplyStun(Upgrades.Inst.Projectile.empStunDuration);
            }
        }
    }
}
