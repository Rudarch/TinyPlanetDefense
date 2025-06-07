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
        return $"Every {interval} seconds, your next shot deals {damageMultiplierPerLevel * NextLevel}x damage and is larger.";
    }

    public override void Initialize()
    {
        ResetUpgrade();
        Upgrades.Inst.overchargedShot = this;
        damageMultiplier = damageMultiplierPerLevel;
    }
}
