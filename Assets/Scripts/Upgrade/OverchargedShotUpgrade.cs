using UnityEngine;

[CreateAssetMenu(fileName = "OverchargedShotUpgrade", menuName = "Upgrades/OverchargedShot")]
public class OverchargedShotUpgrade : Upgrade
{
    [SerializeField] float baseInterval = 3f;
    [SerializeField] float intervalReductionPerLevel = 1f;
    public float interval = 5f;
    public float damageMultiplier = 3f;
    public float scaleMultiplier = 1.5f;

    protected override void ApplyUpgradeInternal()
    {
        interval = baseInterval - intervalReductionPerLevel * currentLevel;
    }

    public override string GetUpgradeEffectText()
    {
        return $"Every {baseInterval - intervalReductionPerLevel * NextLevel} seconds, {damageMultiplier}x damage.";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.OverchargedShot = this;
        interval = baseInterval;
    }
}
