using UnityEngine;

[CreateAssetMenu(fileName = "EnergyMatrixUpgrade", menuName = "Upgrades/EnergyMatrix")]
public class EnergyMatrixUpgrade : Upgrade
{
    [SerializeField] private float bonusRegen = 1.5f;
    [SerializeField] private float bonusCapacity = 15f;

    protected override void InitializeInternal()
    {
        Upgrades.Inst.EnergyMatrix = this;
    }

    public float BonusEnergyRegen
    {
        get
        {
            if (IsActivated) return bonusRegen * CurrentLevel;
            else return 0;
        }
    }

    public float BonusMaxEnergy
    {
        get
        {
            if (IsActivated) return bonusCapacity * CurrentLevel;
            else return 0;
        }
    }

    public override string GetUpgradeEffectText()
    {
        return $"+{bonusRegen * NextLevel:F1}/s Regen, +{bonusCapacity * NextLevel:F0} Max Energy";
    }
}