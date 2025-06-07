using UnityEngine;

[CreateAssetMenu(fileName = "ExtraShotUpgrade", menuName = "Upgrades/ExtraShot")]
public class ExtraShotUpgrade : Upgrade
{
    [SerializeField] int extraShotsAdded = 1;

    public int extraShots;

    protected override void ApplyUpgradeInternal()
    {
        extraShots = extraShotsAdded * currentLevel;
    }

    public override void Initialize()
    {
        ResetUpgrade();
        Upgrades.Inst.extraShot = this;
        extraShots = 0;
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{extraShotsAdded} Extra Shot{(extraShotsAdded > 1 ? "s" : "")}";
    }
}
