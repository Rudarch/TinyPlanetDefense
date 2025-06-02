using UnityEngine;

[CreateAssetMenu(fileName = "ReduceCooldownUpgrade", menuName = "Upgrades/ReduceCooldown")]
public class ReduceCooldownUpgrade : Upgrade
{
    [Range(0f, 1f)]
    public float cooldownReductionPercent = 0.25f;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        var upgradeStateManager = UpgradeStateManager.Instance;
        var state = upgradeStateManager.CannonUpgrades;
        if (state != null)
        {
            state.cooldownReductionMultiplier = Mathf.Max(0.1f, state.cooldownReductionMultiplier * 0.75f);
            state.shotInterval = Mathf.Max(0.05f, state.shotInterval * 0.75f);
            upgradeStateManager.SetCannonUpgrades(state);
        }
        else Debug.Log($"{this.GetType().Name} cannot retrieve the state.");
    }

    public override string GetEffectText()
    {
        int percent = Mathf.RoundToInt(cooldownReductionPercent * 100f);
        return $"-{percent}% Cooldown";
    }
}
