using static UnityEngine.GraphicsBuffer;
using UnityEngine;

public class DisableTurretsOnDeath : EnemyAbilityBase
{
    public float disableDuration = 2f;

    public override void OnDeath()
    {
        foreach (var turret in FindObjectsByType<WeaponSystem>(FindObjectsSortMode.InstanceID))
        {
            if (Vector3.Distance(turret.transform.position, enemy.transform.position) < 3f)
            {
                turret.DisableTemporarily(disableDuration);
            }
        }
    }
}
