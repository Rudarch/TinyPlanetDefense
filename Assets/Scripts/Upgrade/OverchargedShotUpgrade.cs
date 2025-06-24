using UnityEngine;

[CreateAssetMenu(fileName = "OverchargedShotUpgrade", menuName = "Upgrades/OverchargedShot")]
public class OverchargedShotUpgrade : Upgrade
{
    [SerializeField] float baseInterval = 3f;
    [SerializeField] float intervalReductionPerLevel = 1f;
    public float damageMultiplier = 3f;
    public float scaleMultiplier = 1.5f;

    public float Interval { get => baseInterval - intervalReductionPerLevel * CurrentLevel; }

    public override string GetUpgradeEffectText()
    {
        return $"Every {baseInterval - intervalReductionPerLevel * NextLevel} seconds, {damageMultiplier}x damage.";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.OverchargedShot = this;
    }
}
