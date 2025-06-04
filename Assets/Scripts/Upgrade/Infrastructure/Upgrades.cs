using UnityEngine;

public class Upgrades : MonoBehaviour
{
    public static Upgrades Inst { get; private set; }

    public ProjectileUpgradeState Projectile { get; private set; }
    public CannonUpgradeState Cannon { get; private set; }

    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(this);
            return;
        }
        Inst = this;

        Projectile = new ProjectileUpgradeState();
        Cannon = new CannonUpgradeState();
    }

    public void SetProjectileUpgrades(ProjectileUpgradeState state)
    {
        Projectile = state;
    }

    public void SetCannonUpgrades(CannonUpgradeState state)
    {
        Cannon = state;
    }
}
