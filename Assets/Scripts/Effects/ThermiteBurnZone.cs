
using System.Collections.Generic;
using UnityEngine;

public class ThermiteBurnZone : MonoBehaviour
{
    public float duration = 3f;
    public float burnDPS = 5f;
    public float burnDuration = 2f;
    public float radius = 1.5f;

    private float timer = 0f;
    private readonly List<Enemy> affectedEnemies = new();

    void Start()
    {
        float scale = radius / 1;
        transform.localScale = new Vector3(scale, scale, 1f);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            Destroy(gameObject);
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                var burn = enemy.GetComponent<BurningEffect>();
                if (burn == null || !burn.IsActive())
                {
                    if (burn == null)
                    {
                        burn = enemy.gameObject.AddComponent<BurningEffect>();
                        burn.baseDamagePerSecond = burnDPS;
                        burn.burnDuration = burnDuration;
                    }

                    burn.ApplyOrRefresh(burnDPS, burnDuration);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.3f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.4f, 0f, 0.4f); // orange translucent
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
