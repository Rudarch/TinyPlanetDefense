using UnityEngine;

[CreateAssetMenu(fileName = "OverchargedShotUpgrade", menuName = "Upgrades/OverchargedShot")]
public class OverchargedShotUpgrade : Upgrade
{
    public float interval = 5f;
    public float damageMultiplier = 3f;
    public float scaleMultiplier = 1.5f;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        var state = Upgrades.Inst.Projectile;
        state.overchargedEnabled = true;
        state.overchargeInterval = interval;
        state.overchargeDamageMultiplier = damageMultiplier;
        state.overchargeScaleMultiplier = scaleMultiplier;
        Upgrades.Inst.SetProjectileUpgrades(state);
    }

    public override string GetEffectText()
    {
        return $"Every {interval} seconds, your next shot deals {damageMultiplier}x damage and is larger.";
    }

    private void OnEnable()
    {
        isUnique = true;
    }
}
