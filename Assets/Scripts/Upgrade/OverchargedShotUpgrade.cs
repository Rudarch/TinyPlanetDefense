using UnityEngine;

[CreateAssetMenu(fileName = "OverchargedShotUpgrade", menuName = "Upgrades/OverchargedShot")]
public class OverchargedShotUpgrade : Upgrade
{
    [SerializeField] float damageMultiplierPerLevel = 3f;
    public float interval = 5f;
    public float damageMultiplier = 1f;
    public float scaleMultiplier = 1.5f;

    protected override void ApplyUpgradeInternal()
    {
        damageMultiplier = damageMultiplierPerLevel * currentLevel;
    }

    public override string GetUpgradeEffectText()
    {
        return $"Every {interval} seconds, {damageMultiplierPerLevel * NextLevel}x damage.";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.OverchargedShot = this;
        damageMultiplier = damageMultiplierPerLevel;
    }
}
