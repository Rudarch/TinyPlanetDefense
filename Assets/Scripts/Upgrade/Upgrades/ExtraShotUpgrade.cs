using UnityEngine;

[CreateAssetMenu(fileName = "ExtraShotUpgrade", menuName = "Upgrades/ExtraShot")]
public class ExtraShotUpgrade : Upgrade
{
    [Header("Configuration")]
    [SerializeField] int extraShotsPerLevel = 1;
    [SerializeField] float baseShotInterval = 0.1f;

    //[Header("Values")]
    private int shotsPerSalvo;
    private float shotInterval;

    protected override void ApplyUpgradeInternal()
    {
        shotsPerSalvo = extraShotsPerLevel * currentLevel;
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
            if (IsActivated) return shotInterval;
            else return baseShotInterval;
        }
    }

    public int ExtraShotsPerSalvo
    {
        get
        {
            if (IsActivated) return shotsPerSalvo;
            else return 0;
        }
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{extraShotsPerLevel} Extra Shot{(extraShotsPerLevel > 1 ? "s" : "")}, {extraShotsPerLevel * NextLevel} in total";
    }
}
