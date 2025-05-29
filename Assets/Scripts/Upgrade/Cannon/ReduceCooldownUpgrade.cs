using UnityEngine;

[CreateAssetMenu(fileName = "ReduceCooldownUpgrade", menuName = "Upgrades/ReduceCooldown")]
public class ReduceCooldownUpgrade : CannonUpgrade
{
    public float cooldownReduction = 0.3f;

    public override void ApplyUpgrade(GameObject cannon)
    {
        base.ApplyUpgrade(cannon);
        var weapon = cannon.GetComponentInChildren<KineticCannon>();
        if (weapon != null)
        {
            weapon.cooldown = Mathf.Max(0.1f, weapon.cooldown - cooldownReduction);
        }
    }
}