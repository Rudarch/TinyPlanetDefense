using UnityEngine;

[CreateAssetMenu(fileName = "HighCaliberUpgrade", menuName = "Upgrades/HighCaliber")]
public class HighCaliberUpgrade : CannonUpgrade
{
    public float knockbackForce = 5f;

    public override void ApplyUpgrade(GameObject cannon)
    {
        base.ApplyUpgrade(cannon);
        var weapon = cannon.GetComponentInChildren<KineticCannon>();
        if (weapon != null)
        {
            weapon.knockbackEnabled = true;
            weapon.knockbackForce = knockbackForce;
        }
    }

    public override string GetEffectText()
    {
        return $"Projectiles knock enemies back";
    }
}
