using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    public static Upgrades Inst { get; private set; }

    [Header("Projectile Upgrades")]
    public IncreaseDamageUpgrade increaseDamage;
    public LifeSiphonUpgrade lifeSiphon;
    public ExplosiveRoundsUpgrade explosiveRounds;
    public ThermiteRoundsUpgrade thermiteRounds;
    public PiercingAmmoUpgrade piercingAmmo;
    public RicochetUpgrade ricochet;

    [Header("Planet Upgrades")]
    public CryoWaveUpgrade cryoWave;
    public EMPWaveUpgrade empWave;
    public AntigravityPulseUpgrade antigravityPulse;

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

    public PlanetUpgradeHandler planetUpgradeHandler;
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

        var planet = GameObject.FindWithTag("Planet");
        planetUpgradeHandler = planet.GetComponent<PlanetUpgradeHandler>();
    }

    public void ToggleUpgrade(Upgrade upgrade)
    {
        if (upgrade == null)
        {
            Debug.LogWarning("ToggleUpgrade called with null upgrade.");
            return;
        }

        if (upgrade.IsEnabled)
        {
            Debug.Log($"Deactivating upgrade: {upgrade.upgradeName}");
            upgrade.Deactivate();
        }
        else
        {
            Debug.Log($"Activating upgrade: {upgrade.upgradeName}");
            upgrade.Activate();
        }

        UpgradeButtonPanel.Inst?.GetButtonForUpgrade(upgrade)?.UpdateVisual(upgrade.IsEnabled);
    }

    public float GetTotalActiveDrain()
    {
        return allUpgrades
            .Where(upg => upg.activatable && upg.IsEnabled)
            .Sum(upg => upg.energyCostPerSecond);
    }

    public void ForceDeactivateAll()
    {
        foreach (var upgrade in allUpgrades.Where(u => u.activatable && u.IsEnabled))
        {
            upgrade.Deactivate();
            UpgradeButtonPanel.Inst?.GetButtonForUpgrade(upgrade)?.UpdateVisual(false);
        }
    }
}
