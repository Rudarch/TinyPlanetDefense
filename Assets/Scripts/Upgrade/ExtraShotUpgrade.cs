using UnityEngine;

[CreateAssetMenu(fileName = "ExtraShotUpgrade", menuName = "Upgrades/ExtraShot")]
public class ExtraShotUpgrade : Upgrade
{
    [Header("Chance-based Extra Shot")]
    [SerializeField] float baseChance = 0.35f;
    [SerializeField] float chancePerLevel = 0.05f;
    public float extraShotInterval = 0.1f;

    public float TotalChance { get => baseChance + (CurrentLevel * chancePerLevel); }

    public override string GetUpgradeEffectText()
    {
        float previewChance = baseChance + (NextLevel * chancePerLevel);
        return $"Chance to fire extra shots: {previewChance * 100f:F0}%";
    }

    protected override void InitializeInternal()
    {
        Upgrades.Inst.ExtraShot = this;
    }


    public int GetExtraShotCount()
    {
        if (!IsActivated)
            return 0;

        int count = Mathf.FloorToInt(TotalChance);
        float remainder = TotalChance - count;

        if (Random.value < remainder)
            count++;

        return count;
    }
}
