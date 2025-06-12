using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    public static Upgrades Inst { get; private set; }

    [Header("Projectile Upgrades")]
    public CryoShellsUpgrade cryoShells;
    public IncreaseDamageUpgrade increaseDamage;
    public EMPRoundsUpgrade empRounds;
    public LifeSiphonUpgrade lifeSiphon;
    public ExplosiveRoundsUpgrade explosiveRounds;
    public ThermiteRoundsUpgrade thermiteRounds;
    public HighCaliberUpgrade highCaliber;
    public PiercingAmmoUpgrade piercingAmmo;
    public RicochetUpgrade ricochet;

    [Header("Cannon Upgrades")]
    public ExtraShotUpgrade extraShot;
    public TurboLoader reduceCooldown;
    public TwinBarrelUpgrade twinBarrel;
    public IncreaseRotationSpeedUpgrade increaseRotationSpeed;
    public OverchargedShotUpgrade overchargedShot;

    [Header("Special Upgrades")]
    public OrbitalWingUpgrade orbitalWing;

    [Header("All Upgrades")]
    public List<Upgrade> allUpgrades;

    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }

        Inst = this;

        foreach (var upgrade in allUpgrades)
        {
            upgrade.Initialize();
        }
        Debug.Log($"{allUpgrades.Count} upgrades were initialized.");
    }

    public void ToggleUpgrade(Upgrade upgrade)
    {
        if (upgrade == null)
        {
            Debug.LogWarning("ToggleUpgrade called with null upgrade.");
            return;
        }

        if (upgrade.enabled)
        {
            Debug.Log($"Deactivating upgrade: {upgrade.upgradeName}");
            upgrade.OnDeactivate();
        }
        else
        {
            Debug.Log($"Activating upgrade: {upgrade.upgradeName}");
            upgrade.OnActivate();
        }

        UpgradeButtonPanel.Inst?.GetButtonForUpgrade(upgrade)?.UpdateVisual(upgrade.enabled);
    }

    public float GetTotalActiveDrain()
    {
        return allUpgrades
            .Where(upg => upg.activatable && upg.enabled)
            .Sum(upg => upg.energyCostPerSecond);
    }

    public void ForceDeactivateAll()
    {
        foreach (var upgrade in allUpgrades.Where(u => u.activatable && u.enabled))
        {
            upgrade.OnDeactivate();
            UpgradeButtonPanel.Inst?.GetButtonForUpgrade(upgrade)?.UpdateVisual(false);
        }
    }
}
