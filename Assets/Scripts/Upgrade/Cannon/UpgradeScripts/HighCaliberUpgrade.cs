using UnityEngine;

[CreateAssetMenu(fileName = "HighCaliberUpgrade", menuName = "Upgrades/HighCaliber")]
public class HighCaliberUpgrade : CannonUpgrade
{
    public float knockbackForce = 5f;
    public float scaleMultiplier = 1.3f;

    public override void ApplyUpgrade(GameObject cannon)
    {
        base.ApplyUpgrade(cannon);

        var weapon = cannon.GetComponentInChildren<KineticCannon>();
        if (weapon != null)
        {
            weapon.knockbackEnabled = true;
            weapon.knockbackForce = knockbackForce;

            // Increase projectile size
            weapon.projectileScale *= scaleMultiplier;
        }
    }

    public override string GetEffectText()
    {
        return $"Projectiles knock enemies back by {knockbackForce} and are {Mathf.RoundToInt((scaleMultiplier - 1f) * 100)}% larger.";
    }
}
