using UnityEngine;

[CreateAssetMenu(fileName = "EnergySiphonUpgrade", menuName = "Upgrades/EnergySiphon")]
public class LifeSiphonUpgrade : Upgrade
{
    [Range(0f, 1f)] public float healFraction = 0.01f;
    public float lifeSiphonFraction;

    protected override void ApplyUpgradeInternal()
    {
        lifeSiphonFraction = healFraction;
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.lifeSiphon = this;
        lifeSiphonFraction = healFraction;
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{healFraction * 100f}% HP per kill.";
    }
}
