using UnityEngine;

[CreateAssetMenu(fileName = "HighCaliberUpgrade", menuName = "Upgrades/HighCaliber")]
public class HighCaliberUpgrade : Upgrade
{
    public float knockbackForce = 5f;
    public float scaleMultiplier = 1.3f;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        if (IsMaxedOut) return;

        var state = Upgrades.Inst.Projectile;
        state.knockbackEnabled = true;
        state.knockbackForce = knockbackForce;
        state.projectileScale *= scaleMultiplier;
    }

    public override string GetUpgradeEffectText()
    {
        return $"Projectiles knock enemies back by {knockbackForce} and are {Mathf.RoundToInt((scaleMultiplier - 1f) * 100)}% larger.";
    }
}
