using UnityEngine;

[CreateAssetMenu(fileName = "EMPRoundsUpgrade", menuName = "Upgrades/EMPRoundsUpgrade")]
public class EMPRoundsUpgrade : Upgrade
{
    [SerializeField] float baseStunDuration = 0.5f;
    [SerializeField] float stunDurationPerLevel = 0.2f;
    [SerializeField] float baseRadius = 2f;
    [SerializeField] float radiusPerLevel = 0.5f;
    [SerializeField] int baseShotsPerEMP = 5;

    private int shotsPerEMP = 5;
    private int shotCounter;

    private float radius;
    private float stunDuration;

    protected override void ApplyUpgradeInternal()
    {
        radius = baseRadius + (radiusPerLevel * currentLevel);
        stunDuration = baseStunDuration + (stunDurationPerLevel * currentLevel);
    }

    public override string GetUpgradeEffectText()
    {
        return $"{GetEmpStunDuration} seconds stun in {GetRadius} radius.";
    }

    public override void Initialize()
    {
        ResetUpgrade();
        Upgrades.Inst.empRounds = this;
        Radius = baseRadius;
        StunDuration = baseStunDuration;
        ShotCounter = 0;
        ShotsPerEMP = baseShotsPerEMP;
    }

    private float GetRadius => baseRadius + (radiusPerLevel * NextLevel);
    private float GetEmpStunDuration => baseStunDuration + (stunDurationPerLevel * NextLevel);

    public int ShotsPerEMP { get => shotsPerEMP; set => shotsPerEMP = value; }
    public int ShotCounter { get => shotCounter; set => shotCounter = value; }
    public float Radius { get => radius; set => radius = value; }
    public float StunDuration { get => stunDuration; set => stunDuration = value; }

}
