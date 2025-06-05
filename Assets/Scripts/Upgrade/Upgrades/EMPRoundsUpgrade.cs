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
        if (IsMaxedOut) return;

        var state = Upgrades.Inst.Projectile;
        state.empLevel = currentLevel;
        state.empEnabled = true;
        state.shotsPerEMP = shotsPerEMP;
        state.empRadius = baseRadius + (radiusPerLevel * currentLevel);
        state.empStunDuration = baseStunDuration + (stunDurationPerLevel * currentLevel);
    }

    public override string GetUpgradeEffectText()
    {
        return $"Stuns enemies for {GetEmpStunDuration} seconds in {GetRadius} radius.";
    }

    private float GetRadius => baseRadius + (radiusPerLevel * (currentLevel + 1));
    private float GetEmpStunDuration => baseStunDuration + (stunDurationPerLevel * (currentLevel + 1));
}
