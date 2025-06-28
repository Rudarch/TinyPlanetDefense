using UnityEngine;

public class BurnAura : EnemyAbilityBase
{
    public float radius = 2f;
    public float burnDPS = 3f;

    public override void OnUpdate()
    {
        foreach (var hit in Physics2D.OverlapCircleAll(enemy.transform.position, radius))
        {
            var other = hit.GetComponent<Enemy>();
            if (other != null && other != enemy)
            {
                var burn = other.GetComponent<BurningEffect>() ?? other.gameObject.AddComponent<BurningEffect>();
                burn.ApplyOrRefresh(burnDPS, 2f);
            }
        }
    }
}
