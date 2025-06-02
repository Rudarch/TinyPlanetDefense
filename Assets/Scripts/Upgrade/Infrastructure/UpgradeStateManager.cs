using UnityEngine;

public class UpgradeStateManager : MonoBehaviour
{
    public static UpgradeStateManager Instance { get; private set; }

    public ProjectileUpgradeState ProjectileUpgrades { get; private set; }
    public CannonUpgradeState CannonUpgrades { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        ProjectileUpgrades = new ProjectileUpgradeState();
        CannonUpgrades = new CannonUpgradeState();
    }

    public void SetProjectileUpgrades(ProjectileUpgradeState state)
    {
        ProjectileUpgrades = state;
    }

    public void SetCannonUpgrades(CannonUpgradeState state)
    {
        CannonUpgrades = state;
    }
}
