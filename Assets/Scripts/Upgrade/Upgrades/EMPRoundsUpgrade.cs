using UnityEngine;

[CreateAssetMenu(fileName = "EMPRoundsUpgrade", menuName = "Upgrades/EMPRoundsUpgrade")]
public class EMPRoundsUpgrade : Upgrade
{
    public float baseStunDuration = 0.5f;
    public float stunDurationPerLevel = 0.2f;
    public float baseRadius = 2f;
    public float radiusPerLevel = 0.5f;
    public int shotsPerEMP = 5;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();

        var upgradeStateManager = Upgrades.Inst;
        var state = upgradeStateManager.Projectile;
        if (state != null)
        {
            state.empLevel++;
            state.empEnabled = true;
            state.shotsPerEMP = shotsPerEMP;
            state.empRadius = GetRadius;
            state.empStunDuration = GetEmpStunDuration;
            upgradeStateManager.SetProjectileUpgrades(state);
        }
    }

    public override string GetEffectText()
    {
        return $"Stuns enemies for {GetEmpStunDuration} seconds in {GetRadius} radius.";
    }

    private float GetRadius => baseRadius + (radiusPerLevel * Upgrades.Inst.Projectile.empLevel);
    private float GetEmpStunDuration => baseStunDuration + (stunDurationPerLevel * Upgrades.Inst.Projectile.empLevel);
}
