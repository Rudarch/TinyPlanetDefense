using UnityEngine;

[CreateAssetMenu(fileName = "CryoShellsUpgrade", menuName = "Upgrades/CryoShells")]
public class CryoShellsUpgrade : Upgrade
{
    [SerializeField] float baseSlowAmount = 0.3f;
    [SerializeField] float slowAmountPerLevel = 0.2f;
    [SerializeField] float baseSlowDuration = 5f;

    public float slowDuration;
    public float slowAmount;
    protected override void ApplyUpgradeInternal()
    {
        slowAmount = baseSlowAmount + (slowAmountPerLevel * currentLevel);
    }

    public override string GetUpgradeEffectText()
    {
        return $"{GetSlowAmmount}% for {slowDuration} seconds.";
    }

    public override void Initialize()
    {
        ResetUpgrade();
        Upgrades.Inst.cryoShells = this;
        slowDuration = baseSlowDuration;
        slowAmount = 0;
    }

    float GetSlowAmmount => (baseSlowAmount + (slowAmountPerLevel * NextLevel)) * 100f;
}
