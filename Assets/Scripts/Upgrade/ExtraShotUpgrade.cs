using UnityEngine;

[CreateAssetMenu(fileName = "ExtraShotUpgrade", menuName = "Upgrades/ExtraShot")]
public class ExtraShotUpgrade : Upgrade
{
    [Header("Chance-based Extra Shot")]
    [SerializeField] float baseChance = 0.35f;
    [SerializeField] float chancePerLevel = 0.05f;
    public float extraShotInterval = 0.1f;

    [SerializeField] float totalChance = 0;

    public override string GetUpgradeEffectText()
    {
        float totalChance = baseChance + (NextLevel * chancePerLevel);
        return $"Chance to fire an extra shot: {totalChance * 100f:F0}%";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.ExtraShot = this;
        totalChance = 0f;
    }

    protected override void ApplyUpgradeInternal()
    {
        totalChance = baseChance + (currentLevel * chancePerLevel);
    }

    public bool RollExtraShot()
    {
        if (!IsActivated) return false;

        return Random.value < totalChance;
    }
}