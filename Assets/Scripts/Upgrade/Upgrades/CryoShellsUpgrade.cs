using UnityEngine;

[CreateAssetMenu(fileName = "CryoShellsUpgrade", menuName = "Upgrades/CryoShells")]
public class CryoShellsUpgrade : Upgrade
{
    public float baseSlowAmount = 0.3f;
    public float slowAmountPerLevel = 0.2f;
    public float slowDuration = 5f;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        if (IsMaxedOut) return;

        var state = Upgrades.Inst.Projectile;
        state.cryoEnabled = true;
        state.cryoAmount = GetSlowAmmount;
        state.cryoDuration = slowDuration;
    }

    public override string GetUpgradeEffectText()
    {
        return $"Projectiles slow enemies by {GetSlowAmmount}% for {slowDuration} seconds.";
    }

    float GetSlowAmmount => baseSlowAmount + (slowAmountPerLevel * currentLevel);
}
