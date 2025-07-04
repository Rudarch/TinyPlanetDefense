using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/OverheatProtocol")]
public class OverheatProtocolUpgrade : Upgrade
{
    [SerializeField] [Range(0f, 1f)] float energyThreshold = 0.25f;
    [SerializeField] float damageMultiplier = 1.5f;

    public bool IsOverheating { get; private set; } = false;

    protected override void InitializeInternal()
    {
        EnergySystem.OnEnergyChanged += HandleEnergyChanged;
        Upgrades.Inst.OverheatProtocol = this;
    }

    public float OverheatDamageMultiplier
    {
        get
        {
            if (IsActivated && IsOverheating)
            {
                return damageMultiplier;
            }
            return 1f;
        }
        protected set
        {

        }
    }
        public override string GetUpgradeEffectText()
    {
        return $"{OverheatDamageMultiplier * 100f}% damage increase when energy below {energyThreshold}%.";
    }

    private void HandleEnergyChanged(float current, float max)
    {
        IsOverheating = (current / max) < energyThreshold;
    }
}
