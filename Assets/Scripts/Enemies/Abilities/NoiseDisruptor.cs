using UnityEngine;
public class NoiseDisruptor : EnemyAbilityBase
{
    public float range = 5f;
    public float ammount = 3f;

    public override void OnUpdate()
    {
        if (Vector3.Distance(enemy.transform.position, enemy.planetTarget.position) < range)
        {
            UIEffectsController.Inst.ApplyBlur("enemy_noise", ammount);
        }
    }
}
