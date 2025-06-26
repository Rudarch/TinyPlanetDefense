using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    public static Upgrades Inst { get; private set; }
    public List<Upgrade> RegualarUpgrades;
    public List<Upgrade> TacticalUpgrades;

    // Projectile Upgrades
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
    [HideInInspector] public TwinBarrelUpgrade TwinBarrel;
    [HideInInspector] public OverchargedShotUpgrade OverchargedShot;
    [HideInInspector] public OverheatProtocolUpgrade OverheatProtocol;

    // Special Upgrades
    [HideInInspector] public OrbitalWingUpgrade OrbitalWing;
    [HideInInspector] public OrbitalStrikeUpgrade OrbitalStrike;
    [HideInInspector] public OrbitalBladesUpgrade OrbitalBlades;
    [HideInInspector] public AnnihilatorRoundUpgrade AnihilatorRound;
    [HideInInspector] public MoltenCollapseUpgrade MoltenCollapse;

    // Tactical Upgrades
    [HideInInspector] public HighCaliberUpgrade HighCaliber;
    [HideInInspector] public CannonMasteryUpgrade CannonMastery;
    [HideInInspector] public EnergyMatrixUpgrade EnergyMatrix;

    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }

        Inst = this;

        foreach (var upgrade in RegualarUpgrades)
        {
            upgrade.Initialize();
        }

        foreach (var upgrade in TacticalUpgrades)
        {
            upgrade.Initialize();
        }

        Debug.Log($"{RegualarUpgrades.Count} upgrades were initialized.");
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
        return RegualarUpgrades
            .Where(upg => upg.activationStyle == ActivationStyle.Toggle && upg.IsActivated)
            .Sum(upg => upg.energyDrainAmount);
    }

    public void ForceDeactivateAll()
    {
        foreach (var upgrade in RegualarUpgrades.Where(upgrade => upgrade.activationStyle == ActivationStyle.Toggle && upgrade.IsActivated))
        {
            upgrade.Deactivate();
        }
    }

    public void TickTimedUpgrades(float deltaTime)
    {
        foreach (var upgrade in RegualarUpgrades)
        {
            upgrade.TickUpgrade(deltaTime);
        }
    }
}
