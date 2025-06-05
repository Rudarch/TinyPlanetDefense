using UnityEngine;

[CreateAssetMenu(fileName = "EnergySiphonUpgrade", menuName = "Upgrades/EnergySiphon")]
public class EnergySiphonUpgrade : Upgrade
{
    [Range(0f, 1f)] public float healFraction = 0.01f;

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        var upgradeStateManager = Upgrades.Inst;
        var state = upgradeStateManager.Projectile;
        if (state != null)
        {
            state.energySiphonEnabled = true;
            state.energySiphonFraction = healFraction;
            upgradeStateManager.SetProjectileUpgrades(state);
        }
    }

    public override string GetEffectText()
    {
        return $"Kills heal the planet for {healFraction * 100f}% HP.";
    }

    private void OnEnable()
    {
        isUnique = true;
    }
}
