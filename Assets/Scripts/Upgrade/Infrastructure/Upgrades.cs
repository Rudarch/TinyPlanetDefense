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
            upgrade.ResetUpgrade();
        }

        foreach (var upgrade in TacticalUpgrades)
        {
            upgrade.ResetUpgrade();
        }

        Debug.Log($"Regualar={RegualarUpgrades.Count} and Tactical={TacticalUpgrades.Count} upgrades were reset.");
    }

    void Update()
    {
        Upgrades.Inst.TickUpgrades(Time.deltaTime);
    }

    public void ActivateUpgrade(Upgrade upgrade)
    {
        if (upgrade == null)
        {
            Debug.LogWarning("ActivateUpgrade called with null upgrade.");
            return;
        }

        if (upgrade.IsActivated)
        {
            return;
        }
        else if (upgrade.activationStyle == ActivationStyle.Passive)
        {
            upgrade.Activate();
        }
        else
        {
            if (upgrade.IsReadyForActivation)
            {
                upgrade.Activate();
            }
        }
    }

    private void TickUpgrades(float deltaTime)
    {
        foreach (var upgrade in RegualarUpgrades)
        {
            upgrade.TickUpgrade(deltaTime);
        }
    }
}
