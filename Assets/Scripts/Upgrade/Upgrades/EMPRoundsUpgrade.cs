using UnityEngine;

[CreateAssetMenu(fileName = "EMPRoundsUpgrade", menuName = "Upgrades/EMPRoundsUpgrade")]
public class EMPRoundsUpgrade : Upgrade
{
    [SerializeField] float baseStunDuration = 0.5f;
    [SerializeField] float stunDurationPerLevel = 0.2f;
    [SerializeField] float baseRadius = 2f;
    [SerializeField] float radiusPerLevel = 0.5f;
    [SerializeField] int baseShotsPerEMP = 5;

    public int shotsPerEMP = 5;
    public float radius;
    public float stunDuration;
    public int shotCounter;

    protected override void ApplyUpgradeInternal()
    {
        radius = baseRadius + (radiusPerLevel * currentLevel);
        stunDuration = baseStunDuration + (stunDurationPerLevel * currentLevel);
    }

    public override string GetUpgradeEffectText()
    {
        return $"Stuns enemies for {GetEmpStunDuration} seconds in {GetRadius} radius.";
    }

    public override void Initialize()
    {
        ResetUpgrade();
        Upgrades.Inst.empRounds = this;
        radius = baseRadius;
        stunDuration = baseStunDuration;
        shotCounter = 0;
        shotsPerEMP = baseShotsPerEMP;
    }

    private float GetRadius => baseRadius + (radiusPerLevel * NextLevel);
    private float GetEmpStunDuration => baseStunDuration + (stunDurationPerLevel * NextLevel);
}
