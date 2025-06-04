using UnityEngine;

[CreateAssetMenu(fileName = "TwinBarrelUpgrade", menuName = "Upgrades/TwinBarrel")]
public class TwinBarrelUpgrade : Upgrade
{
    public override void ApplyUpgrade()
    {
        var cannon = FindFirstObjectByType<KineticCannon>();
        base.ApplyUpgrade();
        var upgradeStateManager = Upgrades.Inst;
        var state = upgradeStateManager.Cannon;
        if (state != null)
        {
            state.twinBarrelEnabled = true;
            cannon.EnableTwinMuzzles();
            upgradeStateManager.SetCannonUpgrades(state);
        }
        else Debug.Log($"{this.GetType().Name} cannot retrieve the state.");
    }

    public override string GetEffectText()
    {
        return "Adds a second barrel — 2 bullets per shot";
    }

    private void OnEnable()
    {
        isUnique = true;
    }
}
