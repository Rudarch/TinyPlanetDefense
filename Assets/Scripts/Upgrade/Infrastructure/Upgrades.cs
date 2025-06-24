using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    public static Upgrades Inst { get; private set; }
    public List<Upgrade> AllUpgrades;

    // Projectile Upgrades
    [HideInInspector] public HeavyShellsUpgrade HeavyShells;
    [HideInInspector] public LifeSiphonUpgrade LifeSiphon;
    [HideInInspector] public ExplosiveRoundsUpgrade ExplosiveRounds;
    [HideInInspector] public ThermiteRoundsUpgrade ThermiteRounds;
    [HideInInspector] public PiercingAmmoUpgrade PiercingAmmo;
    [HideInInspector] public RicochetUpgrade Ricochet;
    
    // Planet Upgrades
    [HideInInspector] public CryoWaveUpgrade CryoWave;
    [HideInInspector] public EMPWaveUpgrade EmpWave;
    [HideInInspector] public AntigravityPulseUpgrade AntigravityPulse;
    [HideInInspector] public PlasmaHaloUpgrade PlasmaHalo;
    
    // Cannon Upgrades
    [HideInInspector] public ExtraShotUpgrade ExtraShot;
    [HideInInspector] public TurboLoader ReduceCooldown;
    [HideInInspector] public TwinBarrelUpgrade TwinBarrel;
    [HideInInspector] public IncreaseRotationSpeedUpgrade IncreaseRotationSpeed;
    [HideInInspector] public OverchargedShotUpgrade OverchargedShot;
    [HideInInspector] public OverheatProtocolUpgrade OverheatProtocol;

    // Special Upgrades
    [HideInInspector] public OrbitalWingUpgrade OrbitalWing;
    [HideInInspector] public OrbitalStrikeUpgrade OrbitalStrike;
    [HideInInspector] public OverchargedCapacitorsUpgrade OverchargedCapacitors;
    [HideInInspector] public AdaptiveFluxRegulatorUpgrade AdaptiveFluxRegulator;
    [HideInInspector] public OrbitalBladesUpgrade OrbitalBlades;
    [HideInInspector] public AnnihilatorRoundUpgrade AnihilatorRound;
    [HideInInspector] public MoltenCollapseUpgrade MoltenCollapse;

    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }

        Inst = this;

        foreach (var upgrade in AllUpgrades)
        {
            upgrade.Initialize();
        }

        Debug.Log($"{AllUpgrades.Count} upgrades were initialized.");
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
            if (upgrade.activationStyle != ActivationStyle.Timed)
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
        return AllUpgrades
            .Where(upg => upg.activationStyle == ActivationStyle.Toggle && upg.IsActivated)
            .Sum(upg => upg.energyDrainAmount);
    }

    public void ForceDeactivateAll()
    {
        foreach (var upgrade in AllUpgrades.Where(upgrade => upgrade.activationStyle == ActivationStyle.Toggle && upgrade.IsActivated))
        {
            upgrade.Deactivate();
        }
    }

    public void TickTimedUpgrades(float deltaTime)
    {
        foreach (var upgrade in AllUpgrades)
        {
            upgrade.TickUpgrade(deltaTime);
        }
    }
}
