using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    [Header("All Upgrades")]
    public List<Upgrade> allUpgrades;

    public static Upgrades Inst { get; private set; }

    [Header("Projectile Upgrades")]
    public HeavyShellsUpgrade heavyShells;
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
    public OverheatProtocolUpgrade overheatProtocol;

    [Header("Special Upgrades")]
    public OrbitalWingUpgrade orbitalWing;
    public OverchargedCapacitorsUpgrade overchargedCapacitors;
    public AdaptiveFluxRegulatorUpgrade adaptiveFluxRegulator;

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
    }

    public void ToggleUpgrade(Upgrade upgrade)
    {
        if (upgrade == null)
        {
            Debug.LogWarning("ToggleUpgrade called with null upgrade.");
            return;
        }

        if (upgrade.IsActivated)
        {
            upgrade.Deactivate();
        }
        else if (upgrade.activationStyle == ActivationStyle.Timed)
        {
            if (upgrade.IsReadyForActivation && EnergySystem.Inst.HasEnough(upgrade.activationEnergyAmount))
            {
                upgrade.Activate();
            }
        }
        else 
        {
            upgrade.Activate();
        }
    }

    public float GetTotalActiveDrain()
    {
        return allUpgrades
            .Where(upg => upg.activationStyle == ActivationStyle.Toggle && upg.IsActivated)
            .Sum(upg => upg.energyDrainAmount);
    }

    public void ForceDeactivateAll()
    {
        foreach (var upgrade in allUpgrades.Where(upgrade => upgrade.activationStyle == ActivationStyle.Toggle && upgrade.IsActivated))
        {
            upgrade.Deactivate();
        }
    }

    public void TickTimedUpgrades(float deltaTime)
    {
        foreach (var upgrade in allUpgrades)
        {
            upgrade.TickUpgrade(deltaTime);
        }
    }
}
