using UnityEngine;

[CreateAssetMenu(fileName = "ExtraShotUpgrade", menuName = "Upgrades/ExtraShot")]
public class ExtraShotUpgrade : Upgrade
{
    [SerializeField] int extraShotsAdded = 1;
    [SerializeField] float baseShotInterval = 0.1f;

    private int shotsPerSalvo;
    private float shotInterval;

    protected override void ApplyUpgradeInternal()
    {
        shotsPerSalvo = extraShotsAdded * currentLevel;
        shotInterval = baseShotInterval;
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.extraShot = this;
        shotsPerSalvo = 0;
        shotInterval = baseShotInterval;
    }


    public float ExtraShotInterval
    {
        get
        {
            if (IsEnabled) return shotInterval;
            else return baseShotInterval;
        }
    }

    public int ExtraShotsPerSalvo
    {
        get
        {
            if (IsEnabled) return shotsPerSalvo;
            else return 0;
        }
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{extraShotsAdded} Extra Shot{(extraShotsAdded > 1 ? "s" : "")}, {extraShotsAdded * NextLevel} in total";
    }

    //private float GetShotIntervalInternal()
    //{
    //    float multiplier = Mathf.Clamp01(1f - cooldownReductionPercent);

    //    return Mathf.Max(0.05f, shotInterval * multiplier);
    //}
}
