using UnityEngine;

[CreateAssetMenu(fileName = "ExtraShotUpgrade", menuName = "Upgrades/ExtraShot")]
public class ExtraShotUpgrade : Upgrade
{
    [SerializeField] int extraShotsAdded = 1;

    public int shotsPerSalvo;

    protected override void ApplyUpgradeInternal()
    {
        shotsPerSalvo = extraShotsAdded * currentLevel;
    }

    public override void Initialize()
    {
        ResetUpgrade();
        Upgrades.Inst.extraShot = this;
        shotsPerSalvo = 0;
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{extraShotsAdded} Extra Shot{(extraShotsAdded > 1 ? "s" : "")}";
    }
}
